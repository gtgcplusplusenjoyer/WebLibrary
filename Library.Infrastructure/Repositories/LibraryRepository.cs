using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async Task Add(Book book, CancellationToken cancellationToken)
        {
            await _books.AddAsync(book, cancellationToken);
        }

        public void DeleteBook(Book book)
        {
            _books.Remove(book);
        }

        public async Task<IEnumerable<Book>> GetAllBooks(CancellationToken cancellationToken)
        {
            return await _books
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Book?> GetBookById(Guid id, CancellationToken cancellationToken)
        {
            return await _books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(Book book)
        {
            _books.Update(book);
        }
    }
}
