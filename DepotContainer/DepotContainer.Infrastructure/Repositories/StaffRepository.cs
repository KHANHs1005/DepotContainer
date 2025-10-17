using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Domain.Entities;
using DepotContainer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DepotContainer.Infrastructure.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly DepotDbContext _context;

        public StaffRepository(DepotDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            return await _context.Staffs
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Staff?> GetByIdAsync(int id)
        {
            return await _context.Staffs
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StaffId == id);
        }

        // 👇 thêm phương thức này để hỗ trợ login sau này
        public async Task<Staff?> GetByUsernameAsync(string username)
        {
            return await _context.Staffs
                .FirstOrDefaultAsync(s => s.StaffName == username);
        }

        public async Task AddAsync(Staff staff)
        {
            await _context.Staffs.AddAsync(staff);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Staff staff)
        {
            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Staff staff)
        {
            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();
        }
    }
}
