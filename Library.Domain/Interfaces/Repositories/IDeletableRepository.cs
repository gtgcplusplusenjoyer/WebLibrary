using Library.Domain.Entities;

namespace Library.Domain.Interfaces.Repositories
{
    public interface IDeletableRepository<T> where T : BaseEntity
    {
        void Delete(T entity);
    }
}
