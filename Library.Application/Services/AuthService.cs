using AutoMapper;
using Library.Application.Dto.User;
using Library.Application.Interfaces;
using Library.Domain.Dto.User;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Domain.Interfaces.External;

namespace Library.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMapper _mapper;
        public AuthService(IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IMapper mapper)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator; 
            _mapper = mapper;
        }
        public async Task<AuthResult> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByEmail(loginUserDto.Email, cancellationToken);

            if(user == null)
            {
                return AuthResult.Failure("User with this email not exist");
            }

            if(!_passwordHasher.VerifyPassword(loginUserDto.Password, user.PasswordHash))
            {
                return AuthResult.Failure("Invalid email or password");
            }

            var tokenPair = _tokenGenerator.GenerateTokenPair(user);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Token = tokenPair.RefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.Add(refreshToken,cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return AuthResult.Success(tokenPair, user.Id);
        }

        public async Task<AuthResult> LogoutAsync(Guid UserId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetById(UserId, cancellationToken);

            if (user == null)
            {
                return AuthResult.Failure("User not found");
            }

            await _refreshTokenRepository.RevokeAllTokensByUserIdAsync(UserId, cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
            
            return AuthResult.Success(user.Id);
        }

        public async Task<AuthResult> RefreshAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshTokenRequestDto.RefreshToken, cancellationToken);

            if(storedToken == null)
            { 
                return AuthResult.Failure("Refresh token is not found");
            }

            if(storedToken.IsRevoked)
            {
                return AuthResult.Failure("Refresh token has been revoked");
            }

            if(storedToken.ExpiresAt < DateTime.UtcNow)
            { 
                return AuthResult.Failure("Refresh token has expired");
            }

            var user = await _userRepository.GetById(storedToken.UserId, cancellationToken);

            if(user == null)
            {
                return AuthResult.Failure("User with this id is not found");
            }

            await _refreshTokenRepository.RevokeAsync(storedToken.Id, cancellationToken);

            var newTokenPair = _tokenGenerator.GenerateTokenPair(user);

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                Token = newTokenPair.RefreshToken,
                UserId = user.Id
            };

            await _refreshTokenRepository.Add(newRefreshToken, cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
             
            return AuthResult.Success(newTokenPair, user.Id);
        }

        public async Task<AuthResult> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken = default)
        {
            var existingUser = await _userRepository.GetUserByEmail(registerUserDto.Email,cancellationToken);

            if (existingUser != null)
            {
                return AuthResult.Failure("User with this email already exist");
            }

            var user = _mapper.Map<User>(registerUserDto);

            user.Id = Guid.NewGuid();
            user.PasswordHash = _passwordHasher.HashPassword(registerUserDto.Password);
            user.Role = "User";
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.Add(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            var tokenPair = _tokenGenerator.GenerateTokenPair(user);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Token = tokenPair.RefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
            };

            await _refreshTokenRepository.Add(refreshToken, cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return AuthResult.Success(tokenPair, user.Id);
        }
    }
}
