using DepotContainer.API.Controllers;
using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;
using DepotContainer.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DepotContainer.UnitTests.TestAPI
{
    public class EirControllerTests
    {
        private readonly Mock<IEirService> _mockService;
        private readonly EirController _controller;

        public EirControllerTests()
        {
            _mockService = new Mock<IEirService>();
            _controller = new EirController(_mockService.Object);
        }

        //GET ALL - Trả về danh sách có dữ liệu
        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfEirs()
        {
            // Arrange
            var eirs = new List<EirDto>
            {
                new EirDto { EirId = 1, EirNumber = "EIR251001001", Type = EirType.GateIn }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(eirs);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var data = Assert.IsAssignableFrom<IEnumerable<EirDto>>(result.Value);
            Assert.Single(data);
        }

        //GET ALL - Trả về danh sách rỗng
        [Fact]
        public async Task GetAll_ReturnsOk_WithEmptyList()
        {
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<EirDto>());

            var result = await _controller.GetAll() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var data = Assert.IsAssignableFrom<IEnumerable<EirDto>>(result.Value);
            Assert.Empty(data);
        }

        //GET BY ID - Tìm thấy
        [Fact]
        public async Task GetById_Found_ReturnsOk()
        {
            var eir = new EirDto { EirId = 1, EirNumber = "EIR251001001", Type = EirType.GateOut };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(eir);

            var result = await _controller.GetById(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var returned = Assert.IsType<EirDto>(result.Value);
            Assert.Equal("EIR251001001", returned.EirNumber);
        }

        //GET BY ID - Không tìm thấy
        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((EirDto?)null);

            var result = await _controller.GetById(1) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("EIR not found.", result.Value);
        }

        //CREATE - Thành công
        [Fact]
        public async Task Create_ValidEir_ReturnsOk()
        {
            var dto = new CreateEirDto
            {
                EirNumber = "EIR251001002",
                Type = EirType.GateIn,
                ContainerId = 1,
                CustomerId = 1
            };

            var created = new EirDto { EirId = 1, EirNumber = dto.EirNumber, Type = dto.Type };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await _controller.Create(dto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var eir = Assert.IsType<EirDto>(result.Value);
            Assert.Equal("EIR251001002", eir.EirNumber);
        }

        //CREATE - Dữ liệu null
        [Fact]
        public async Task Create_NullDto_ReturnsBadRequest()
        {
            var result = await _controller.Create(null) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid EIR data.", result.Value);
        }

        //UPDATE - Thành công
        [Fact]
        public async Task Update_ValidDto_ReturnsOk()
        {
            var dto = new UpdateEirDto
            {
                EirId = 1,
                EirNumber = "EIR251001005",
                Type = EirType.GateOut
            };

            var result = await _controller.Update(dto) as OkObjectResult;

            _mockService.Verify(s => s.UpdateAsync(dto), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("EIR updated successfully.", result.Value);
        }

        //8️⃣ UPDATE - Null DTO
        [Fact]
        public async Task Update_NullDto_ReturnsBadRequest()
        {
            var result = await _controller.Update(null) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid update data.", result.Value);
        }

        //DELETE - Thành công
        [Fact]
        public async Task Delete_ExistingEir_ReturnsOk()
        {
            var result = await _controller.Delete(1) as OkObjectResult;

            _mockService.Verify(s => s.DeleteAsync(1), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("EIR deleted successfully.", result.Value);
        }

        // CREATE - Khi service ném lỗi
        [Fact]
        public async Task Create_ServiceThrowsException_ReturnsError()
        {
            var dto = new CreateEirDto { EirNumber = "EIR251001999", Type = EirType.GateIn, ContainerId = 1, CustomerId = 1 };

            _mockService.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new Exception("Service failed"));

            await Assert.ThrowsAsync<Exception>(() => _controller.Create(dto));
        }

        //UPDATE - Khi service ném lỗi
        [Fact]
        public async Task Update_ServiceThrowsException_ThrowsError()
        {
            var dto = new UpdateEirDto { EirId = 1 };
            _mockService.Setup(s => s.UpdateAsync(dto)).ThrowsAsync(new Exception("Update failed"));

            await Assert.ThrowsAsync<Exception>(() => _controller.Update(dto));
        }

        //DELETE - Khi service ném lỗi
        [Fact]
        public async Task Delete_ServiceThrowsException_ThrowsError()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ThrowsAsync(new Exception("Delete failed"));

            await Assert.ThrowsAsync<Exception>(() => _controller.Delete(1));
        }
    }
}
