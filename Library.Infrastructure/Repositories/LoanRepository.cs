using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryDbContext _context;
        private readonly DbSet<Loan> _loans;
        public LoanRepository(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _loans = _context.Set<Loan>();
        }

        public async Task Add(Loan loan, CancellationToken cancellationToken = default)
        {
            await _loans.AddAsync(loan, cancellationToken);
        }

        public async Task<IEnumerable<Loan>> GetActiveLoans(CancellationToken cancellationToken = default)
        {
            return await _loans
                .AsNoTracking()
                .Include(l => l.Book)
                .OrderBy(l => l.LoanDate)
                .Where(l => l.Status == LoanStatus.Active) 
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Loan>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _loans
                .AsNoTracking()
                .Include(l => l.Book)
                .OrderBy(l => l.LoanDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Loan>> GetByBookId(Guid id, CancellationToken cancellationToken = default)
        {
            return await _loans
                .AsNoTracking()
                .Include(l => l.Book)
                .Where(l => l.BookId == id)
                .ToListAsync(cancellationToken);
        }

        public async Task<Loan?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoans(CancellationToken cancellationToken = default)
        {
            return await _loans
                .AsNoTracking()
                .Include(l => l.Book)
                .OrderBy(l => l.DueDate)
                .Where(l => l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(Loan loan)
        {
            _loans.Update(loan);
        }
    }
}
