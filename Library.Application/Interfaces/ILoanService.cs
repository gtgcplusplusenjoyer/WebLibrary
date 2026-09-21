using Library.Application.Dto.Loan;

namespace Library.Application.Interfaces
{
    public interface ILoanService
    {
        Task<LoanResponseDto> CreateLoanAsync(CreateLoanDto createLoanDto, CancellationToken cancellationToken = default);
        Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync(
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
        Task<LoanResponseDto> GetLoanByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<LoanResponseDto>> GetLoansByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);
        Task<IEnumerable<LoanResponseDto>> GetActiveLoansAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<LoanResponseDto>> GetOverdueLoansAsync(CancellationToken cancellationToken = default);
        Task<LoanResponseDto> ReturnLoanAsync(Guid id, CancellationToken cancellationToken = default);
        Task<LoanResponseDto> UpdateLoanAsync(Guid id, UpdateLoanDto updateLoanDto, CancellationToken cancellationToken = default);

    }
}
