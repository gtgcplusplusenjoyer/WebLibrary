using Library.Application.Dto.Book;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<BookResponseDto> CreateBookAsync(CreateBookDto createBookDto, CancellationToken cancellationToken = default);
        Task<BookResponseDto> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookResponseDto>> GetAllBooksAsync(
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            CancellationToken cancellationToken = default);
        Task<BookResponseDto> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto, CancellationToken cancellationToken = default);
        Task DeleteBookAsync(Guid id,CancellationToken cancellationToken = default);
    }
}
