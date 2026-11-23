using AutoMapper;
using Library.Application.Dto.Book;
using Library.Application.Exceptions;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Library.Application.Services
{
    public class BookService : IBookService
    {
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);
        private const string VersionKey = "books:version";
        private readonly ILibraryRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public BookService(ILibraryRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper, ICacheService cacheService)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<BookResponseDto> CreateBookAsync(CreateBookDto createBookDto, CancellationToken cancellationToken = default)
        {
            var book = _mapper.Map<Book>(createBookDto);

            book.Id = Guid.NewGuid();
            book.CreatedAt = DateTime.UtcNow;
            book.AvailableCopies = book.TotalCopies;

            if (createBookDto.AuthorIds != null && createBookDto.AuthorIds.Any())
            {
                var authors = await _authorRepository.GetByIds(createBookDto.AuthorIds, cancellationToken);

                foreach (var a in authors)
                {
                    book.BookAuthors.Add(new BookAuthor
                    {
                        AuthorId = a.Id,
                        BookId = book.Id
                    });
                }
            }

            await _bookRepository.Add(book, cancellationToken);
            await _bookRepository.SaveChangesAsync(cancellationToken);

            var createdBook = await _bookRepository.GetById(book.Id, cancellationToken);

            await InvalidateCacheAsync(createdBook!.Id, cancellationToken);

            return _mapper.Map<BookResponseDto>(createdBook);
        }

        public async Task DeleteBookAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetById(id, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException($"Book with Id: {id} not found");
            }

            _bookRepository.Delete(book);

            await _bookRepository.SaveChangesAsync(cancellationToken);
            await InvalidateCacheAsync(book.Id, cancellationToken);
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync(
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            var version = await _cacheService.GetAsync<int?>(VersionKey, cancellationToken) ?? 0;

            var key = $"books:v{version}:p{pageNumber}:ps{pageSize}";

            var cachedResult = await _cacheService.GetAsync<IEnumerable<BookResponseDto>>(key, cancellationToken);

            if(cachedResult != null)
            {
                return cachedResult;
            }

            var books = await _bookRepository.GetAll(pageNumber, pageSize, cancellationToken);

            var response = _mapper.Map<List<BookResponseDto>>(books);

            await _cacheService.SetAsync<List<BookResponseDto>>(key, response, CacheTtl, cancellationToken);

            return response;
        }

        public async Task<BookResponseDto> GetBookByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var key = $"book:{id}";

            var cachedBook = await _cacheService.GetAsync<BookResponseDto>(key, cancellationToken);

            if(cachedBook != null)
            {
                return cachedBook;
            }

            var book = await _bookRepository.GetById(id, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException($"Book with Id: {id} not found");
            }

            var response = _mapper.Map<BookResponseDto>(book);

            await _cacheService.SetAsync<BookResponseDto>(key, response, CacheTtl, cancellationToken);

            return response;
        }

        public async Task<BookResponseDto> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetById(id, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException($"Book with Id: {id} not found");
            }

            book.Title = updateBookDto.Title;
            book.Publisher = updateBookDto.Publisher;
            book.Description = updateBookDto.Description;
            book.TotalCopies = updateBookDto.TotalCopies;

            if (updateBookDto.AuthorIds != null)
            {
                book.BookAuthors.Clear();

                var authors = await _authorRepository.GetByIds(updateBookDto.AuthorIds, cancellationToken);

                foreach (var a in authors)
                {
                    book.BookAuthors.Add(new BookAuthor
                    {
                        AuthorId = a.Id,
                        BookId = book.Id
                    });
                }
            }

            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync(cancellationToken);

            var updatedBook = await _bookRepository.GetById(book.Id, cancellationToken);

            var response = _mapper.Map<BookResponseDto>(updatedBook); 

            await InvalidateCacheAsync(updatedBook!.Id, cancellationToken);

            return response;
        }
        private async Task InvalidateCacheAsync(Guid bookId, CancellationToken cancellationToken)
        {
            await _cacheService.RemoveAsync($"book:{bookId}", cancellationToken);

            var currentVersion = await _cacheService.GetAsync<int?>(VersionKey, cancellationToken) ?? 0;

            await _cacheService.SetAsync(VersionKey, currentVersion + 1, TimeSpan.FromHours(2), cancellationToken);
        }
    }
}
