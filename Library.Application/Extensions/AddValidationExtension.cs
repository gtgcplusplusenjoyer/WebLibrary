using FluentValidation;
using Library.Application.Dto;
using Library.Application.Validators;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Library.Application.Extensions
{
    public static class AddValidationExtension
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateBookDto>, CreateBookDtoValidator>();
            services.AddScoped<IValidator<UpdateBookDto>, UpdateBookDtoValidator>();

            services.AddFluentValidationAutoValidation();

            return services;
        }
    }
}
