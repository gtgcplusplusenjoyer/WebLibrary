using Library.Domain.Enums;

namespace Library.Application.Dto
{
    public record UpdateLoanDto(Guid BookId, DateTime DueDate, LoanStatus Status);
}
