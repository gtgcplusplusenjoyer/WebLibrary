namespace Library.Domain.Dto.Reports
{
    public record PopularBookDto(Guid BookId, string Title, string Publisher, int LoanCount);
}
