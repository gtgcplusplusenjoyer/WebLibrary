using Library.Application.Dto;
using Library.Application.Dto.Loan;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("WebLibrary/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _service;
        public LoansController(ILoanService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanDto createLoanDto,
            CancellationToken cancellationToken = default)
        {
            var loan = await _service.CreateLoanAsync(createLoanDto, cancellationToken);

            return CreatedAtAction(nameof(GetLoanById), new { id = loan.Id }, loan);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanById([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var loan = await _service.GetLoanByIdAsync(id, cancellationToken);

            return Ok(loan);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLoans(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 1;
            }
            else if (pageSize > 50)
            {
                pageSize = 50;
            }

            var loans = await _service.GetAllLoansAsync(pageNumber, pageSize, cancellationToken);

            return Ok(loans);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveLoans(CancellationToken cancellationToken = default)
        {
            var loans = await _service.GetActiveLoansAsync(cancellationToken);

            return Ok(loans);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueLoans(CancellationToken cancellationToken = default)
        {
            var loans = await _service.GetOverdueLoansAsync(cancellationToken);

            return Ok(loans);
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetLoanByBookId([FromRoute] Guid bookId, CancellationToken cancellationToken = default)
        {
            var loans = await _service.GetLoansByBookIdAsync(bookId, cancellationToken);

            return Ok(loans);
        }

        [HttpPatch("{id}/return")]
        public async Task<IActionResult> ReturnLoan([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var loan = await _service.ReturnLoanAsync(id, cancellationToken);

            return Ok(loan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(
            [FromRoute] Guid id,
            [FromBody] UpdateLoanDto updateLoanDto,
            CancellationToken cancellationToken = default)
        {
            var loan = await _service.UpdateLoanAsync(id, updateLoanDto, cancellationToken);

            return Ok(loan);
        }
    }
}
