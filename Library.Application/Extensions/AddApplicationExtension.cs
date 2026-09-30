using Library.Application.Interfaces;
using Library.Application.Mapper;
using Library.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Extensions
{
    public static class AddApplicationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddAutoMapper(cfg => { }, typeof(BookMapper), typeof(AuthorMapper), typeof(LoanMapper));

            return services;
        }
    }
}
