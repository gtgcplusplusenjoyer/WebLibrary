using AutoMapper;
using FluentAssertions;
using Library.Application.Dto;
using Library.Application.Dto.Loan;
using Library.Application.Exceptions;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Moq;

namespace Library.Tests.Services
{
    public class LoanServiceTests
    {
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<ILibraryRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly LoanService _service;
        public LoanServiceTests()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _bookRepositoryMock = new Mock<ILibraryRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new LoanService(_loanRepositoryMock.Object, _bookRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetLoanByIdAsync_WhenLoanExist_ReturnsLoanResponseDto()
        {
            var loanId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            var loan = new Loan
            {
                Id = loanId,
                BookId = bookId,
                CreatedAt = DateTime.UtcNow,
                Status = Domain.Enums.LoanStatus.Active,
                LoanDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30),
                ReturnDate = null
            };

            var loanResponse = new LoanResponseDto(loanId, loan.CreatedAt, bookId, "Name", loan.LoanDate, loan.DueDate, loan.ReturnDate, loan.Status);

            _loanRepositoryMock
                .Setup(r => r.GetById(loanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loan);

            _mapperMock
                .Setup(m => m.Map<LoanResponseDto>(loan))
                .Returns(loanResponse);

            var result = await _service.GetLoanByIdAsync(loanId, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(loanId);
            result.BookId.Should().Be(bookId);
        }

        [Fact]
        public async Task GetLoanByIdAsync_WhenLoanDoesNotExist_ThrowsNotFoundException()
        {
            var loanId = Guid.NewGuid();

            _loanRepositoryMock
                .Setup(r => r.GetById(loanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Loan?)null);

            Func<Task> act = async () =>
            {
                await _service.GetLoanByIdAsync(loanId, CancellationToken.None);
            };

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Loan with Id: {loanId} not found");
        }

        [Fact]
        public async Task GetAllLoansAsync_WhenLoansExist_ReturnsLoanResponseDtos()
        {
            var loans = new List<Loan>()
            {
                new Loan
                {
                    Id = Guid.NewGuid(),
                    BookId = Guid.NewGuid(),
                    Status = Domain.Enums.LoanStatus.Active,
                },

                new Loan
                {
                    Id = Guid.NewGuid(),
                    BookId = Guid.NewGuid(),
                    Status = Domain.Enums.LoanStatus.Returned,
                }
            };

            var loansResponseDtos = new List<LoanResponseDto>()
            {
                new LoanResponseDto(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), "Book1",
                DateTime.UtcNow, DateTime.UtcNow, null, Domain.Enums.LoanStatus.Active),

                new LoanResponseDto(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), "Book2",
                DateTime.UtcNow, DateTime.UtcNow, null, Domain.Enums.LoanStatus.Returned)
            };

            _loanRepositoryMock
                .Setup(r => r.GetAll(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loans);

            _mapperMock
                .Setup(m => m.Map<List<LoanResponseDto>>(loans))
                .Returns(loansResponseDtos);

            var result = await _service.GetAllLoansAsync(1, 10, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllLoansAsync_WhenNoLoans_ReturnsEmptyList()
        {
            var loans = new List<Loan>();

            var loansResponseDtos = new List<LoanResponseDto>();

            _loanRepositoryMock
                .Setup(r => r.GetAll(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loans);

            _mapperMock
                .Setup(m => m.Map<List<LoanResponseDto>>(loans))
                .Returns(loansResponseDtos);

            var result = await _service.GetAllLoansAsync(1, 10, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetActiveLoansAsync_WhenLoansExist_ReturnsActiveLoans()
        {
            var loans = new List<Loan>()
            {
                new Loan
                {
                    Id = Guid.NewGuid(),
                    BookId = Guid.NewGuid(),
                    Status = Domain.Enums.LoanStatus.Active,
                },

                new Loan
                {
                    Id = Guid.NewGuid(),
                    BookId = Guid.NewGuid(),
                    Status = Domain.Enums.LoanStatus.Active,
                }
            };

            var loansResponseDtos = new List<LoanResponseDto>()
            {
                new LoanResponseDto(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), "Book1",
                DateTime.UtcNow, DateTime.UtcNow, null, Domain.Enums.LoanStatus.Active),

                new LoanResponseDto(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), "Book2",
                DateTime.UtcNow, DateTime.UtcNow, null, Domain.Enums.LoanStatus.Active)
            };

            _loanRepositoryMock
                .Setup(r => r.GetActiveLoans(It.IsAny<CancellationToken>()))
                .ReturnsAsync(loans);

            _mapperMock
                .Setup(m => m.Map<List<LoanResponseDto>>(loans))
                .Returns(loansResponseDtos);

            var result = await _service.GetActiveLoansAsync(CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetActiveLoansAsync_WhenNoLoans_ReturnsEmptyList()
        {
            var loans = new List<Loan>();

            var loansResponseDtos = new List<LoanResponseDto>();

            _loanRepositoryMock
                .Setup(r => r.GetActiveLoans(It.IsAny<CancellationToken>()))
                .ReturnsAsync(loans);

            _mapperMock
                .Setup(m => m.Map<List<LoanResponseDto>>(loans))
                .Returns(loansResponseDtos);

            var result = await _service.GetActiveLoansAsync(CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        //
        [Fact]
        public async Task GetOverdueLoansAsync_WhenLoansExist_ReturnsOverdueLoans()
        {
            var loans = new List<Loan>()
            {
                new Loan
                {
                    Id = Guid.NewGuid(),
                    BookId = Guid.NewGuid(),
                    Status = Domain.Enums.LoanStatus.Overdue,
                },

                new Loan
                {
                    Id = Guid.NewGuid(),
                    BookId = Guid.NewGuid(),
                    Status = Domain.Enums.LoanStatus.Overdue,
                }
            };

            var loansResponseDtos = new List<LoanResponseDto>()
            {
                new LoanResponseDto(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), "Book1",
                DateTime.UtcNow, DateTime.UtcNow, null, Domain.Enums.LoanStatus.Overdue),

                new LoanResponseDto(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), "Book2",
                DateTime.UtcNow, DateTime.UtcNow, null, Domain.Enums.LoanStatus.Overdue)
            };

            _loanRepositoryMock
                .Setup(r => r.GetOverdueLoans(It.IsAny<CancellationToken>()))
                .ReturnsAsync(loans);

            _mapperMock
                .Setup(m => m.Map<List<LoanResponseDto>>(loans))
                .Returns(loansResponseDtos);

            var result = await _service.GetOverdueLoansAsync(CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetOverdueLoansAsync_WhenNoLoans_ReturnsEmptyList()
        {
            var loans = new List<Loan>();

            var loansResponseDtos = new List<LoanResponseDto>();

            _loanRepositoryMock
                .Setup(r => r.GetOverdueLoans(It.IsAny<CancellationToken>()))
                .ReturnsAsync(loans);

            _mapperMock
                .Setup(m => m.Map<List<LoanResponseDto>>(loans))
                .Returns(loansResponseDtos);

            var result = await _service.GetOverdueLoansAsync(CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }


        [Fact]
        public async Task CreateLoanAsync_WhenBookAvailable_CreatesLoan()
        {
            var bookId = Guid.NewGuid();
            var book = new Book
            {
                Id = bookId,
                Title = "Война и мир",
                AvailableCopies = 3
            };

            var createDto = new CreateLoanDto(bookId, 30);

            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                BookId = bookId,
                CreatedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(createDto.LoanDays),
                ReturnDate = null,
                LoanDate = DateTime.UtcNow,
                Status = Domain.Enums.LoanStatus.Active,
                Book = book,
            };

            var loanDto = new LoanResponseDto(
                loan.Id, DateTime.UtcNow, bookId, "Война и мир",
                loan.LoanDate, loan.DueDate, null, LoanStatus.Active);

            _bookRepositoryMock
                .Setup(r => r.GetById(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _mapperMock
                .Setup(m => m.Map<Loan>(createDto))
                .Returns(loan);

            _loanRepositoryMock
                .Setup(r => r.GetById(loan.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loan);

            _mapperMock
                .Setup(m => m.Map<LoanResponseDto>(loan))
                .Returns(loanDto);

            var result = await _service.CreateLoanAsync(createDto, CancellationToken.None);

            result.Should().NotBeNull();
            result.BookId.Should().Be(bookId);
            book.AvailableCopies.Should().Be(2);
        }

        [Fact]
        public async Task CreateLoanAsync_WhenBookNotFound_ThrowsNotFoundException()
        {
            var bookId = Guid.NewGuid();
            var createDto = new CreateLoanDto(bookId, 30);

            _bookRepositoryMock
                .Setup(r => r.GetById(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Book?)null);

            Func<Task> act = async () =>
            {
                await _service.CreateLoanAsync(createDto, CancellationToken.None);
            };

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Book with Id: {bookId} not found");
        }

        [Fact]
        public async Task CreateLoanAsync_WhenNoAvailableCopies_ThrowsInvalidOperationException()
        {
            var bookId = Guid.NewGuid();
            var book = new Book
            {
                Id = bookId,
                Title = "Война и мир",
                AvailableCopies = 0
            };

            var createDto = new CreateLoanDto(bookId, 30);

            _bookRepositoryMock
                .Setup(r => r.GetById(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            Func<Task> act = async () =>
            {
                await _service.CreateLoanAsync(createDto, It.IsAny<CancellationToken>());
            };

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Book with Id: {bookId} doesn't have available copy");
        }

        [Fact]
        public async Task GetLoansByBookIdAsync_WhenLoanExist_ReturnsLoanResponse()
        {
            var bookId = Guid.NewGuid();
            var books = new List<Book>
            {
                new Book{
                Id = Guid.NewGuid(),
                AvailableCopies = 3,
                Title = "Book1"
                },

                new Book{
                Id = Guid.NewGuid(),
                AvailableCopies = 2,
                Title = "Book2"
                }
            };

            var loans = new List<Loan>
            {
                new Loan{
                Id = Guid.NewGuid(),
                BookId = Guid.NewGuid(),
                Status = LoanStatus.Active,
                },

                new Loan{
                Id = Guid.NewGuid(),
                BookId = Guid.NewGuid(),
                Status = LoanStatus.Active,
                }
            };

            var responseDtos = new List<LoanResponseDto>{
                new LoanResponseDto(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(),
                "Book1", DateTime.UtcNow, DateTime.UtcNow, null, LoanStatus.Active),

                new LoanResponseDto(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(),
                "Book2", DateTime.UtcNow, DateTime.UtcNow, null, LoanStatus.Active),
            };

            _loanRepositoryMock
                .Setup(r => r.GetByBookId(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loans);

            _mapperMock
                .Setup(m => m.Map<List<LoanResponseDto>>(loans))
                .Returns(responseDtos);

            var result = await _service.GetLoansByBookIdAsync(bookId, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetLoansByBookIdAsync_WhenLoanNotExists_ReturnsEmptyList()
        {
            var bookId = Guid.NewGuid();

            var loans = new List<Loan>();

            var responseDtos = new List<LoanResponseDto>();

            _loanRepositoryMock
                .Setup(r => r.GetByBookId(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loans);

            _mapperMock
                .Setup(m => m.Map<List<LoanResponseDto>>(loans))
                .Returns(responseDtos);

            var result = await _service.GetLoansByBookIdAsync(bookId, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task ReturnLoanAsync_WhenLoanExists_ReturnsReturnedLoan()
        {
            var bookId = Guid.NewGuid();

            var book = new Book
            {
                Id = bookId,
                Title = "Book1",
                AvailableCopies = 2,
            };

            var loanId = Guid.NewGuid();

            var loan = new Loan()
            {
                Id = loanId,
                BookId = bookId,
                Status = LoanStatus.Active
            };

            var returnedLoan = new Loan
            {
                Id = loanId,
                BookId = bookId,
                Status = LoanStatus.Returned,
                ReturnDate = DateTime.UtcNow
            };

            var responseDto = new LoanResponseDto(loanId, DateTime.UtcNow, bookId, "Book1",
                DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, LoanStatus.Returned);

            _loanRepositoryMock
                .SetupSequence(r => r.GetById(loanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loan)
                .ReturnsAsync(returnedLoan);

            _bookRepositoryMock
                .Setup(r => r.GetById(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _mapperMock
                .Setup(m => m.Map<LoanResponseDto>(returnedLoan))
                .Returns(responseDto);

            var result = await _service.ReturnLoanAsync(loanId, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(loanId);
            result.Status.Should().Be(LoanStatus.Returned);
            book.AvailableCopies.Should().Be(3);
        }

        [Fact]
        public async Task ReturnLoanAsync_WhenLoanNotFound_ThrowsNotFoundException()
        {
            var loanId = Guid.NewGuid();

            var loan = new Loan();

            _loanRepositoryMock
                .Setup(r => r.GetById(loanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Loan?)null);

            Func<Task> act = async () =>
            {
                await _service.ReturnLoanAsync(loanId, It.IsAny<CancellationToken>());
            };

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Loan with Id: {loanId} not found");
        }

        [Fact]
        public async Task ReturnLoanAsync_WhenLoanIsNotActive_ThrowsInvalidOperationException()
        {
            var loanId = Guid.NewGuid();

            var loan = new Loan()
            {
                Id = loanId,
                BookId = Guid.NewGuid(),
                Status = LoanStatus.Returned
            };

            _loanRepositoryMock
                .Setup(r => r.GetById(loanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loan);

            Func<Task> act = async () =>
            {
                await _service.ReturnLoanAsync(loanId, It.IsAny<CancellationToken>());
            };

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Loan with Id: {loanId} is not active");
        }

        [Fact]
        public async Task UpdateLoanAsync_WhenBookIdChanged_UpdatesBothBooks()
        {
            var oldBookId = Guid.NewGuid();
            var newBookId = Guid.NewGuid();

            var oldBook = new Book
            {
                Id = oldBookId,
                AvailableCopies = 2,
                Title = "Old"
            };

            var newBook = new Book
            {
                Id = newBookId,
                AvailableCopies = 3,
                Title = "New"
            };

            var loanId = Guid.NewGuid();

            var loan = new Loan
            {
                Id = loanId,
                BookId = oldBookId,
                Status = LoanStatus.Active,
            };

            var updateLoanDto = new UpdateLoanDto(newBookId, DateTime.UtcNow.AddDays(30), LoanStatus.Active);

            var returnedLoan = new Loan
            {
                Id = loanId,
                BookId = newBookId,
                Status = LoanStatus.Active,
            };

            var responseDto = new LoanResponseDto(Guid.NewGuid(), DateTime.UtcNow, newBookId, "New", DateTime.UtcNow, DateTime.UtcNow.AddDays(30),
                null, LoanStatus.Active);

            _loanRepositoryMock
                .SetupSequence(r => r.GetById(loanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loan)
                .ReturnsAsync(returnedLoan);

            _bookRepositoryMock
                .Setup(r => r.GetById(oldBookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(oldBook);

            _bookRepositoryMock
                .Setup(r => r.GetById(newBookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newBook);

            _mapperMock
                .Setup(m => m.Map<LoanResponseDto>(returnedLoan))
                .Returns(responseDto);

            var result = await _service.UpdateLoanAsync(loanId, updateLoanDto, CancellationToken.None);

            result.Should().NotBeNull();
            result.BookId.Should().Be(newBookId);
            oldBook.AvailableCopies.Should().Be(3);
            newBook.AvailableCopies.Should().Be(2);
        }

        [Fact]
        public async Task UpdateLoanAsync_WhenLoanNotFound_ThrowsNotFoundException()
        {
            var loanId = Guid.NewGuid();

            var updateDto = new UpdateLoanDto(Guid.NewGuid(), DateTime.UtcNow.AddDays(30), LoanStatus.Active);

            _loanRepositoryMock
                .Setup(r => r.GetById(loanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Loan?)null);

            Func<Task> act = async () =>
            {
                await _service.UpdateLoanAsync(loanId, updateDto, CancellationToken.None);
            };

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Loan with Id: {loanId} is not found");
        }

        [Fact]
        public async Task UpdateLoanAsync_WhenBookIdSame_UpdatesOnlyLoan()
        {
            var loanId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            var loan = new Loan
            {
                Id = loanId,
                BookId = bookId,
                Status = LoanStatus.Active,
                DueDate = DateTime.UtcNow.AddDays(30)
            };

            var updateDto = new UpdateLoanDto(bookId, DateTime.UtcNow.AddDays(60), LoanStatus.Active);

            var updatedLoan = new Loan { Id = loanId, BookId = bookId, Status = LoanStatus.Active };

            var loanDto = new LoanResponseDto(loanId, DateTime.UtcNow, bookId, "Book1",
                DateTime.UtcNow, DateTime.UtcNow.AddDays(60), null, LoanStatus.Active);

            _loanRepositoryMock
                .SetupSequence(r => r.GetById(loanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loan)
                .ReturnsAsync(updatedLoan);

            _mapperMock
                .Setup(m => m.Map<LoanResponseDto>(updatedLoan))
                .Returns(loanDto);

            var result = await _service.UpdateLoanAsync(loanId, updateDto, CancellationToken.None);

            result.Should().NotBeNull();
            result.BookId.Should().Be(bookId);
            _bookRepositoryMock.Verify(
                r => r.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

    }
}
