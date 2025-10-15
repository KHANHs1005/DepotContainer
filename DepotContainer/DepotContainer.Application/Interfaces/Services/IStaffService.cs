using DepotContainer.Application.DTOs;

namespace DepotContainer.Application.Interfaces.Services
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffDto>> GetAllAsync();
        Task<StaffDto?> GetByIdAsync(int id);
        Task<StaffDto> CreateAsync(CreateStaffDto dto);
        Task UpdateAsync(UpdateStaffDto dto);
        Task DeleteAsync(int id);
    }
}
