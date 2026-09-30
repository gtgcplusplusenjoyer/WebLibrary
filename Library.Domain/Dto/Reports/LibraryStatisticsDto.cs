namespace Library.Domain.Dto.Reports
{
    public record LibraryStatisticsDto(long TotalBooks, long TotalAuthors, long ActiveLoans, long OverdueLoans);
}
