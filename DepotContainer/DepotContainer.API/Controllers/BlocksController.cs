using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepotContainer.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BlockController : ControllerBase
    {
        private readonly IBlockService _blockService;

        public BlockController(IBlockService blockService)
        {
            _blockService = blockService;
        }

        // ✅ Lấy tất cả block kèm số lượng slot trống / chiếm
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var blocks = await _blockService.GetAllAsync();

            var result = blocks.Select(b => new
            {
                blockId = b.BlockId,
                blockName = b.BlockName,
                availableSlots = b.Slots?.Count(s => s.Container == null) ?? 0, // slot trống
                occupiedSlots = b.Slots?.Count(s => s.Container != null) ?? 0, // slot có container
                totalSlots = b.Slots?.Count ?? 0
            });

            return Ok(result);
        }

        // ✅ Lấy chi tiết 1 block
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var block = await _blockService.GetByIdAsync(id);
            if (block == null)
                return NotFound();

            return Ok(block);
        }

        // ✅ Lấy danh sách slot theo blockId (hiển thị container nếu có)
        [HttpGet("{blockId}/slots")]
        public async Task<IActionResult> GetSlotsByBlockId(int blockId)
        {
            var block = await _blockService.GetBlockWithSlotsAsync(blockId);

            if (block == null)
                return NotFound($"Không tìm thấy block có ID {blockId}");

            var result = block.Slots.Select(slot => new
            {
                slotId = slot.SlotId,
                bay = slot.Bay,
                row = slot.Row,
                tier = slot.Tier,
                container = slot.Container == null ? null : new
                {
                    containerId = slot.Container.ContainerId,
                    containerNumber = slot.Container.ContainerNo,
                    contStatus = slot.Container.ContStatus,
                    contCondition = slot.Container.ContCondition,
                    weight = slot.Container.Weight,
                    timeIn = slot.Container.TimeIn
                }
            });

            return Ok(result);
        }

        // ✅ Tạo block mới
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlockDto dto)
        {
            var result = await _blockService.CreateAsync(dto);
            return Ok(result);
        }

        // ✅ Cập nhật block
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBlockDto dto)
        {
            var block = await _blockService.UpdateAsync(id, dto);
            return Ok(new
            {
                message = "Block updated successfully",
                block
            });
        }

        // ✅ Xóa block
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _blockService.DeleteAsync(id);
            return Ok("Block deleted successfully");
        }
    }
}
