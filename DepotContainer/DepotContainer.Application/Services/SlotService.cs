using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Application.Interfaces.Services;
using DepotContainer.Domain.Entities;

public class SlotService : ISlotService
{
    private readonly ISlotRepository _slotRepository;

    // Property để quản lý block
    public IBlockService BlockService { get; }

    public SlotService(ISlotRepository slotRepository, IBlockService blockService)
    {
        _slotRepository = slotRepository;
        BlockService = blockService;
    }

    // Tạo Slot mới
    public async Task<Slot> CreateSlotAsync(CreateSlotDto dto)
    {
        // Kiểm tra block tồn tại
        var block = await BlockService.GetByIdAsync(dto.BlockId);
        if (block == null) throw new Exception($"Block Id={dto.BlockId} không tồn tại.");

        var slot = new Slot
        {
            Bay = dto.Bay,
            Row = dto.Row,
            Tier = dto.Tier,
            BlockId = dto.BlockId
        };

        await _slotRepository.AddAsync(slot);
        return slot;
    }

    // Lấy tất cả Slot
    public async Task<IEnumerable<Slot>> GetAllAsync()
    {
        return await _slotRepository.GetAllAsync();
    }

    // Lấy Slot theo Id
    public async Task<Slot> GetSlotByIdAsync(int id)
    {
        return await _slotRepository.GetByIdAsync(id);
    }

    // Cập nhật Slot
    public async Task<Slot> UpdateSlotAsync(int id, UpdateSlotDto dto)
    {
        var slot = await _slotRepository.GetByIdAsync(id);
        if (slot == null) throw new Exception($"Slot Id={id} không tồn tại.");

        // Kiểm tra block tồn tại
        var block = await BlockService.GetByIdAsync(dto.BlockId);
        if (block == null) throw new Exception($"Block Id={dto.BlockId} không tồn tại.");

        slot.Bay = dto.Bay;
        slot.Row = dto.Row;
        slot.Tier = dto.Tier;
        slot.BlockId = dto.BlockId; 

        await _slotRepository.UpdateAsync(slot);
        return slot;
    }

    // Xóa Slot
    public async Task DeleteSlotAsync(int id)
    {
        var slot = await _slotRepository.GetByIdAsync(id);
        if (slot == null) throw new Exception($"Slot Id={id} không tồn tại.");

        await _slotRepository.DeleteAsync(slot);
    }
}
