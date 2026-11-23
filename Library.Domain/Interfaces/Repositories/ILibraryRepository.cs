using Library.Domain.Entities;

namespace Library.Domain.Interfaces.Repositories
{
    public interface ILibraryRepository : IBaseRepository<Book>, IDeletableRepository<Book>
    {
    }
}
