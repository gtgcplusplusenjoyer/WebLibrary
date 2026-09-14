using Library.Application.Dto;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("WebLibrary/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;
        public BooksController(IBookService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto createBookDto, CancellationToken cancellationToken)
        {
            var book = await _service.CreateBookAsync(createBookDto, cancellationToken);

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(Guid id,CancellationToken cancellationToken)
        {
            var book = await _service.GetBookByIdAsync(id, cancellationToken);

            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks(CancellationToken cancellationToken,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if(pageNumber < 1)
            {
                pageNumber = 1;
            }

            if(pageSize < 5)
            {
                pageSize = 5;
            }
            else if(pageSize> 50)
            {
                pageSize = 50;
            }
            

            var books = await _service.GetAllBooksAsync(cancellationToken, pageNumber, pageSize);

            return Ok(books);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id,
            [FromBody] UpdateBookDto updateBookDto,
            CancellationToken cancellationToken)
        {
            var book = await _service.UpdateBookAsync(id, updateBookDto, cancellationToken);

            return Ok(book);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id, CancellationToken cancellationToken)
        {
            await _service.DeleteBookAsync(id, cancellationToken);

            return NoContent();
        }

    }
}
