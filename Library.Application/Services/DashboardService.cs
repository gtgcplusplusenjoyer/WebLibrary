using Library.Application.Dto.Dashboard;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using System.Diagnostics;

namespace Library.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<DashboardDto> GetDashboardWithoutWhenAllAsync(CancellationToken cancellationToken = default)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var books = await _dashboardRepository.GetBooksCountAsync(cancellationToken);
            var authors = await _dashboardRepository.GetAuthorsCountAsync(cancellationToken);
            var loans = await _dashboardRepository.GetActiveLoansCountAsync(cancellationToken);

            stopwatch.Stop();

            return new DashboardDto(books, authors, loans, stopwatch.ElapsedMilliseconds);
        }

        public async Task<DashboardDto> GetDashboardWithWhenAllAsync(CancellationToken cancellationToken = default)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var booksTask = _dashboardRepository.GetBooksCountAsync(cancellationToken);
            var authorsTask = _dashboardRepository.GetAuthorsCountAsync(cancellationToken);
            var loansTask = _dashboardRepository.GetActiveLoansCountAsync(cancellationToken);

            await Task.WhenAll(booksTask, authorsTask, loansTask);

            var books = await booksTask;
            var authors = await authorsTask;
            var loans = await loansTask;

            stopwatch.Stop();

            return new DashboardDto(books, authors, loans, stopwatch.ElapsedMilliseconds);
        }
    }
}
