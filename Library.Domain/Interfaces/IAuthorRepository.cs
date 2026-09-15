using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        Task Add(Author author, CancellationToken cancellationToken = default);
        Task<Author?> GetAuthorById(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Author>> GetAll(
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<Author>> GetByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        void DeleteAuthor(Author author);
        void Update(Author author);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
