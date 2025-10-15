using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Interfaces.Repositories
{
    public interface IBlockRepository
    {
        Task<Block?> GetByNameAsync(string blockName);
    }
}
