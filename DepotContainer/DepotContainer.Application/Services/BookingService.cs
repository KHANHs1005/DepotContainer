using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories;
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

        // ✅ METHOD MỚI: Lấy booking với chi tiết containers theo ID
        public async Task<BookingDetailDto?> GetBookingDetailsAsync(int id)
        {
            var booking = await _bookingRepository.GetBookingWithDetailsAsync(id);
            
            if (booking == null)
                return null;

            return MapToBookingDetailDto(booking);
        }

        // ✅ METHOD MỚI: Lấy booking với chi tiết containers theo Booking Number
        public async Task<BookingDetailDto?> GetBookingDetailsByNumberAsync(string bookingNumber)
        {
            var booking = await _bookingRepository.GetBookingWithDetailsByNumberAsync(bookingNumber);
            
            if (booking == null)
                return null;

            return MapToBookingDetailDto(booking);
        }

        public async Task<BookingDto> CreateAsync(CreateBookingDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.BookingNumber))
                throw new Exception("BookingNumber là bắt buộc");

            var existing = await _bookingRepository.GetAllAsync();
            if (existing.Any(b => b.BookingNumber == dto.BookingNumber))
                throw new Exception("BookingNumber đã tồn tại");

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
                throw new Exception("Booking không tồn tại");

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
                throw new Exception("Booking không tồn tại");

            await _bookingRepository.DeleteAsync(booking);
        }

        // ✅ HELPER METHOD: Map Entity sang BookingDetailDto
        private BookingDetailDto MapToBookingDetailDto(Booking booking)
        {
            return new BookingDetailDto
            {
                BookingId = booking.BookingId,
                BookingNumber = booking.BookingNumber,
                ContSize = booking.ContSize,
                ContQuantity = booking.ContQuantity,
                OperatorName = booking.OperatorName,
                ReleaseExpireDate = booking.ReleaseExpireDate,
                CusId = booking.CustomerId,
                CustomerName = booking.Customer?.Name, // Điều chỉnh theo property trong Customer entity
                Containers = booking.Container?.Select(c => new ContainerInBookingDto
                {
                    ContId = c.ContainerId,
                    ContNo = c.ContainerNo,
                    ContType = c.ContIso?.IsoCode,     // GP/40FT, HC/40FT...
                    ContSize = booking.ContSize,    // 20FT, 40FT...
                    OperatorName = c.OperatorName,
                    ContStatus = c.ContStatus,              // Empty, Full
                    ContCondition = c.ContCondition,        // Good, Damaged, Under_repair
                    HasEir = c.TimeIn != null,              // Đã có Gate-In EIR
                    TimeIn = c.TimeIn,
                    TimeOut = c.TimeOut,
                    Weight = (float?)c.Weight
                }).ToList() ?? new List<ContainerInBookingDto>()
            };
        }
    }
}