using Library.Domain.Interfaces;
using Library.Domain.Interfaces.External;
using Library.Infrastructure.Context;
using Library.Infrastructure.External;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Library.Infrastructure.Extensions
{
    public static class AddInfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LibraryDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString(nameof(LibraryDbContext)));
            });

            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IReportRepository, DapperReportRepository>();
            services.AddScoped<IDashboardRepository, DapperDashboardRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddAuthExtension(configuration);

            return services;
        }
        public static IServiceCollection AddAuthExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthSettings>(configuration.GetSection("Auth"));
            var authSettings = configuration.GetSection("Auth").Get<AuthSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authSettings?.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authSettings?.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(authSettings?.Secret ?? "fallback-secret-key-32-chars-long!!!")),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

            return services;
        }
    }
}
