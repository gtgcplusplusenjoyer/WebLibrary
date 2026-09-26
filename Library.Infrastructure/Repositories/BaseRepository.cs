using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class BaseRepository<T, TContext> : IBaseRepository<T>
        where T : BaseEntity
        where TContext : LibraryDbContext
    {
        protected readonly TContext _context;
        protected readonly DbSet<T> _dbSet;
        public BaseRepository(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }
        public async Task Add(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .OrderBy(e => e.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<T?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(e=>e.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
