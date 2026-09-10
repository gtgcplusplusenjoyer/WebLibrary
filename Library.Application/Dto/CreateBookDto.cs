namespace Library.Application.Dto
{
    public record CreateBookDto(string Title, string Publisher, string? Description, int TotalCopies = 1);
}
