using FluentValidation;
using Library.Application.Dto;

namespace Library.Application.Validators
{
    public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
    {
        public UpdateAuthorDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Имя автора обязательно")
                .MaximumLength(100)
                .WithMessage("Имя не должно превышать 100 символов")
                .MinimumLength(2)
                .WithMessage("Имя должно содержать минимум 2 символа");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Фамилия автора обязательна")
                .MaximumLength(100)
                .WithMessage("Фамилия не должна превышать 100 символов")
                .MinimumLength(2)
                .WithMessage("Фамилия должна содержать минимум 2 символа");

            RuleFor(x => x.Biography)
                .MaximumLength(2000)
                .WithMessage("Биография не должна превышать 2000 символов")
                .When(x => !string.IsNullOrEmpty(x.Biography));
        }
    }
}
