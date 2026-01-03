using Microsoft.AspNetCore.Mvc;
using DepotContainer.Application.Interfaces.Services;

namespace DepotContainer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var statistics = await _statisticsService.GetDashboardStatisticsAsync();
            return Ok(statistics);
        }
    }
}