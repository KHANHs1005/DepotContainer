using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Application.Services;
using DepotContainer.Domain.Entities;
using DepotContainer.Domain.Enums;
using Moq;
using Xunit;
using System.Text.RegularExpressions;

namespace DepotContainer.UnitTests.TestService
{
    public class EirServiceTests
    {
        private readonly Mock<IEirRepository> _mockRepo;
        private readonly EirService _service;

        public EirServiceTests()
        {
            _mockRepo = new Mock<IEirRepository>();
            _service = new EirService(_mockRepo.Object);
        }

        // 🟢 Test lấy tất cả
        [Fact(DisplayName = "Lấy toàn bộ EIR - Trả về danh sách hợp lệ")]
        public async Task LayTatCaEIR_TraVeDanhSachHopLe()
        {
            var eirList = new List<EIR>
            {
                new EIR
                {
                    EirId = 1,
                    EirNumber = "EIR251014001",
                    Type = EirType.GateIn,
                    Customer = new Customer { Name = "Công Ty Vận Tải" },
                    Staff = new Staff { StaffName = "Đàm Duy Khánh" },
                    Container = new Container { ContainerNo = "EGHU1029384" },
                    PlateNumber = "51D-67890",
                    BatNo = 3,
                    IssueDate = DateTime.Now,
                    RegisAt = DateTime.Now
                }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(eirList);

            var result = await _service.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("EIR251014001", result.First().EirNumber);
        }

        // 🟢 Test lấy theo ID
        [Fact(DisplayName = "Lấy EIR theo ID - Tồn tại")]
        public async Task LayEIRTheoId_TonTai()
        {
            var eir = new EIR
            {
                EirId = 1,
                EirNumber = "EIR251014001",
                Type = EirType.GateIn,
                Customer = new Customer { Name = "Công Ty Vận Tải" },
                Staff = new Staff { StaffName = "Đàm Duy Khánh" },
                Container = new Container { ContainerNo = "EGHU1029384" },
                PlateNumber = "51D-67890"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(eir);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("EIR251014001", result.EirNumber);
        }

        [Fact(DisplayName = "Lấy EIR theo ID - Không tồn tại")]
        public async Task LayEIRTheoId_KhongTonTai()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EIR)null);
            var result = await _service.GetByIdAsync(99);
            Assert.Null(result);
        }

        // 🟢 Test tạo mới
        [Fact(DisplayName = "Tạo mới EIR - Sinh mã tự động nếu trống")]
        public async Task TaoMoiEIR_SinhMaTuDongNeuTrong()
        {
            var dto = new CreateEirDto { EirNumber = "", Type = EirType.GateIn };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<EIR>())).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            // Chấp nhận EIR251014001 hoặc EIR-20251014xxxx
            Assert.Matches(new Regex(@"^EIR-?\d{6,20}$"), result.EirNumber);
        }

        [Fact(DisplayName = "Tạo mới EIR - Dùng mã có sẵn nếu được cung cấp")]
        public async Task TaoMoiEIR_DungMaCoSanNeuCo()
        {
            var dto = new CreateEirDto { EirNumber = "EIR251014001", Type = EirType.GateIn };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<EIR>())).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            Assert.Equal("EIR251014001", result.EirNumber);
        }

        // 🟢 Test cập nhật
        [Fact(DisplayName = "Cập nhật EIR - Thành công")]
        public async Task CapNhatEIR_ThanhCong()
        {
            var eir = new EIR { EirId = 1, EirNumber = "EIR251014001" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(eir);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<EIR>())).Returns(Task.CompletedTask);

            var dto = new UpdateEirDto { EirId = 1, PlateNumber = "51D-67890" };
            await _service.UpdateAsync(dto);

            _mockRepo.Verify(r => r.UpdateAsync(It.Is<EIR>(e => e.PlateNumber == "51D-67890")), Times.Once);
        }

        [Fact(DisplayName = "Cập nhật EIR - Không tồn tại")]
        public async Task CapNhatEIR_KhongTonTai()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((EIR)null);
            var dto = new UpdateEirDto { EirId = 1 };

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(dto));
            Assert.Contains("EIR not found", ex.Message);
        }

        // 🟢 Test xóa
        [Fact(DisplayName = "Xóa EIR - Thành công")]
        public async Task XoaEIR_ThanhCong()
        {
            var eir = new EIR { EirId = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(eir);
            _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<EIR>())).Returns(Task.CompletedTask);

            await _service.DeleteAsync(1);

            _mockRepo.Verify(r => r.DeleteAsync(eir), Times.Once);
        }

        [Fact(DisplayName = "Xóa EIR - Không tồn tại")]
        public async Task XoaEIR_KhongTonTai()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EIR)null);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(99));
            Assert.Contains("EIR not found", ex.Message);
        }
    }
}
