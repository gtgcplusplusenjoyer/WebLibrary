using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class LoanRepository : BaseRepository<Loan, LibraryDbContext>, ILoanRepository
    {
        public LoanRepository(LibraryDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Loan>> GetActiveLoans(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(l => l.Book)
                .OrderBy(l => l.LoanDate)
                .Where(l => l.Status == LoanStatus.Active)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Loan>> GetByBookId(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(l => l.Book)
                .Where(l => l.BookId == id)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoans(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(l => l.Book)
                .OrderBy(l => l.DueDate)
                .Where(l => l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow)
                .ToListAsync(cancellationToken);
        }
    }
}
