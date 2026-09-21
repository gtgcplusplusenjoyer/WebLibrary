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
        private readonly ILibraryRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public BookService(ILibraryRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        public async Task<BookResponseDto> CreateBookAsync(CreateBookDto createBookDto, CancellationToken cancellationToken = default)
        {
            var book = _mapper.Map<Book>(createBookDto);

            book.Id = Guid.NewGuid();
            book.CreatedAt = DateTime.UtcNow;
            book.AvailableCopies = book.TotalCopies;

            if(createBookDto.AuthorIds != null && createBookDto.AuthorIds.Any())
            {
                var authors = await _authorRepository.GetByIds(createBookDto.AuthorIds, cancellationToken);

                foreach(var a in authors)
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

            var createdBook = await _bookRepository.GetBookById(book.Id, cancellationToken);

            return _mapper.Map<BookResponseDto>(createdBook);
        }

        public async Task DeleteBookAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetBookById(id, cancellationToken);

            if(book == null)
            {
                throw new NotFoundException($"Book with Id: {id} not found");
            }

            _bookRepository.DeleteBook(book);
            await _bookRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync(
            int pageNumber = 1,
            int pageSize = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            var books = await _bookRepository.GetAllBooks(pageNumber, pageSize, cancellationToken);

            return _mapper.Map<List<BookResponseDto>>(books);
        }

        public async Task<BookResponseDto> GetBookByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookById(id, cancellationToken);

            if (book == null)
            {
                throw new NotFoundException($"Book with Id: {id} not found");
            }

            return _mapper.Map<BookResponseDto>(book);
        }

        public async Task<BookResponseDto> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookById(id, cancellationToken);

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

            var updatedBook = await _bookRepository.GetBookById(book.Id, cancellationToken);

            return _mapper.Map<BookResponseDto>(updatedBook);
        }
    }
}
