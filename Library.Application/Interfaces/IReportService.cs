using Library.Domain.Dto.Reports;

namespace Library.Application.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<PopularBookDto>> GetPopularBooksAsync(int count = 10, CancellationToken cancellationToken = default);
        Task<LibraryStatisticsDto> GetStatisticsAsync(CancellationToken cancellationToken = default);
    }
}
