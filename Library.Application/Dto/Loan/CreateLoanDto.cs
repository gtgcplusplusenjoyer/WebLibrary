namespace Library.Application.Dto
{
    public record CreateLoanDto(Guid BookId, Guid UserId, int LoanDays = 30);
}
