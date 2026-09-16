using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly DbSet<Book> _books;
        private readonly LibraryDbContext _context;
        public LibraryRepository(LibraryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _books = _context.Set<Book>();
        }

        public async Task Add(Book book, CancellationToken cancellationToken = default)
        {
            await _books.AddAsync(book, cancellationToken);
        }

        public void DeleteBook(Book book)
        {
            _books.Remove(book);
        }

        public async Task<IEnumerable<Book>> GetAllBooks(
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            return await _books
                .AsNoTracking()
                .OrderBy(f => f.Title) 
                .Skip((pageNumber - 1) * pageSize)
                .Take( pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Book?> GetBookById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _books
                .Include(b =>b.BookAuthors)
                .ThenInclude(ba=>ba.Author)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(Book book)
        {
            _books.Update(book);
        } 
    }
}
