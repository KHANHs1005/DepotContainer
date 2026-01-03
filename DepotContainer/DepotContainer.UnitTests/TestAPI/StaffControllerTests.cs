using DepotContainer.API.Controllers;
using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DepotContainer.UnitTests.TestAPI
{
    public class StaffControllerTests
    {
        private readonly Mock<IStaffService> _staffServiceMock;
        private readonly StaffController _controller;

        public StaffControllerTests()
        {
            _staffServiceMock = new Mock<IStaffService>();
            _controller = new StaffController(_staffServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfStaff()
        {
            // Arrange
            var staffs = new List<StaffDto>
            {
                new StaffDto { StaffId = 1, StaffName = "Khanh", IsActive = true },
                new StaffDto { StaffId = 2, StaffName = "Nam", IsActive = false }
            };
            _staffServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(staffs);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<StaffDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsOk()
        {
            // Arrange
            var staff = new StaffDto { StaffId = 1, StaffName = "Khanh" };
            _staffServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(staff);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<StaffDto>(okResult.Value);
            Assert.Equal(1, returnValue.StaffId);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _staffServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((StaffDto?)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new CreateStaffDto
            {
                StaffName = "New Staff",
                Username = "newuser",
                Password = "123",
                StaffType = "Admin"
            };

            var createdStaff = new StaffDto
            {
                StaffId = 10,
                StaffName = "New Staff",
                Username = "newuser",
                IsActive = true
            };

            _staffServiceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdStaff);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
            var returnValue = Assert.IsType<StaffDto>(createdResult.Value);
            Assert.Equal(10, returnValue.StaffId);
        }

        [Fact]
        public async Task Update_ValidData_ReturnsNoContent()
        {
            // Arrange
            var updateDto = new UpdateStaffDto
            {
                StaffId = 1,
                StaffName = "Updated Name"
            };
            _staffServiceMock.Setup(s => s.UpdateAsync(updateDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _staffServiceMock.Verify(s => s.UpdateAsync(It.Is<UpdateStaffDto>(d => d.StaffId == 1)), Times.Once);
        }

        [Fact]
        public async Task Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange
            _staffServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _staffServiceMock.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}
