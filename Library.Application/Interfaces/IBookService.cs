using Library.Application.Dto;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<BookResponseDto> CreateBookAsync(CreateBookDto createBookDto, CancellationToken cancellationToken);
        Task<BookResponseDto?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<BookResponseDto>> GetAllBooksAsync(CancellationToken cancellationToken);
        Task<BookResponseDto> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto, CancellationToken cancellationToken);
        Task DeleteBookAsync(Guid id,CancellationToken cancellationToken);
    }
}
