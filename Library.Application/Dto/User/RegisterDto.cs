namespace Library.Application.Dto.User
{
    public record RegisterDto(string FirstName, string LastName, string Email, string PasswordHash);
}
