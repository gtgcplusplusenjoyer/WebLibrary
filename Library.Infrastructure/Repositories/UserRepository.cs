using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User, LibraryDbContext>, IUserRepository
    {
        public UserRepository(LibraryDbContext context) : base(context) { }

        public async Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u=>u.Email == email,cancellationToken);
        }
    }
}
