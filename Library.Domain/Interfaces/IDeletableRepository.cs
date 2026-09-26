using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IDeletableRepository<T> where T : BaseEntity
    {
        void Delete(T entity);
    }
}
