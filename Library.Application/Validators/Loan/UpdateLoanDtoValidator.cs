using FluentValidation;
using Library.Application.Dto.Loan;

namespace Library.Application.Validators
{
    public class UpdateLoanDtoValidator : AbstractValidator<UpdateLoanDto>
    {
        public UpdateLoanDtoValidator()
        {
            RuleFor(x => x.BookId)
                .NotEmpty()
                .WithMessage("BookId обязательно");

            RuleFor(x => x.DueDate)
                .NotEmpty()
                .WithMessage("Дата возврата обязательна")
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Дата возврата должна быть в будущем");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Некорректный статус");
        }
    }
}
