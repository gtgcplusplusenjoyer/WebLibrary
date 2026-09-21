using Library.Domain.Enums;

namespace Library.Application.Dto.Loan
{
    public record UpdateLoanDto(Guid BookId, DateTime DueDate, LoanStatus Status);
}
