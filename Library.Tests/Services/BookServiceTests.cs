using AutoMapper;
using FluentAssertions;
using Library.Application.Dto;
using Library.Application.Dto.Author;
using Library.Application.Dto.Book;
using Library.Application.Exceptions;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Moq;

namespace Library.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<ILibraryRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly BookService _service;
        public BookServiceTests()
        {
            _repositoryMock = new Mock<ILibraryRepository>();
            _mapperMock = new Mock<IMapper>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _service = new BookService(_repositoryMock.Object, _authorRepositoryMock.Object, _mapperMock.Object); 
        }

        [Fact]
        public async Task GetAllBooksAsync_WhenBooksExist_ReturnsBookResponseDtos()
        {
            // Arrange (Подготовка)
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Война и мир", Publisher = "АСТ" },
                new Book { Id = Guid.NewGuid(), Title = "Преступление и наказание", Publisher = "Эксмо" }
            };

            var bookDtos = new List<BookResponseDto>
            {
                new BookResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Война и мир", "АСТ", null, 1, 1, new List<AuthorResponseDto>()),
                new BookResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Преступление и наказание", "Эксмо", null, 1, 1,
                    new List<AuthorResponseDto>())
            };

            _repositoryMock
                .Setup(r => r.GetAllBooks(1,
                    10, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(books);

            _mapperMock
                .Setup(m => m.Map<List<BookResponseDto>>(books))
                .Returns(bookDtos);

            // Act (Действие)
            var result = await _service.GetAllBooksAsync(
                    1,
                    10,
                    CancellationToken.None);

            // Assert (Проверка)
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(bookDtos);
        }

        [Fact]
        public async Task GetAllBooksAsync_WhenNoBooks_ReturnsEmptyList()
        {
            var books = new List<Book>();
            var bookDtos = new List<BookResponseDto>();

            _repositoryMock
                .Setup(r => r.GetAllBooks(
                    1,
                    10,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(books);

            _mapperMock
                .Setup(m => m.Map<List<BookResponseDto>>(books))
                .Returns(bookDtos);


            var result = await _service.GetAllBooksAsync(
                    1,
                    10, 
                    CancellationToken.None);


            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBookByIdAsync_WhenBookExist_ReturnsResponseDto()
        {
            var bookId = Guid.NewGuid();
            var book = new Book()
            {
                Id = bookId,
                CreatedAt = DateTime.UtcNow,
                TotalCopies = 3,
                AvailableCopies = 2,
                Description = "Interesting book",
                Publisher = "Dantes",
                Title = "Hello world"
            };

            var bookResponseDto = new BookResponseDto
            (
                 bookId, DateTime.UtcNow, "Hello world", "Dantes", "Interesting book", 3, 2, new List<AuthorResponseDto>()
            );

            _repositoryMock
                .Setup(r => r.GetBookById(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _mapperMock
                .Setup(m => m.Map<BookResponseDto>(book))
                .Returns(bookResponseDto);


            var result = await _service.GetBookByIdAsync(bookId, CancellationToken.None);


            result.Should().NotBeNull();
            result.Id.Should().Be(bookId);
            result.Title.Should().Be("Hello world");
        }

        [Fact]
        public async Task GetBookByIdAsync_WhenBookNotExist_ThrowsNotFoundException()
        {
            var bookId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetBookById(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Book?)null);

            Func<Task> act = async () =>
            {
                await _service.GetBookByIdAsync(bookId, CancellationToken.None);
            };


            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Book with Id: {bookId} not found");
        }

        [Fact]
        public async Task CreateBookAsync_ValidDto_ReturnsBookResponseDto()
        {
            var id = Guid.NewGuid();
            var createBookDto = new CreateBookDto("Title", "Publisher", "desc", 1);

            var book = new Book
            {
                Id = id,
                Title = "Title",
                Publisher = "Publisher",
                Description = "desc",
                TotalCopies = 1
            };

            var bookResponseDto = new BookResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Title", "Publisher", "desc", 1, 1,
                new List<AuthorResponseDto>());

            _mapperMock
                .Setup(m => m.Map<Book>(createBookDto))
                .Returns(book);

            _repositoryMock
                .Setup(r => r.GetBookById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _mapperMock
                .Setup(m => m.Map<BookResponseDto>(book))
                .Returns(bookResponseDto);

            var result = await _service.CreateBookAsync(createBookDto, CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("Title");
            result.TotalCopies.Should().Be(1);
        }

        [Fact]
        public async Task CreateBookAsync_ValidDto_SetsAvailableCopiesEqualsTotalCopies()
        {
            var id = Guid.NewGuid();
            var createBookDto = new CreateBookDto("Title", "Publisher", "desc", 1);

            var book = new Book
            {
                Id = id,
                Title = "Title",
                Publisher = "Publisher",
                Description = "desc",
                TotalCopies = 1
            };

            var bookResponseDto = new BookResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Title", "Publisher", "desc", 1, 1,
                new List<AuthorResponseDto>());

            _mapperMock
                .Setup(m => m.Map<Book>(createBookDto))
                .Returns(book);

            _repositoryMock
                .Setup(r => r.GetBookById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _mapperMock
                .Setup(m => m.Map<BookResponseDto>(book))
                .Returns(bookResponseDto);

            var result = await _service.CreateBookAsync(createBookDto, CancellationToken.None);

            result.AvailableCopies.Should().Be(1);
        }

        [Fact]
        public async Task CreateBookAsync_ValidDto_CallsRepositoryAdd()
        {
            var createBookDto = new CreateBookDto("Title", "Publi", null, 1);

            var book = new Book
            {
                Title = "Title",
                Publisher = "Publi",
                Description = null,
                TotalCopies = 1
            };

            var responseBookDto = new BookResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Title", "Publi", null, 1, 1,
                new List<AuthorResponseDto>());

            _mapperMock
                .Setup(m => m.Map<Book>(createBookDto))
                .Returns(book);

            _mapperMock
                .Setup(m => m.Map<BookResponseDto>(book))
                .Returns(responseBookDto);


            await _service.CreateBookAsync(createBookDto, CancellationToken.None);

            _repositoryMock.Verify(r => r.Add(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateBookAsync_ValidDto_CallsSaveChanges()
        {
            var createBookDto = new CreateBookDto("Title", "Publi", null, 1);

            var book = new Book
            {
                Title = "Title",
                Publisher = "Publi",
                Description = null,
                TotalCopies = 1
            };

            var responseBookDto = new BookResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Title", "Publi", null, 1, 1,
                new List<AuthorResponseDto>());

            _mapperMock
                .Setup(m => m.Map<Book>(createBookDto))
                .Returns(book);

            _mapperMock
                .Setup(m => m.Map<BookResponseDto>(book))
                .Returns(responseBookDto);

            await _service.CreateBookAsync(createBookDto, CancellationToken.None);

            _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_WhenBookExists_DeletesBook()
        {
            var id = Guid.NewGuid();
            var book = new Book
            {
                Id = id,
                Title = "Title",
                Publisher = "Publi",
                AvailableCopies = 1,
                TotalCopies = 1,
                CreatedAt = DateTime.UtcNow,
                Description = null
            };

            _repositoryMock
                .Setup(r => r.GetBookById(id, CancellationToken.None))
                .ReturnsAsync(book);

            await _service.DeleteBookAsync(id, CancellationToken.None);

            _repositoryMock
                .Verify(r => r.DeleteBook(book), Times.Once);

            _repositoryMock
                .Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_WhenBookNotExists_ThrowsNotFoundException()
        {
            var id = Guid.NewGuid();


            _repositoryMock
                .Setup(r => r.GetBookById(id, CancellationToken.None))
                .ReturnsAsync((Book?)null);


            Func<Task> act = async () =>
            {
                await _service.DeleteBookAsync(id, CancellationToken.None);
            };


            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Book with Id: {id} not found");
        }

        [Fact]
        public async Task UpdateBookAsync_WhenBookExists_ReturnsUpdatedBook()
        {
            var bookId = Guid.NewGuid();

            var book = new Book
            {
                Id = bookId,
                Title = "Старое название",
                Publisher = "АСТ",
                TotalCopies = 5,
                AvailableCopies = 3
            };

            var updateDto = new UpdateBookDto("Новое название", "Эксмо", "Новое описание", 10);

            var updatedDto = new BookResponseDto(
        bookId, DateTime.UtcNow, "Новое название", "Эксмо", "Новое описание", 10, 3, new List<AuthorResponseDto>());

            _repositoryMock
                .Setup(r => r.GetBookById(bookId, CancellationToken.None))
                .ReturnsAsync(book);

            _mapperMock
                .Setup(m => m.Map<BookResponseDto>(book))
                .Returns(updatedDto);

            var result = await _service.UpdateBookAsync(bookId, updateDto, CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("Новое название");
            result.TotalCopies.Should().Be(10);
        }

        [Fact]
        public async Task UpdateBookAsync_WhenBookDoesNotExist_ThrowsNotFoundException()
        {
            var bookId = Guid.NewGuid();

            var updatedDto = new UpdateBookDto("t", "p", null, 1);

            _repositoryMock
                .Setup(r => r.GetBookById(bookId, CancellationToken.None))
                .ReturnsAsync((Book?)null);

            Func<Task> act = async () =>
            {
                await _service.UpdateBookAsync(bookId, updatedDto, CancellationToken.None);
            };

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Book with Id: {bookId} not found");
        }

        [Fact]
        public async Task UpdateBookAsync_DoesNotChangeAvailableCopies()
        {
            var bookId = Guid.NewGuid();

            var existingBook = new Book
            {
                Id = bookId,
                Title = "Старое",
                Publisher = "АСТ",
                TotalCopies = 5,
                AvailableCopies = 3
            };

            var dto = new UpdateBookDto("Новое", "Эксмо", null, 10);

            var responseDto = new BookResponseDto(bookId, DateTime.UtcNow, "Новое", "Эксмо", null, 10, 3, new List<AuthorResponseDto>());

            _repositoryMock
                .Setup(r => r.GetBookById(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingBook);

            _mapperMock
                .Setup(m => m.Map<BookResponseDto>(existingBook))
                .Returns(responseDto);

            await _service.UpdateBookAsync(bookId, dto, CancellationToken.None);

            existingBook.AvailableCopies.Should().Be(3);
            existingBook.TotalCopies.Should().Be(10);
        }

         
    }
}
