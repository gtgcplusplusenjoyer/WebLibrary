using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken = default);
    }
}
