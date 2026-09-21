using FluentValidation;
using Library.Application.Dto;
using Library.Application.Dto.Author;
using Library.Application.Dto.Book;
using Library.Application.Dto.Loan;
using Library.Application.Validators;
using Library.Application.Validators.Author;
using Library.Application.Validators.Book;
using Library.Application.Validators.Loan;
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
            services.AddScoped<IValidator<CreateAuthorDto>, CreateAuthorDtoValidator>();
            services.AddScoped<IValidator<UpdateAuthorDto>, UpdateAuthorDtoValidator>();
            services.AddScoped<IValidator<CreateLoanDto>, CreateLoanDtoValidator>();
            services.AddScoped<IValidator<UpdateLoanDto>, UpdateLoanDtoValidator>();

            services.AddFluentValidationAutoValidation();

            return services;
        }
    }
}
