using Library.Application.Dto.User;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.API.Controllers
{
    [Route("WebLibrary/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto, CancellationToken cancellationToken = default)
        {
            var result = await _authService.RegisterAsync(registerDto, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            return Ok(new AuthResponseDto(
                result.TokenPair!.AccessToken,
                result.TokenPair!.RefreshToken,
                result.UserId!.Value
                ));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto, CancellationToken cancellationToken = default)
        {
            var result = await _authService.LoginAsync(loginDto, cancellationToken);

            if (!result.IsSuccess)
            {
                return Unauthorized(new { error = result.ErrorMessage });
            }

            return Ok(new AuthResponseDto(
                result.TokenPair!.AccessToken,
                result.TokenPair!.RefreshToken,
                result.UserId!.Value
                ));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
        {
            var result = await _authService.RefreshAsync(refreshTokenRequestDto, cancellationToken);

            if (!result.IsSuccess)
            {
                return Unauthorized(new { error = result.ErrorMessage });
            }

            return Ok(new AuthResponseDto(
                result.TokenPair!.AccessToken,
                result.TokenPair!.RefreshToken,
                result.UserId!.Value));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest(new { error = "Invalid user identifier" });
            }

            var result = await _authService.LogoutAsync(userId, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            return Ok(new { message = "Logged out successfully" });
        }

    }
}
