using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DepotContainer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EirController : ControllerBase
    {
        private readonly IEirService _eirService;

        public EirController(IEirService eirService)
        {
            _eirService = eirService;
        }

        // ✅ GET: api/Eir
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var eirs = await _eirService.GetAllAsync();
            return Ok(eirs);
        }

        // ✅ GET: api/Eir/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var eir = await _eirService.GetByIdAsync(id);
            if (eir == null)
                return NotFound("EIR not found.");

            return Ok(eir);
        }

        // ✅ POST: api/Eir
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEirDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid EIR data.");

            var created = await _eirService.CreateAsync(dto);
            return Ok(created);
        }

        // ✅ PUT: api/Eir
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEirDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid update data.");

            await _eirService.UpdateAsync(dto);
            return Ok("EIR updated successfully.");
        }

        // ✅ DELETE: api/Eir/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _eirService.DeleteAsync(id);
            return Ok("EIR deleted successfully.");
        }
    }
}
