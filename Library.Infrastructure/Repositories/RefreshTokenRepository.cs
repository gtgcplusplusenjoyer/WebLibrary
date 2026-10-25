using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken, LibraryDbContext>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(r => r.User)
                .FirstOrDefaultAsync(r=>r.Token == token, cancellationToken);
        }
    }
}
