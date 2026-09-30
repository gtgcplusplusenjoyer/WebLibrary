namespace Library.Application.Dto.Author
{
    public record AuthorResponseDto(
        Guid Id,
        DateTime CreatedAt,
        string FirstName,
        string LastName,
        string? Biography
    );
}
