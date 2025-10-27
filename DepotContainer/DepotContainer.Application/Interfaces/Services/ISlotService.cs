using DepotContainer.Application.DTOs;
using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Interfaces.Services
{
    public interface ISlotService
    {
        Task<Slot> CreateSlotAsync(CreateSlotDto dto);
        Task<Slot> UpdateSlotAsync(int id, UpdateSlotDto dto);
        Task DeleteSlotAsync(int id);
        Task<Slot> GetSlotByIdAsync(int id);
        Task<IEnumerable<Slot>> GetAllAsync();
        IBlockService BlockService { get; }
            
    }
}
