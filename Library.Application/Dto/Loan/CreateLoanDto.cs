namespace Library.Application.Dto
{
    public record CreateLoanDto(Guid BookId, int LoanDays = 30);
}
