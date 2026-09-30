using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("WebLibrary/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("dashboard-without-whenall")]
        public async Task<IActionResult> GetDashboardWithoutWhenAllAsync(CancellationToken cancellationToken = default)
        {
            var dashboard = await _service.GetDashboardWithoutWhenAllAsync(cancellationToken);

            return Ok(dashboard);
        }
        [HttpGet("dashboard-with-whenall")]
        public async Task<IActionResult> GetDashboardWithWhenAllAsync(CancellationToken cancellationToken = default)
        {
            var dashboard = await _service.GetDashboardWithWhenAllAsync(cancellationToken);

            return Ok(dashboard);
        }
    }
}
