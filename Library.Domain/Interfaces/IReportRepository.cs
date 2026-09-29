using Library.Domain.Dto.Reports;

namespace Library.Domain.Interfaces
{
    public interface IReportRepository
    {
        Task<LibraryStatisticsDto> GetStatisticsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<PopularBookDto>> GetPopularBooksAsync(int count = 10, CancellationToken cancellationToken = default);
    }
}
