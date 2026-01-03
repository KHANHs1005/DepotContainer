using DepotContainer.API.Controllers;
using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;
using DepotContainer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DepotContainer.UnitTests.TestAPI
{
    public class BlockControllerTests
    {
        private readonly Mock<IBlockService> _mockService;
        private readonly BlockController _controller;

        public BlockControllerTests()
        {
            _mockService = new Mock<IBlockService>();
            _controller = new BlockController(_mockService.Object);
        }

        // ✅ GetAll
        [Fact]
        public async Task GetAll_ReturnsOk_WithBlockList()
        {
            var blocks = new List<Block>
            {
                new Block
                {
                    BlockId = 1,
                    BlockName = "A",
                    Slots = new List<Slot>
                    {
                        new Slot { SlotId = 1, Container = null },
                        new Slot { SlotId = 2, Container = new Container() }
                    }
                }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(blocks);

            var result = await _controller.GetAll() as OkObjectResult;

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result.Value);
        }

        // ✅ GetById found
        [Fact]
        public async Task GetById_ReturnsOk_WhenBlockExists()
        {
            var block = new Block { BlockId = 1, BlockName = "Block A" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(block);

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(block, ok.Value);
        }

        // ❌ GetById not found
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenBlockDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Block?)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        // ✅ GetSlotsByBlockId found
        [Fact]
        public async Task GetSlotsByBlockId_ReturnsOk_WithSlots()
        {
            var block = new Block
            {
                BlockId = 1,
                Slots = new List<Slot>
        {
            new Slot
            {
                SlotId = 1,
                Bay = 1,
                Row = 1,
                Tier = 1,
                Container = new Container
                {
                    ContainerId = 10,
                    ContainerNo = "CONT001",
                    ContStatus = "Full",
                    ContCondition = "Good",
                    Weight = 2500,
                    TimeIn = DateTime.Now
                }
            }
        }
            };

            _mockService.Setup(s => s.GetBlockWithSlotsAsync(1)).ReturnsAsync(block);

            var result = await _controller.GetSlotsByBlockId(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }


        // ❌ GetSlotsByBlockId not found
        [Fact]
        public async Task GetSlotsByBlockId_ReturnsNotFound_WhenBlockDoesNotExist()
        {
            _mockService.Setup(s => s.GetBlockWithSlotsAsync(999)).ReturnsAsync((Block?)null);

            var result = await _controller.GetSlotsByBlockId(999);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("Không tìm thấy", notFound.Value?.ToString());
        }

        // ✅ Create
        [Fact]
        public async Task Create_ReturnsOk_WithCreatedBlock()
        {
            var dto = new CreateBlockDto { BlockName = "NewBlock" };
            var createdBlock = new Block { BlockId = 1, BlockName = "NewBlock" };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(createdBlock);

            var result = await _controller.Create(dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(createdBlock, ok.Value);
        }

        // ✅ Update
        [Fact]
        public async Task Update_ReturnsOk_WithUpdatedBlock()
        {
            // Arrange
            var dto = new UpdateBlockDto { BlockName = "Updated" };
            var updated = new Block { BlockId = 1, BlockName = "Updated" };

            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(updated);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value!;

            // 🧩 Dùng Reflection để đọc property từ anonymous object
            var message = value.GetType().GetProperty("message")?.GetValue(value, null);
            var block = value.GetType().GetProperty("block")?.GetValue(value, null);

            Assert.Equal("Block updated successfully", message);
            Assert.Equal(updated, block);
        }

        // ✅ Delete
        [Fact]
        public async Task Delete_ReturnsOk_WhenDeletedSuccessfully()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Block deleted successfully", ok.Value);
        }
    }
}
