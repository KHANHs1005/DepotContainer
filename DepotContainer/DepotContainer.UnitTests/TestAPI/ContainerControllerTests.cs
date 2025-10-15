
using Moq;
using Microsoft.AspNetCore.Mvc;
using DepotContainer.API.Controllers;
using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;

namespace DepotContainer.UnitTests.TestAPI
{
    public class ContainerControllerTests
    {
        private readonly Mock<IContainerService> _mockService;
        private readonly ContainerController _controller;

        public ContainerControllerTests()
        {
            _mockService = new Mock<IContainerService>();
            _controller = new ContainerController(_mockService.Object);
        }

        // -------------------- GET ALL --------------------

        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfContainers()
        {
            var list = new List<ContainerDto> {
                new ContainerDto { ContainerId = 1, ContainerNumber = "MSKU1234567" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var result = await _controller.GetAll() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var data = Assert.IsAssignableFrom<IEnumerable<ContainerDto>>(result.Value);
            Assert.Single(data);
        }

        // -------------------- GET BY ID --------------------

        [Fact]
        public async Task GetById_ValidId_ReturnsOk()
        {
            var container = new ContainerDto { ContainerId = 1, ContainerNumber = "MSKU7654321" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(container);

            var result = await _controller.GetById(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(container, result.Value);
        }

        [Fact]
        public async Task GetById_NotFound_Returns404()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((ContainerDto?)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        // -------------------- CREATE --------------------

        [Fact]
        public async Task Create_ValidData_ReturnsOk()
        {
            var dto = new CreateContainerDto { ContainerNumber = "TGHU1111111" };
            var created = new ContainerDto { ContainerId = 10, ContainerNumber = "TGHU1111111" };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await _controller.Create(dto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(created, result.Value);
        }

        [Fact]
        public async Task Create_ThrowsException_Returns500()
        {
            var dto = new CreateContainerDto { ContainerNumber = "ERR001" };
            _mockService.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new Exception("Lỗi khi tạo container"));

            // Giả lập try-catch đơn giản
            var ex = await Assert.ThrowsAsync<Exception>(() => _controller.Create(dto));
            Assert.Equal("Lỗi khi tạo container", ex.Message);
        }

        // -------------------- UPDATE --------------------

        [Fact]
        public async Task Update_Valid_ReturnsOk()
        {
            var dto = new UpdateContainerDto { ContainerId = 1, ContStatus = "Empty" };
            _mockService.Setup(s => s.UpdateAsync(dto)).Returns(Task.CompletedTask);

            var result = await _controller.Update(dto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Updated successfully", result.Value);
        }

        [Fact]
        public async Task Update_ThrowsException_ReturnsError()
        {
            var dto = new UpdateContainerDto { ContainerId = 1 };
            _mockService.Setup(s => s.UpdateAsync(dto)).ThrowsAsync(new Exception("Container không tồn tại"));

            var ex = await Assert.ThrowsAsync<Exception>(() => _controller.Update(dto));
            Assert.Equal("Container không tồn tại", ex.Message);
        }

        // -------------------- DELETE --------------------

        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Deleted successfully", result.Value);
        }

        [Fact]
        public async Task Delete_ThrowsException_ReturnsError()
        {
            _mockService.Setup(s => s.DeleteAsync(99)).ThrowsAsync(new Exception("Container không tồn tại"));

            var ex = await Assert.ThrowsAsync<Exception>(() => _controller.Delete(99));
            Assert.Equal("Container không tồn tại", ex.Message);
        }
    }
}
