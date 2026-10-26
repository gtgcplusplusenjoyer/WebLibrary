using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
        Task RevokeAsync(Guid tokenId, CancellationToken cancellationToken = default);
        Task RevokeAllTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
