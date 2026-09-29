using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("WebLibrary/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;
        public ReportsController(IReportService service)
        {
            _service = service;
        }
        [HttpGet("popular-books")]
        public async Task<IActionResult> GetPopularBooks([FromQuery] int count = 10, CancellationToken cancellationToken = default)
        {
            var popularBooks = await _service.GetPopularBooksAsync(count, cancellationToken);

            return Ok(popularBooks);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics(CancellationToken cancellationToken = default)
        {
            var statistics = await _service.GetStatisticsAsync(cancellationToken);

            return Ok(statistics);
        }
    }
}
