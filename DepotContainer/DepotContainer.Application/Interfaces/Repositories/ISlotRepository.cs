using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Interfaces.Repositories
{
    public interface ISlotRepository
    {
        Task<Slot?> GetSlotAsync(int blockId, int bay, int row, int tier);
        Task AddAsync(Slot slot);
        Task UpdateAsync(Slot slot);
    }
}
