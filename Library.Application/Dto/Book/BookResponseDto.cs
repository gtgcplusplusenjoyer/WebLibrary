using Library.Application.Dto.Author;

namespace Library.Application.Dto.Book
{
    public record BookResponseDto(Guid Id,
        DateTime CreatedAt,
        string Title,
        string Publisher,
        string? Description,
        int TotalCopies,
        int AvailableCopies,
        List<AuthorResponseDto> Authors);
}
