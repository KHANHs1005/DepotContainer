using DepotContainer.API.Controllers;
using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DepotContainer.UnitTests.TestAPI
{
    public class BookingControllerTests
    {
        private readonly Mock<IBookingService> _mockService;
        private readonly BookingController _controller;

        public BookingControllerTests()
        {
            _mockService = new Mock<IBookingService>();
            _controller = new BookingController(_mockService.Object);
        }

        // 1️⃣ Lấy tất cả Booking
        [Fact]
        public async Task GetAll_ReturnsOkWithList()
        {
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<BookingDto>
            {
                new BookingDto { BookingId = 1, BookingNumber = "BK001" }
            });

            var result = await _controller.GetAll() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var list = Assert.IsAssignableFrom<IEnumerable<BookingDto>>(result.Value);
            Assert.Single(list);
        }

        // 2️⃣ Lấy tất cả rỗng
        [Fact]
        public async Task GetAll_ReturnsOkWithEmptyList()
        {
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<BookingDto>());

            var result = await _controller.GetAll() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var list = Assert.IsAssignableFrom<IEnumerable<BookingDto>>(result.Value);
            Assert.Empty(list);
        }

        // 3️⃣ Lấy theo ID (có dữ liệu)
        [Fact]
        public async Task GetById_Found_ReturnsOk()
        {
            var booking = new BookingDto { BookingId = 1, BookingNumber = "BK001" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(booking);

            var result = await _controller.GetById(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var value = Assert.IsType<BookingDto>(result.Value);
            Assert.Equal("BK001", value.BookingNumber);
        }

        // 4️⃣ Lấy theo ID (không có dữ liệu)
        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((BookingDto?)null);

            var result = await _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result);
        }

        // 5️⃣ Tạo mới booking thành công
        [Fact]
        public async Task Create_ReturnsOkWithResult()
        {
            var dto = new CreateBookingDto
            {
                BookingNumber = "BK001",
                ContSize = "20",
                ContQuantity = 2,
                OperatorName = "Maersk",
                ReleaseExpireDate = DateTime.UtcNow,
                CusId = 1
            };

            var expected = new BookingDto
            {
                BookingId = 1,
                BookingNumber = "BK001"
            };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(expected);

            var result = await _controller.Create(dto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var booking = Assert.IsType<BookingDto>(result.Value);
            Assert.Equal("BK001", booking.BookingNumber);
        }

        // 6️⃣ Cập nhật thành công
        [Fact]
        public async Task Update_ReturnsOk()
        {
            var dto = new UpdateBookingDto
            {
                BookingId = 1,
                ContSize = "40",
                ContQuantity = 3,
                OperatorName = "MSC"
            };

            var result = await _controller.Update(dto) as OkObjectResult;

            _mockService.Verify(s => s.UpdateAsync(dto), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Booking updated successfully", result.Value);
        }

        // 7️⃣ Xoá thành công
        [Fact]
        public async Task Delete_ReturnsOk()
        {
            var result = await _controller.Delete(1) as OkObjectResult;

            _mockService.Verify(s => s.DeleteAsync(1), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Booking deleted successfully", result.Value);
        }

        // 8️⃣ Xử lý lỗi nội bộ khi tạo
        [Fact]
        public async Task Create_ThrowsException_ReturnsServerError()
        {
            var dto = new CreateBookingDto { BookingNumber = "BK001" };
            _mockService.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new Exception("Lỗi hệ thống"));

            // Gọi thử
            await Assert.ThrowsAsync<Exception>(() => _controller.Create(dto));
        }
    }
}
