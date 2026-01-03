using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Interfaces.Repositories
{
    public interface ISlotRepository
    {
        Task<IEnumerable<Slot>> GetAllAsync();
        Task<Slot?> GetByIdAsync(int slotId);
        Task<IEnumerable<Slot>> GetByBlockAsync(int blockId);
        Task AddAsync(Slot slot);
        Task UpdateAsync(Slot slot);
        Task DeleteAsync(Slot slot);
        Task<Slot?> GetSlotAsync(int bay, int row, int tier, int blockId);
        Task SaveChangesAsync();
    }
}
