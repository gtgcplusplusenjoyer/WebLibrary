namespace Library.Application.Dto
{
    public record AuthorResponseDto(
        Guid Id,
        DateTime CreatedAt,
        string FirstName,
        string LastName,
        string? Biography
    );
}
