using DepotContainer.Application.DTOs;

namespace DepotContainer.Application.Interfaces.Services
{
    public interface IContainerService
    {
        Task<IEnumerable<ContainerDto>> GetAllAsync();
        Task<ContainerDto?> GetByIdAsync(int id);
        Task<ContainerDto> CreateAsync(CreateContainerDto dto);
        Task UpdateAsync(UpdateContainerDto dto);
        Task DeleteAsync(int id);
    }
}
