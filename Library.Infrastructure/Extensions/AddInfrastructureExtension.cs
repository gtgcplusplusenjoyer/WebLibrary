using Library.Domain.Interfaces;
using Library.Domain.Interfaces.External;
using Library.Infrastructure.Context;
using Library.Infrastructure.External;
using Library.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Extensions
{
    public static class AddInfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IReportRepository, DapperReportRepository>();
            services.AddScoped<IDashboardRepository, DapperDashboardRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddDbContext<LibraryDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString(nameof(LibraryDbContext)));
            });

            return services;
        }
    }
}
