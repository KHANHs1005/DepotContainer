using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Application.Interfaces.Services;
using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Services
{
    public class ContainerService : IContainerService
    {
        private readonly IContainerRepository _containerRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly ISlotRepository _slotRepository;

        public ContainerService(
            IContainerRepository containerRepository,
            IBlockRepository blockRepository,
            ISlotRepository slotRepository)
        {
            _containerRepository = containerRepository;
            _blockRepository = blockRepository;
            _slotRepository = slotRepository;
        }

        // GET ALL
        public async Task<IEnumerable<ContainerDto>> GetAllAsync()
        {
            var containers = await _containerRepository.GetAllAsync();
            return containers.Select(c => new ContainerDto
            {
                ContainerId = c.ContainerId,
                OperatorName = c.OperatorName,
                ContainerNumber = c.ContainerNo,
                ContStatus = c.ContStatus,
                ContCondition = c.ContCondition,
                SlotId = c.SlotId,
                CurrentBlock = c.Slot?.Block?.BlockName ?? string.Empty,
                Bay = c.Slot?.Bay,
                Row = c.Slot?.Row,
                Tier = c.Slot?.Tier,
                Size = c.ContIso?.Length?.ToString() ?? string.Empty,
                Type = c.ContIso?.Description ?? string.Empty
            });
        }

        // GET BY ID
        public async Task<ContainerDto?> GetByIdAsync(int id)
        {
            var c = await _containerRepository.GetByIdAsync(id);
            if (c == null) return null;

            return new ContainerDto
            {
                ContainerId = c.ContainerId,
                OperatorName= c.OperatorName,
                ContainerNumber = c.ContainerNo,
                ContStatus = c.ContStatus,
                ContCondition = c.ContCondition,
                SlotId = c.SlotId,
                CurrentBlock = c.Slot?.Block?.BlockName ?? string.Empty,
                Bay = c.Slot?.Bay,
                Row = c.Slot?.Row,
                Tier = c.Slot?.Tier,
                Size = c.ContIso?.Length?.ToString() ?? string.Empty,
                Type = c.ContIso?.Description ?? string.Empty
            };
        }

        // CREATE CONTAINER — user chọn vị trí cụ thể
        public async Task<ContainerDto> CreateAsync(CreateContainerDto dto)
        {
            // 1️ Kiểm tra input
            if (string.IsNullOrEmpty(dto.ContainerNumber))
                throw new Exception("ContainerNumber là bắt buộc.");

            if (string.IsNullOrEmpty(dto.CurrentBlock) || dto.Bay == null || dto.Row == null || dto.Tier == null)
                throw new Exception("Vui lòng nhập đầy đủ CurrentBlock, Bay, Row, Tier.");

            // 2 Tìm block
            var block = await _blockRepository.GetByNameAsync(dto.CurrentBlock);
            if (block == null)
                throw new Exception($"Block '{dto.CurrentBlock}' không tồn tại.");

            var slot = await _slotRepository.GetSlotAsync(block.BlockId, dto.Bay.Value, dto.Row.Value, dto.Tier.Value);
            if (slot == null)
            {
                slot = new Slot
                {
                    BlockId = block.BlockId,
                    Bay = dto.Bay.Value,
                    Row = dto.Row.Value,
                    Tier = dto.Tier.Value,
                    StatusSlot = "Empty"
                };
                await _slotRepository.AddAsync(slot);
            }
            else if (slot.StatusSlot == "Full")
            {
                throw new Exception($"Vị trí {block.BlockName}-{dto.Bay}-{dto.Row}-{dto.Tier} đã có container khác.");
            }

            var container = new Container
            {
                ContainerNo = dto.ContainerNumber,
                OperatorName = dto.OperatorName,
                ContStatus = dto.ContStatus ?? "Empty",
                ContCondition = dto.ContCondition ?? "Good",
                IsEmpty = (dto.ContStatus?.ToLower() == "empty"),
                SlotId = slot.SlotId,
                TimeIn = DateTime.Now
            };

            await _containerRepository.AddAsync(container);

            // 6️⃣ Cập nhật slot -> Full
            slot.StatusSlot = "Full";
            await _slotRepository.UpdateAsync(slot);

            // 7️⃣ Trả kết quả
            return new ContainerDto
            {
                ContainerId = container.ContainerId,
                OperatorName = container.OperatorName,
                ContainerNumber = container.ContainerNo,
                ContStatus = container.ContStatus,
                ContCondition = container.ContCondition,
                SlotId = slot.SlotId,
                CurrentBlock = block.BlockName,
                Bay = slot.Bay,
                Row = slot.Row,
                Tier = slot.Tier
            };
        }

        // UPDATE CONTAINER
        public async Task UpdateAsync(UpdateContainerDto dto)
        {
            var container = await _containerRepository.GetByIdAsync(dto.ContainerId);
            if (container == null)
                throw new Exception("Container không tồn tại.");

            container.ContStatus = dto.ContStatus ?? container.ContStatus;
            container.ContCondition = dto.ContCondition ?? container.ContCondition;

            // Nếu user đổi vị trí
            if (!string.IsNullOrEmpty(dto.CurrentBlock) && dto.Bay != null && dto.Row != null && dto.Tier != null)
            {
                var block = await _blockRepository.GetByNameAsync(dto.CurrentBlock);
                if (block == null)
                    throw new Exception($"Block '{dto.CurrentBlock}' không tồn tại.");

                var slot = await _slotRepository.GetSlotAsync(block.BlockId, dto.Bay.Value, dto.Row.Value, dto.Tier.Value);
                if (slot == null)
                {
                    slot = new Slot
                    {
                        BlockId = block.BlockId,
                        Bay = dto.Bay.Value,
                        Row = dto.Row.Value,
                        Tier = dto.Tier.Value,
                        StatusSlot = "Empty"
                    };
                    await _slotRepository.AddAsync(slot);
                }

                container.SlotId = slot.SlotId;
            }

            await _containerRepository.UpdateAsync(container);
        }
        // DELETE CONTAINER
        public async Task DeleteAsync(int id)
        {
            var container = await _containerRepository.GetByIdAsync(id);
            if (container == null)
                throw new Exception("Container không tồn tại.");

            await _containerRepository.DeleteAsync(container.ContainerId);
        }
    }
}
