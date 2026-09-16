using AutoMapper;
using Library.Application.Dto;
using Library.Application.Exceptions;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;

namespace Library.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;
        private readonly IMapper _mapper;
        public AuthorService(IAuthorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AuthorResponseDto> CreateAuthorAsync(CreateAuthorDto createAuthorDto, CancellationToken cancellationToken = default)
        {
            var author = _mapper.Map<Author>(createAuthorDto);

            author.Id = Guid.NewGuid();
            author.CreatedAt = DateTime.UtcNow;

            await _repository.Add(author, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AuthorResponseDto>(author);
        }

        public async Task DeleteAuthorAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var author = await _repository.GetAuthorById(id,cancellationToken);

            if(author == null)
            {
                throw new NotFoundException($"Author with Id: {id} not found");
            }

            _repository.DeleteAuthor(author);

            await _repository.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<AuthorResponseDto>> GetAllAuthorsAsync(
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var authors = await _repository.GetAll(pageNumber, pageSize, cancellationToken);

            return _mapper.Map<List<AuthorResponseDto>>(authors);
        }

        public async Task<AuthorResponseDto> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var author = await _repository.GetAuthorById(id, cancellationToken);

            if(author == null)
            {
                throw new NotFoundException($"Author with Id: {id} not found");
            }

            return _mapper.Map<AuthorResponseDto>(author);
        }

        public async Task<AuthorResponseDto> UpdateAuthorAsync(Guid id,
            UpdateAuthorDto updateAuthorDto,
            CancellationToken cancellationToken = default)
        {
            var author = await _repository.GetAuthorById(id, cancellationToken);

            if (author == null)
            {
                throw new NotFoundException($"Author with Id: {id} not found");
            }

            author.FirstName = updateAuthorDto.FirstName;
            author.LastName = updateAuthorDto.LastName;
            author.Biography = updateAuthorDto.Biography;

            _repository.Update(author);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AuthorResponseDto>(author);
        }
    }
}
