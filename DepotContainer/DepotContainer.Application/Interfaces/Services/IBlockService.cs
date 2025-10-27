using DepotContainer.Application.DTOs;
using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Interfaces.Services
{
    public interface IBlockService
    {
        Task<Block?> GetBlockWithSlotsAsync(int blockId);
        Task<IEnumerable<Block>> GetAllAsync();
        Task<Block> GetByIdAsync(int id);
        Task<Block> CreateAsync(CreateBlockDto dto);
        Task<Block> UpdateAsync(int id, UpdateBlockDto dto);
        Task DeleteAsync(int id);

    }
}
