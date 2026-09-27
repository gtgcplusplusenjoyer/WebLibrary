using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class LibraryRepository : BaseRepository<Book, LibraryDbContext>, ILibraryRepository
    {
        public LibraryRepository(LibraryDbContext context) : base(context)
        {

        }
        public void Delete(Book entity)
        {
            _dbSet.Remove(entity);
        }
        protected override IQueryable<Book> ApplyIncludes(IQueryable<Book> query)
        {
            return query
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author);
        }
    }
}
