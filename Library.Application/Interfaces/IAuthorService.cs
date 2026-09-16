using Library.Application.Dto;

namespace Library.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorResponseDto> CreateAuthorAsync(CreateAuthorDto createAuthorDto, CancellationToken cancellationToken = default);
        Task<AuthorResponseDto> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<AuthorResponseDto>> GetAllAuthorsAsync(
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            CancellationToken cancellationToken = default);
        Task<AuthorResponseDto> UpdateAuthorAsync(Guid id, UpdateAuthorDto updateAuthorDto, CancellationToken cancellationToken = default);
        Task DeleteAuthorAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
