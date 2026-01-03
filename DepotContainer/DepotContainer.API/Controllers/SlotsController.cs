using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DepotContainer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlotsController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotsController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSlotDto dto)
        {
            var result = await _slotService.CreateSlotAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _slotService.GetAllAsync();
            return Ok(result);
        }
    }
}
