using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Interfaces.Repositories
{
    public interface IEirRepository
    {
        Task<IEnumerable<EIR>> GetAllAsync();
        Task<EIR?> GetByIdAsync(int id);
        Task AddAsync(EIR eir);
        Task UpdateAsync(EIR eir);
        Task DeleteAsync(EIR eir);
    }
}
