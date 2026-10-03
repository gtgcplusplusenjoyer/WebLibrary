using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<T?> GetById(Guid id, CancellationToken cancellationToken = default);
        Task Add(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
