using DepotContainer.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepotContainer.Application.Interfaces.Repositories
{
    public interface IBlockRepository
    {
        Task<Block?> GetBlockWithSlotsAsync(int blockId);

        Task<Block> GetByNameAsync(string name);
        Task<IEnumerable<Block>> GetAllAsync();
        Task<Block> GetByIdAsync(int id);
        Task AddAsync(Block block);
        Task UpdateAsync(Block block);
        Task DeleteAsync(Block block);

    }
}
