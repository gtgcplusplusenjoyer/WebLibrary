namespace Library.Application.Dto
{
    public class AuthorResponseDto(
        Guid Id,
        DateTime CreatedAt,
        string FirstName,
        string LastName,
        string? Biography
    );
}
