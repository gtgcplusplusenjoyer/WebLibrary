using Library.Application.Dto;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("WebLibrary/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _service;
        public AuthorsController(IAuthorService service)
        {
            _service = service;
        }

        [HttpPost] 
        public async Task<IActionResult> CreateAuthor(
            [FromBody] CreateAuthorDto createAuthorDto,
            CancellationToken cancellationToken = default)
        {
            var author = await _service.CreateAuthorAsync(createAuthorDto, cancellationToken);

            return CreatedAtAction(nameof(GetAuthorById), new {id = author.Id}, author);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if(pageNumber < 0)
            {
                pageNumber = 1;
            }

            if(pageSize < 1)
            {
                pageSize = 1;
            }

            else if(pageSize > 50)
            {
                pageSize = 50;
            }

            var books = await _service.GetAllAuthorsAsync(pageNumber, pageSize, cancellationToken);

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var author = await _service.GetAuthorByIdAsync(id, cancellationToken);

            return Ok(author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor([FromRoute] Guid id,
            [FromBody] UpdateAuthorDto updateAuthorDto,
            CancellationToken cancellationToken = default)
        {
            var author = await _service.UpdateAuthorAsync(id, updateAuthorDto, cancellationToken);

            return Ok(author);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            await _service.DeleteAuthorAsync(id, cancellationToken);

            return NoContent();
        }
         

    }
}
