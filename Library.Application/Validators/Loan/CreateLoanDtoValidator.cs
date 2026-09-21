using FluentValidation;
using Library.Application.Dto;
using Library.Application.Dto.Loan;

namespace Library.Application.Validators.Loan
{
    public class CreateLoanDtoValidator : AbstractValidator<CreateLoanDto>
    {
        public CreateLoanDtoValidator()
        {
            RuleFor(x => x.BookId)
                .NotEmpty()
                .WithMessage("BookId обязательно");

            RuleFor(x => x.LoanDays)
                .GreaterThan(0)
                .WithMessage("Срок выдачи должен быть больше 0")
                .LessThanOrEqualTo(365)
                .WithMessage("Срок выдачи не должен превышать 365 дней");
        }
    }
}
