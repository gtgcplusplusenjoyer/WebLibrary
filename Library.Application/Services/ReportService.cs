using Library.Application.Interfaces;
using Library.Domain.Dto.Reports;
using Library.Domain.Interfaces;

namespace Library.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<IEnumerable<PopularBookDto>> GetPopularBooksAsync(int count = 10, CancellationToken cancellationToken = default)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Count must be greater than 0");
            }

            return await _reportRepository.GetPopularBooksAsync(count, cancellationToken);
        }

        public async Task<LibraryStatisticsDto> GetStatisticsAsync(CancellationToken cancellationToken = default)
        {
            return await _reportRepository.GetStatisticsAsync(cancellationToken);
        }
    }
}
