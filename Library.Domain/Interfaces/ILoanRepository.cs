using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface ILoanRepository
    {
        Task Add(Loan loan, CancellationToken cancellationToken = default);
        Task<Loan?> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Loan>> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<IEnumerable<Loan>> GetByBookId(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Loan>> GetActiveLoans(CancellationToken cancellationToken = default);
        Task<IEnumerable<Loan>> GetOverdueLoans(CancellationToken cancellationToken = default);
        void Update(Loan loan);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
