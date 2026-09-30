using Library.Application.Dto.Dashboard;

namespace Library.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardWithoutWhenAllAsync(CancellationToken cancellationToken = default);
        Task<DashboardDto> GetDashboardWithWhenAllAsync(CancellationToken cancellationToken = default);
    }
}
