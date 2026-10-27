using FluentValidation;
using Library.Application.Dto.User;

namespace Library.Application.Validators.User
{
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required")
                .MinimumLength(32).WithMessage("Refresh token must be at least 32 characters")
                .MaximumLength(512).WithMessage("Refresh token must not exceed 512 characters");
        }
    }
}
