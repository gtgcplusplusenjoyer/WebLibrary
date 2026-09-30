using Library.Application.Dto.Dashboard;
using Library.Application.Interfaces;
using System.Diagnostics;

namespace Library.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly ILoanService _loanService;
        public DashboardService(IBookService bookService, IAuthorService authorService, ILoanService loanService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _loanService = loanService;
        }
        public async Task<DashboardDto> GetDashboardWithoutWhenAllAsync(CancellationToken cancellationToken = default) 
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var books = await _bookService.GetAllBooksAsync(cancellationToken: cancellationToken);
            var authors = await _authorService.GetAllAuthorsAsync(cancellationToken: cancellationToken);
            var loans = await _loanService.GetAllLoansAsync(cancellationToken: cancellationToken);

            stopwatch.Stop();

            return new DashboardDto(books, authors, loans, stopwatch.ElapsedMilliseconds);
        }

        public async Task<DashboardDto> GetDashboardWithWhenAllAsync(CancellationToken cancellationToken = default)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var booksTask = _bookService.GetAllBooksAsync(cancellationToken: cancellationToken);
            var authorsTask = _authorService.GetAllAuthorsAsync(cancellationToken: cancellationToken);
            var loansTask = _loanService.GetAllLoansAsync(cancellationToken: cancellationToken);

            await Task.WhenAll(booksTask, authorsTask, loansTask);

            var books = await booksTask;
            var authors = await authorsTask;
            var loans = await loansTask;

            stopwatch.Stop();

            return new DashboardDto(books, authors, loans, stopwatch.ElapsedMilliseconds);
        }
    }
}
