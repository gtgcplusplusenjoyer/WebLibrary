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

        public async Task RevokeAllTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _dbSet
                .Where(r => r.UserId == userId && r.RevokedAt == null)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
            {
                token.RevokedAt = DateTime.UtcNow;
            }

        }

        public async Task RevokeAsync(Guid tokenId, CancellationToken cancellationToken = default)
        {
            var token = await _dbSet.FindAsync(tokenId, cancellationToken);

            if(token != null)
            {
                token.RevokedAt = DateTime.UtcNow;
            }
        }
    }
}
