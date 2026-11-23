namespace Library.Domain.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<int> GetBooksCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetAuthorsCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetActiveLoansCountAsync(CancellationToken cancellationToken = default);

    }
}
