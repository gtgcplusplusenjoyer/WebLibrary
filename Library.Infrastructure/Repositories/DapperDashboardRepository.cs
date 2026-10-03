using Dapper;
using Library.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Library.Infrastructure.Repositories
{
    public class DapperDashboardRepository : IDashboardRepository
    {
        private readonly string _connectionString;
        public DapperDashboardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LibraryDbContext")!;
        }
        public async Task<int> GetActiveLoansCountAsync(CancellationToken cancellationToken = default)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            const string sql = @"
                SELECT 
                    COUNT(*)
                FROM ""Loans""
                WHERE ""Status"" = 1
            ";

            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> GetAuthorsCountAsync(CancellationToken cancellationToken = default)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            const string sql = @"
                SELECT 
                    COUNT(*)
                FROM ""Authors""
            ";

            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> GetBooksCountAsync(CancellationToken cancellationToken = default)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            const string sql = @"
                SELECT 
                    COUNT(*)
                FROM ""Books""    
            ";

            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
