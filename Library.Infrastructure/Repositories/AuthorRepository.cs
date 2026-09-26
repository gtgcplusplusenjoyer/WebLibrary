using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class AuthorRepository : BaseRepository<Author, LibraryDbContext>, IAuthorRepository
    {
        public AuthorRepository(LibraryDbContext context) : base(context) 
        {
            
        }     
        public async Task<IEnumerable<Author>> GetByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(a => ids.Contains(a.Id))
                .ToListAsync(cancellationToken);
        }  
        public void Delete(Author entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
