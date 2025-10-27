using DepotContainer.API.Controllers;
using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // 🧪 Lấy tất cả Booking
        [Fact(DisplayName = "Lấy tất cả Booking - Trả về danh sách")]
        public async Task GetAll_TraVeDanhSachBooking()
        {
            var bookings = new List<BookingDto> { new BookingDto { BookingId = 1, BookingNumber = "BK001" } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(bookings);

            var ketQua = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(ketQua);
            var duLieu = Assert.IsAssignableFrom<IEnumerable<BookingDto>>(ok.Value);
            Assert.Single(duLieu);
        }

        // 🧪 Lấy Booking theo ID
        [Fact(DisplayName = "Lấy Booking theo ID - Tồn tại")]
        public async Task GetById_TonTai_TraVeBooking()
        {
            var dto = new BookingDto { BookingId = 1, BookingNumber = "BK001" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

            var ketQua = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(ketQua);
            var duLieu = Assert.IsType<BookingDto>(ok.Value);
            Assert.Equal("BK001", duLieu.BookingNumber);
        }

        [Fact(DisplayName = "Lấy Booking theo ID - Không tồn tại")]
        public async Task GetById_KhongTonTai_TraVeNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((BookingDto)null);

            var ketQua = await _controller.GetById(99);

            Assert.IsType<NotFoundResult>(ketQua);
        }

        // 🧪 Lấy chi tiết Booking theo ID
        [Fact(DisplayName = "Lấy chi tiết Booking theo ID - Tồn tại")]
        public async Task GetBookingDetails_TonTai_TraVeOk()
        {
            var dto = new BookingDetailDto { BookingId = 1 };
            _mockService.Setup(s => s.GetBookingDetailsAsync(1)).ReturnsAsync(dto);

            var ketQua = await _controller.GetBookingDetails(1);

            var ok = Assert.IsType<OkObjectResult>(ketQua);
            Assert.IsType<BookingDetailDto>(ok.Value);
        }

        [Fact(DisplayName = "Lấy chi tiết Booking theo ID - Không tồn tại")]
        public async Task GetBookingDetails_KhongTonTai_TraVeNotFound()
        {
            _mockService.Setup(s => s.GetBookingDetailsAsync(1)).ReturnsAsync((BookingDetailDto)null);

            var ketQua = await _controller.GetBookingDetails(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(ketQua);
            Assert.Contains("không tồn tại", notFound.Value.ToString());
        }

        // 🧪 Lấy chi tiết theo BookingNumber
        [Fact]
        public async Task GetBookingDetailsByNumber_TonTai_TraVeOk()
        {
            var booking = new BookingDetailDto { BookingId = 1, BookingNumber = "BK001" };

            _mockService
                .Setup(s => s.GetBookingDetailsByNumberAsync("BK001"))
                .ReturnsAsync(booking);

            var result = await _controller.GetBookingDetailsByNumber("BK001");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<BookingDetailDto>(okResult.Value);
            Assert.Equal("BK001", data.BookingNumber);
        }

        [Fact(DisplayName = "Lấy chi tiết theo BookingNumber - Không tồn tại")]
        public async Task GetBookingDetailsByNumber_KhongTonTai_TraVeNotFound()
        {
            _mockService.Setup(s => s.GetBookingDetailsByNumberAsync("BK001")).ReturnsAsync((BookingDetailDto)null);

            var ketQua = await _controller.GetBookingDetailsByNumber("BK001");

            var notFound = Assert.IsType<NotFoundObjectResult>(ketQua);
            Assert.Contains("không tồn tại", notFound.Value.ToString());
        }

        // 🧪 Tạo mới Booking
        [Fact(DisplayName = "Tạo mới Booking - Thành công")]
        public async Task Create_ThanhCong_TraVeOk()
        {
            var dto = new CreateBookingDto { BookingNumber = "BK002" };
            var created = new BookingDto { BookingId = 1, BookingNumber = "BK002" };
            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var ketQua = await _controller.Create(dto);

            var ok = Assert.IsType<OkObjectResult>(ketQua);
            var duLieu = Assert.IsType<BookingDto>(ok.Value);
            Assert.Equal("BK002", duLieu.BookingNumber);
        }

        // 🧪 Cập nhật Booking
        [Fact(DisplayName = "Cập nhật Booking - Thành công")]
        public async Task Update_ThanhCong_TraVeOk()
        {
            var dto = new UpdateBookingDto { BookingId = 1 };
            _mockService.Setup(s => s.UpdateAsync(dto)).Returns(Task.CompletedTask);

            var ketQua = await _controller.Update(dto);

            var ok = Assert.IsType<OkObjectResult>(ketQua);
            Assert.Equal("Booking updated successfully", ok.Value);
        }

        // 🧪 Xóa Booking
        [Fact(DisplayName = "Xóa Booking - Thành công")]
        public async Task Delete_ThanhCong_TraVeOk()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var ketQua = await _controller.Delete(1);

            var ok = Assert.IsType<OkObjectResult>(ketQua);
            Assert.Equal("Booking deleted successfully", ok.Value);
        }
    }
}
