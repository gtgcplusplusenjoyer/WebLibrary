using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface ILibraryRepository : IBaseRepository<Book>, IDeletableRepository<Book>
    {
    }
}
