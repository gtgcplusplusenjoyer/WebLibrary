namespace Library.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<int> GetBooksCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetAuthorsCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetLoansCountAsync(CancellationToken cancellationToken = default);
        
    }
}
