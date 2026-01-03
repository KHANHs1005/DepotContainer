using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Interfaces.Repositories
{
    public interface IStaffRepository
    {
        Task<IEnumerable<Staff>> GetAllAsync();
        Task<Staff?> GetByIdAsync(int id);
        Task<Staff?> GetByUsernameAsync(string username);
        Task AddAsync(Staff staff);
        Task UpdateAsync(Staff staff);
        Task DeleteAsync(Staff staff);
    }
}
