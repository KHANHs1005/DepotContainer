using DepotContainer.Application.DTOs;

namespace DepotContainer.Application.Interfaces.Services
{
    public interface IEirService
    {
        Task<IEnumerable<EirDto>> GetAllAsync();
        Task<EirDto?> GetByIdAsync(int id);
        Task<EirDto> CreateAsync(CreateEirDto dto);
        Task UpdateAsync(UpdateEirDto dto);
        Task DeleteAsync(int id);
    }
}
