using AutoMapper;
using Library.Application.Dto;
using Library.Application.Exceptions;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;

namespace Library.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ILibraryRepository _bookRepository;
        private readonly IMapper _mapper;
        public LoanService(ILoanRepository loanRepository, ILibraryRepository bookRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<LoanResponseDto> CreateLoanAsync(CreateLoanDto createLoanDto, CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetBookById(createLoanDto.BookId, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException($"Book with Id: {createLoanDto.BookId} not found");
            }

            if (book.AvailableCopies < 1)
            {
                throw new InvalidOperationException($"Book with Id: {createLoanDto.BookId} doesn't have available copy");
            }

            book.AvailableCopies--;
            _bookRepository.Update(book);

            var loan = _mapper.Map<Loan>(createLoanDto);

            await _loanRepository.Add(loan, cancellationToken);
            await _loanRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<LoanResponseDto>(loan);
        }

        public async Task<IEnumerable<LoanResponseDto>> GetActiveLoansAsync(CancellationToken cancellationToken = default)
        {
            var loans = await _loanRepository.GetActiveLoans(cancellationToken);

            return _mapper.Map<List<LoanResponseDto>>(loans);
        }

        public async Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync(
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var loans = await _loanRepository.GetAll(pageNumber, pageSize, cancellationToken);

            return _mapper.Map<List<LoanResponseDto>>(loans);
        }

        public async Task<LoanResponseDto> GetLoanByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var loan = await _loanRepository.GetById(id, cancellationToken);

            if(loan == null)
            {
                throw new NotFoundException($"Loan with Id: {id} not found");
            }

            return _mapper.Map<LoanResponseDto>(loan);
        }

        public async Task<IEnumerable<LoanResponseDto>> GetLoansByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default)
        {
            var loan = await _loanRepository.GetByBookId(bookId, cancellationToken); 

            return _mapper.Map<List<LoanResponseDto>>(loan);

        }

        public async Task<IEnumerable<LoanResponseDto>> GetOverdueLoansAsync(CancellationToken cancellationToken = default)
        {
            var loans = await _loanRepository.GetOverdueLoans(cancellationToken);

            return _mapper.Map<List<LoanResponseDto>>(loans);
        }

        public async Task<LoanResponseDto> ReturnLoanAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var loan = await _loanRepository.GetById(id, cancellationToken);

            if(loan == null)
            {
                throw new NotFoundException($"Loan with Id: {id} not found");
            }

            if(loan.Status != LoanStatus.Active)
            {
                throw new InvalidOperationException($"Loan with Id: {id} is not active");
            }

            loan.ReturnDate = DateTime.UtcNow;
            loan.Status = LoanStatus.Returned;

            var book = await _bookRepository.GetBookById(loan.BookId, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException($"Book with Id: {loan.BookId} not found");
            }

            book.AvailableCopies++;

            _bookRepository.Update(book);
            _loanRepository.Update(loan);
            await _loanRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<LoanResponseDto>(loan);
        }

        public async Task<LoanResponseDto> UpdateLoanAsync(Guid id, UpdateLoanDto updateLoanDto, CancellationToken cancellationToken = default)
        {
            var loan = await _loanRepository.GetById(id, cancellationToken);

            if (loan == null)
            {
                throw new NotFoundException($"Loan with Id: {id} is not found");
            }

            if(updateLoanDto.BookId != loan.BookId)
            {
                var oldBook = await _bookRepository.GetBookById(loan.BookId, cancellationToken);

                if (oldBook == null)
                {
                    throw new NotFoundException($"Book with Id: {loan.BookId} not found");
                }

                oldBook.AvailableCopies++;
                _bookRepository.Update(oldBook);

                var newBook = await _bookRepository.GetBookById(updateLoanDto.BookId, cancellationToken);

                if (newBook == null)
                {
                    throw new NotFoundException($"Book with Id: {updateLoanDto.BookId} not found");
                }

                if (newBook.AvailableCopies < 1)
                {
                    throw new InvalidOperationException($"Book with Id: {updateLoanDto.BookId} doesn't have available copy");
                }

                newBook.AvailableCopies--;
                _bookRepository.Update(newBook);

                loan.BookId = newBook.Id;
            }

            loan.DueDate = updateLoanDto.DueDate;
            loan.Status = updateLoanDto.Status;

            _loanRepository.Update(loan);
            await _loanRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<LoanResponseDto>(loan);
        }
    }
} 