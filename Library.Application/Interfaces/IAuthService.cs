using Library.Application.Dto.User;
using Library.Domain.Dto.User;

namespace Library.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken = default);
        Task<AuthResult> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken = default);
        Task<AuthResult> LogoutAsync(Guid UserId, CancellationToken cancellationToken = default);
        Task<AuthResult> RefreshAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default);
    }
}
