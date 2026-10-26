namespace Library.Application.Dto.User
{
    public record AuthResponseDto(string Token, string Email, string FirstName, string LastName, string Role);
}
