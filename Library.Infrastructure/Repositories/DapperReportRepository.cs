using Dapper;
using Library.Domain.Dto.Reports;
using Library.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Library.Infrastructure.Repositories
{
    public class DapperReportRepository : IReportRepository
    {
        private readonly string _connectionString;
        public DapperReportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LibraryDbContext")!;
        }
        public async Task<IEnumerable<PopularBookDto>> GetPopularBooksAsync(int count, CancellationToken cancellationToken)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            const string sql = @"
            SELECT 
                B.""Id"" AS BookId,
                B.""Title"" AS Title,
                B.""Publisher"" AS Publisher,
                COUNT(L.""Id"") AS LoanCount
            FROM ""Books"" AS B
            LEFT JOIN ""Loans"" AS L
                ON(B.""Id"" = L.""BookId"")
            GROUP BY B.""Id"", B.""Title"", B.""Publisher""
            ORDER BY LoanCount DESC
            LIMIT @Count
            ";

            return await connection.QueryAsync<PopularBookDto>(sql, new { Count = count });
        }

        public async Task<LibraryStatisticsDto> GetStatisticsAsync(CancellationToken cancellationToken)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            const string sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM ""Books"") AS TotalBooks,
                    (SELECT COUNT(*) FROM ""Authors"") AS TotalAuthors,
                    (SELECT COUNT(*) FROM ""Loans"" WHERE ""Status"" = 1) AS ActiveLoans,
                    (SELECT COUNT(*) FROM ""Loans"" WHERE ""Status"" = 1 AND ""DueDate"" < NOW()) AS OverdueLoans               
            ";

            return await connection.QuerySingleAsync<LibraryStatisticsDto>(sql);
        }
    }
}
