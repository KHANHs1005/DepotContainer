using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Application.Services;
using DepotContainer.Domain.Entities;
using Moq;
using Xunit;

namespace ServiceTests
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _mockRepo;
        private readonly BookingService _service;

        public BookingServiceTests()
        {
            _mockRepo = new Mock<IBookingRepository>();
            _service = new BookingService(_mockRepo.Object);
        }

        // 1️⃣ Lấy tất cả
        [Fact]
        public async Task LayTatCa_TraVeDanhSachBooking()
        {
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Booking>
            {
                new Booking { BookingId = 1, BookingNumber = "BK20250926-001" }
            });

            var result = await _service.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("BK20250926-001", result.First().BookingNumber);
        }

        // 2️⃣ Lấy 1 booking theo ID
        [Fact]
        public async Task LayTheoId_TonTai_TraVeBooking()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Booking { BookingId = 1, BookingNumber = "BK20250926-001" });

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("BK20250926-001", result.BookingNumber);
        }

        // 3️⃣ Lấy 1 booking không tồn tại
        [Fact]
        public async Task LayTheoId_KhongTonTai_TraVeNull()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Booking)null);

            var result = await _service.GetByIdAsync(1);

            Assert.Null(result);
        }

        // 4️⃣ Tạo mới booking thành công
        [Fact]
        public async Task TaoMoi_ThanhCong()
        {
            var dto = new CreateBookingDto
            {
                BookingNumber = "BK20250926-001",
                ContSize = "20",
                ContQuantity = 2,
                OperatorName = "Maersk",
                ReleaseExpireDate = DateTime.UtcNow,
                CusId = 1
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Booking>());

            var result = await _service.CreateAsync(dto);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Once);
            Assert.Equal("BK20250926-001", result.BookingNumber);
        }

        // 5️⃣ Cập nhật booking thành công
        [Fact]
        public async Task CapNhat_ThanhCong()
        {
            var existing = new Booking
            {
                BookingId = 1,
                BookingNumber = "BK20250926-001",
                ContSize = "20",
                ContQuantity = 2,
                OperatorName = "Maersk",
                ReleaseExpireDate = DateTime.UtcNow
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var dto = new UpdateBookingDto
            {
                BookingId = 1,
                ContSize = "40",
                ContQuantity = 3,
                OperatorName = "MSC",
                ReleaseExpireDate = DateTime.UtcNow.AddDays(1)
            };

            await _service.UpdateAsync(dto);

            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Booking>()), Times.Once);
            Assert.Equal("MSC", existing.OperatorName);
        }

        // 6️⃣ Xóa booking thành công
        [Fact]
        public async Task Xoa_ThanhCong()
        {
            var existing = new Booking { BookingId = 1, BookingNumber = "BK20250926-001" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            await _service.DeleteAsync(1);

            _mockRepo.Verify(r => r.DeleteAsync(existing), Times.Once);
        }

        // 🔸 TEST MỚI

        [Fact]
        public async Task LayTatCa_KhongCoBooking_TraVeRong()
        {
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Booking>());

            var result = await _service.GetAllAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task TaoMoi_ThieuThongTin_ThatBai()
        {
            var dto = new CreateBookingDto { BookingNumber = "" };

            await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task TaoMoi_TrungMaBooking_ThatBai()
        {
            var dto = new CreateBookingDto { BookingNumber = "BK20250926-001" };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Booking>
            {
                new Booking { BookingNumber = "BK20250926-001" }
            });

            await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CapNhat_KhongTonTai_ThatBai()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Booking)null);

            var dto = new UpdateBookingDto { BookingId = 99 };

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(dto));
        }

        [Fact]
        public async Task Xoa_KhongTonTai_ThatBai()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Booking)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(99));
        }
    }
}
