using DepotContainer.Application.DTOs;

namespace DepotContainer.Application.Interfaces.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllAsync();
        Task<BookingDto?> GetByIdAsync(int id);
        Task<BookingDto> CreateAsync(CreateBookingDto dto);
        Task UpdateAsync(UpdateBookingDto dto);
        Task DeleteAsync(int id);
    }
}
