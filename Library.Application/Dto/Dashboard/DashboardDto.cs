namespace Library.Application.Dto.Dashboard
{
    public record DashboardDto( 
        int BooksCount,
        int AuthorsCount,
        int ActiveLoansCount,
        long ElapsedMilliseconds);
}
