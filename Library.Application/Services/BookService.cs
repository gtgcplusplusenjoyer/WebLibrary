using AutoMapper;
using Library.Application.Dto;
using Library.Application.Exceptions;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;

namespace Library.Application.Services
{
    public class BookService : IBookService
    {
        private readonly ILibraryRepository _repository;
        private readonly IMapper _mapper;
        public BookService(ILibraryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<BookResponseDto> CreateBookAsync(CreateBookDto createBookDto, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(createBookDto);

            book.Id = Guid.NewGuid();
            book.CreatedAt = DateTime.UtcNow;
            book.AvailableCopies = book.TotalCopies;

            await _repository.Add(book, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<BookResponseDto>(book);
        }

        public async Task DeleteBookAsync(Guid id, CancellationToken cancellationToken)
        {
            var book = await _repository.GetBookById(id, cancellationToken);

            if(book == null)
            {
                throw new NotFoundException($"Book with Id: {id} not found");
            }

            _repository.DeleteBook(book);
            await _repository.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync(CancellationToken cancellationToken)
        {
            var books = await _repository.GetAllBooks(cancellationToken);

            return _mapper.Map<List<BookResponseDto>>(books);
        }

        public async Task<BookResponseDto?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var book = await _repository.GetBookById(id, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException($"Book with Id: {id} not found");
            }

            return _mapper.Map<BookResponseDto>(book);
        }

        public async Task<BookResponseDto> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto, CancellationToken cancellationToken)
        {
            var book = await _repository.GetBookById(id, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException($"Book with Id: {id} not found");
            }

            book.Title = updateBookDto.Title;
            book.Publisher = updateBookDto.Publisher;
            book.Description = updateBookDto.Description;
            book.TotalCopies = updateBookDto.TotalCopies;

            _repository.Update(book);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<BookResponseDto>(book);
        }
    }
}
