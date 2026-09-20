using Library.Domain.Enums;

namespace Library.Application.Dto
{
    public record LoanResponseDto(Guid Id,
        DateTime CreatedAt,
        Guid BookId,
        string BookTitle,
        DateTime LoanDate,
        DateTime DueDate,
        DateTime? ReturnDate,
        LoanStatus Status);
}
