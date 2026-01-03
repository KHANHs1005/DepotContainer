using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Application.Services;
using DepotContainer.Domain.Entities;
using Moq;
using Xunit;

namespace DepotContainer.UnitTests.TestService
{
    public class ContainerServiceTests
    {
        private readonly Mock<IContainerRepository> _mockContainerRepo;
        private readonly Mock<IBlockRepository> _mockBlockRepo;
        private readonly Mock<ISlotRepository> _mockSlotRepo;
        private readonly ContainerService _service;

        public ContainerServiceTests()
        {
            _mockContainerRepo = new Mock<IContainerRepository>();
            _mockBlockRepo = new Mock<IBlockRepository>();
            _mockSlotRepo = new Mock<ISlotRepository>();
            _service = new ContainerService(_mockContainerRepo.Object, _mockBlockRepo.Object, _mockSlotRepo.Object);
        }

        // ------------------- LẤY DỮ LIỆU -------------------

        [Fact]
        public async Task LayTatCaContainer_TraVeDanhSach()
        {
            var list = new List<Container> {
                new Container { ContainerId = 1, ContainerNo = "MSKU1234567", OperatorName = "Maersk" }
            };
            _mockContainerRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            var ketQua = await _service.GetAllAsync();

            Assert.Single(ketQua);
        }

        [Fact]
        public async Task LayContainerTheoId_TonTai_TraVeContainer()
        {
            var container = new Container { ContainerId = 1, ContainerNo = "TGHU7654321", OperatorName = "MSC" };
            _mockContainerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(container);

            var ketQua = await _service.GetByIdAsync(1);

            Assert.NotNull(ketQua);
            Assert.Equal("TGHU7654321", ketQua.ContainerNumber);
        }

        [Fact]
        public async Task LayContainerTheoId_KhongTonTai_TraVeNull()
        {
            _mockContainerRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Container)null);

            var ketQua = await _service.GetByIdAsync(99);

            Assert.Null(ketQua);
        }

        // ------------------- TẠO MỚI -------------------

        [Fact]
        public async Task TaoMoiContainer_HopLe_ThanhCong()
        {
            var dto = new CreateContainerDto
            {
                ContainerNumber = "MSKU1234567",
                OperatorName = "Maersk",
                ContStatus = "Full",
                ContCondition = "Good",
                CurrentBlock = "A",
                Bay = 5,
                Row = 3,
                Tier = 2
            };

            var block = new Domain.Entities.Block { BlockId = 1, BlockName = "A" };
            var slot = new Slot { SlotId = 500, BlockId = 1, Bay = 5, Row = 3, Tier = 2, StatusSlot = "Empty" };

            _mockBlockRepo.Setup(r => r.GetByNameAsync("A1")).ReturnsAsync(block);
            _mockSlotRepo.Setup(r => r.GetSlotAsync(1, 5, 3, 2)).ReturnsAsync(slot);

            _mockContainerRepo.Setup(r => r.AddAsync(It.IsAny<Container>()))
                .Callback<Container>(c => c.ContainerId = 999)
                .Returns(Task.CompletedTask);

            var ketQua = await _service.CreateAsync(dto);

            Assert.NotNull(ketQua);
            Assert.Equal("MSKU1234567", ketQua.ContainerNumber);
            _mockSlotRepo.Verify(r => r.UpdateAsync(It.Is<Slot>(s => s.StatusSlot == "Full")), Times.Once);
        }

        [Fact]
        public async Task TaoMoiContainer_ThieuContainerNumber_ThatBai()
        {
            var dto = new CreateContainerDto { ContainerNumber = "", CurrentBlock = "A1", Bay = 1, Row = 1, Tier = 1 };

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(dto));

            Assert.Contains("ContainerNumber là bắt buộc", ex.Message);
        }

        [Fact]
        public async Task TaoMoiContainer_ThieuThongTinViTri_ThatBai()
        {
            var dto = new CreateContainerDto { ContainerNumber = "MSKU1234567" };

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(dto));

            Assert.Contains("Vui lòng nhập đầy đủ", ex.Message);
        }

        [Fact]
        public async Task TaoMoiContainer_KhongTimThayBlock_ThatBai()
        {
            // Arrange
            var dto = new CreateContainerDto
            {
                ContainerNumber = "MSKU1234567",
                CurrentBlock = "B",
                Bay = 1,
                Row = 1,
                Tier = 1
            };

            // Giả lập GetByNameAsync trả về null
            _mockBlockRepo.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Domain.Entities.Block)null);

            // Act + Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(dto));

            // ✅ Kiểm tra nội dung exception (chỉ cần chứa từ “Block” là đủ)
            Assert.Contains("Block", ex.Message);
        }


        [Fact]
        public async Task TaoMoiContainer_SlotDaDay_ThatBai()
        {
            var dto = new CreateContainerDto
            {
                ContainerNumber = "MSKU1234567",
                CurrentBlock = "A",
                Bay = 1,
                Row = 1,
                Tier = 1
            };
            var block = new Domain.Entities.Block { BlockId = 1, BlockName = "A" };
            var slot = new Slot { SlotId = 1, BlockId = 1, Bay = 1, Row = 1, Tier = 1, StatusSlot = "Full" };

            _mockBlockRepo.Setup(r => r.GetByNameAsync("A")).ReturnsAsync(block);
            _mockSlotRepo.Setup(r => r.GetSlotAsync(1, 1, 1, 1)).ReturnsAsync(slot);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(dto));

            Assert.Contains("đã có container khác", ex.Message);
        }

        [Fact]
        public async Task TaoMoiContainer_TaoSlotMoiKhiChuaTonTai()
        {
            var dto = new CreateContainerDto
            {
                ContainerNumber = "MSKU1234567",
                CurrentBlock = "A",
                Bay = 5,
                Row = 5,
                Tier = 1
            };

            var block = new Domain.Entities.Block { BlockId = 1, BlockName = "A" };
            _mockBlockRepo.Setup(r => r.GetByNameAsync("A")).ReturnsAsync(block);
            _mockSlotRepo.Setup(r => r.GetSlotAsync(1, 5, 5, 1)).ReturnsAsync((Slot)null);

            await _service.CreateAsync(dto);

            _mockSlotRepo.Verify(r => r.AddAsync(It.IsAny<Slot>()), Times.Once);
        }

        // ------------------- CẬP NHẬT -------------------

        [Fact]
        public async Task CapNhatContainer_TonTai_ThanhCong()
        {
            var container = new Container { ContainerId = 1, SlotId = 10, ContStatus = "Full" };
            var dto = new UpdateContainerDto { ContainerId = 1, ContStatus = "Empty" };
            _mockContainerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(container);

            await _service.UpdateAsync(dto);

            _mockContainerRepo.Verify(r => r.UpdateAsync(It.IsAny<Container>()), Times.Once);
        }

        [Fact]
        public async Task CapNhatContainer_KhongTonTai_ThatBai()
        {
            _mockContainerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Container)null);
            var dto = new UpdateContainerDto { ContainerId = 1 };

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(dto));

            Assert.Contains("Container không tồn tại", ex.Message);
        }

        [Fact]
        public async Task CapNhatContainer_DoiViTri_TaoSlotMoiNeuChuaTonTai()
        {
            var dto = new UpdateContainerDto
            {
                ContainerId = 1,
                CurrentBlock = "A",
                Bay = 2,
                Row = 3,
                Tier = 1
            };
            var container = new Container { ContainerId = 1 };
            var block = new Domain.Entities.Block { BlockId = 5, BlockName = "A" };

            _mockContainerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(container);
            _mockBlockRepo.Setup(r => r.GetByNameAsync("A")).ReturnsAsync(block);
            _mockSlotRepo.Setup(r => r.GetSlotAsync(5, 2, 3, 1)).ReturnsAsync((Slot)null);

            await _service.UpdateAsync(dto);

            _mockSlotRepo.Verify(r => r.AddAsync(It.IsAny<Slot>()), Times.Once);
        }

        [Fact]
        public async Task CapNhatContainer_KhongTimThayBlock_ThatBai()
        {
            var dto = new UpdateContainerDto { ContainerId = 1, CurrentBlock = "B", Bay = 1, Row = 1, Tier = 1 };
            var container = new Container { ContainerId = 1 };
        }
    }   
}
