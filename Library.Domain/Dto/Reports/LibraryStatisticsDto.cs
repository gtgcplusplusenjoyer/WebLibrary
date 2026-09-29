namespace Library.Domain.Dto.Reports
{
    public record LibraryStatisticsDto(int TotalBooks, int TotalAuthors, int ActiveLoans, int OverdueLoans);
}
