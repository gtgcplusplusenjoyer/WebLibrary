namespace Library.Application.Dto.Book
{
    public record UpdateBookDto(string Title, string Publisher, string? Description, int TotalCopies, List<Guid>? AuthorIds = null);
}
