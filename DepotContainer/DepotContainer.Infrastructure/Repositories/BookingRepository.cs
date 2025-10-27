using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Domain.Entities;
using DepotContainer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DepotContainer.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DepotDbContext _context;

        public BookingRepository(DepotDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        // ✅ SỬA: Include đúng navigation property name
        public async Task<Booking?> GetBookingWithDetailsAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Container)
                    .ThenInclude(c => c.ContIso) // ✅ Sửa: ContainerISO -> ContIso
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        // ✅ SỬA: Include đúng navigation property name
        public async Task<Booking?> GetBookingWithDetailsByNumberAsync(string bookingNumber)
        {
            return await _context.Bookings
                .Include(b => b.Container)
                    .ThenInclude(c => c.ContIso) // ✅ Sửa: ContainerISO -> ContIso
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.BookingNumber == bookingNumber);
        }
    }
}