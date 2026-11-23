using Library.Domain.Entities;

namespace Library.Domain.Interfaces.Repositories
{
    public interface ILoanRepository : IBaseRepository<Loan>
    {
        Task<IEnumerable<Loan>> GetByBookId(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Loan>> GetActiveLoans(CancellationToken cancellationToken = default);
        Task<IEnumerable<Loan>> GetOverdueLoans(CancellationToken cancellationToken = default);
    }
}
