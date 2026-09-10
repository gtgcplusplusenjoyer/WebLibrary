namespace Library.Application.Dto
{
    public record BookResponseDto(Guid Id,
        DateTime CreatedAt,
        string Title,
        string Publisher,
        string? Description,
        int TotalCopies,
        int AvailableCopies);
}
