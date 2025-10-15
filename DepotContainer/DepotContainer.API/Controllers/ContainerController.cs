using Microsoft.AspNetCore.Mvc;
using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;

namespace DepotContainer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerService _containerService;

        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var containers = await _containerService.GetAllAsync();
            return Ok(containers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var container = await _containerService.GetByIdAsync(id);
            if (container == null) return NotFound();
            return Ok(container);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContainerDto dto)
        {
            var created = await _containerService.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateContainerDto dto)
        {
            await _containerService.UpdateAsync(dto);
            return Ok("Updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _containerService.DeleteAsync(id);
            return Ok("Deleted successfully");
        }
    }
}
