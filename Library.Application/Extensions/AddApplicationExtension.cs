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
            services.AddAutoMapper(cfg => { }, typeof(BookMapper));

            return services;
        }
    }
}
