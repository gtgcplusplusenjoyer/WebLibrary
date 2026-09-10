using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface ILibraryRepository
    {
        Task Add(Book book, CancellationToken cancellationToken);
        Task<Book?> GetBookById(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Book>> GetAllBooks(CancellationToken cancellationToken);
        void DeleteBook(Book book);
        void Update(Book book);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
