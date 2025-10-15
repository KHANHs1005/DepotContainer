using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories;  // ✅ thêm dòng này
using DepotContainer.Application.Interfaces.Services;
using DepotContainer.Domain.Entities;


namespace DepotContainer.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<BookingDto>> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Select(b => new BookingDto
            {
                BookingId = b.BookingId,
                BookingNumber = b.BookingNumber,
                ContSize = b.ContSize,
                ContQuantity = b.ContQuantity,
                OperatorName = b.OperatorName,
                ReleaseExpireDate = b.ReleaseExpireDate,
                CusId = b.CustomerId
            });
        }

        public async Task<BookingDto?> GetByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                return null;

            return new BookingDto
            {
                BookingId = booking.BookingId,
                BookingNumber = booking.BookingNumber,
                ContSize = booking.ContSize,
                ContQuantity = booking.ContQuantity,
                OperatorName = booking.OperatorName,
                ReleaseExpireDate = booking.ReleaseExpireDate,
                CusId = booking.CustomerId
            };
        }

        public async Task<BookingDto> CreateAsync(CreateBookingDto dto)
        {
            var booking = new Booking
            {
                BookingNumber = dto.BookingNumber,
                ContSize = dto.ContSize,
                ContQuantity = dto.ContQuantity,
                OperatorName = dto.OperatorName,
                ReleaseExpireDate = dto.ReleaseExpireDate,
                CustomerId = dto.CusId
            };

            await _bookingRepository.AddAsync(booking);

            return new BookingDto
            {
                BookingId = booking.BookingId,
                BookingNumber = booking.BookingNumber,
                ContSize = booking.ContSize,
                ContQuantity = booking.ContQuantity,
                OperatorName = booking.OperatorName,
                ReleaseExpireDate = booking.ReleaseExpireDate,
                CusId = booking.CustomerId
            };
        }

        public async Task UpdateAsync(UpdateBookingDto dto)
        {
            var booking = await _bookingRepository.GetByIdAsync(dto.BookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            booking.ContSize = dto.ContSize;
            booking.ContQuantity = dto.ContQuantity;
            booking.OperatorName = dto.OperatorName;
            booking.ReleaseExpireDate = dto.ReleaseExpireDate;

            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task DeleteAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                throw new Exception("Booking not found");

            await _bookingRepository.DeleteAsync(booking);
        }
    }
}
