using FluentValidation;
using Library.Application.Dto;

namespace Library.Application.Validators
{
    public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Название книги обязательно")
                .MaximumLength(200)
                .WithMessage("Название не должно превышать 200 символов")
                .MinimumLength(3)
                .WithMessage("Название должно содержать минимум 3 символа");

            RuleFor(x => x.Publisher)
                .NotEmpty()
                .WithMessage("Издательство обязательно")
                .MaximumLength(100)
                .WithMessage("Издательство не должно превышать 100 символов");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Описание не должно превышать 1000 символов")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.TotalCopies)
                .GreaterThan(0)
                .WithMessage("Количество экземпляров должно быть больше 0")
                .LessThanOrEqualTo(1000)
                .WithMessage("Количество экземпляров не должно превышать 1000");
        }
    }
}
