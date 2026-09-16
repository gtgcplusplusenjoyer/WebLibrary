using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DbSet<Author> _authors;
        private readonly LibraryDbContext _context;
        public AuthorRepository(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _authors = _context.Set<Author>();
        }

        public async Task Add(Author author, CancellationToken cancellationToken = default)
        {
            await _authors.AddAsync(author, cancellationToken);
        }

        public void DeleteAuthor(Author author)
        {
            _authors.Remove(author);
        }

        public async Task<IEnumerable<Author>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _authors
                .AsNoTracking()
                .OrderBy(a => a.LastName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Author?> GetAuthorById(Guid id, CancellationToken cancellationToken)
        {
            return await _authors.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Author>> GetByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _authors
                .Where(a => ids.Contains(a.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(Author author)
        {
            _authors.Update(author);
        }
    }
}
