namespace Library.Application.Dto
{
    public record UpdateBookDto(string Title, string Publisher, string? Description, int TotalCopies = 1, int AvailableCopies = 1);
}
