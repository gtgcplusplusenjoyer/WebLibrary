using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IAuthorRepository : IBaseRepository<Author>, IDeletableRepository<Author>
    {
        Task<IEnumerable<Author>> GetByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    }
}
