using DepotContainer.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepotContainer.Application.Interfaces.Repositories

{
    public interface IContainerRepository
    {
        Task<IEnumerable<Container>> GetAllAsync();
        Task<Container?> GetByIdAsync(int id);
        Task AddAsync(Container container);
        Task UpdateAsync(Container container);
        Task DeleteAsync(int id);
    }
}
