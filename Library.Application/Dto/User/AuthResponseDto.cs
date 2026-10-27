namespace Library.Application.Dto.User
{
    public record AuthResponseDto(
        string AccessToken,
        string RefreshToken,
        Guid UserId);
}
