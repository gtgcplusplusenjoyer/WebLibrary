using Library.Application.Dto.Author;
using Library.Application.Dto.Book;
using Library.Application.Dto.Loan;

namespace Library.Application.Dto.Dashboard
{
    public record DashboardDto(IEnumerable<BookResponseDto> Books,
        IEnumerable<AuthorResponseDto> Authors,
        IEnumerable<LoanResponseDto> Loans,
        long ElapsedMilliseconds,
        int BooksCount,
        int AuthorsCount,
        int ActiveLoansCount);
}
