namespace Library.Application.Dto.Book
{
    public record CreateBookDto(string Title, string Publisher, string? Description, int TotalCopies = 1, List<Guid>? AuthorIds = null);
}
