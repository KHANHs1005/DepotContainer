using DepotContainer.Domain.Entities;
using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DepotContainer.Infrastructure.Repositories
{
    public class EirRepository : IEirRepository
    {
        private readonly DepotDbContext _context;

        public EirRepository(DepotDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EIR>> GetAllAsync()
        {
            return await _context.Eirs
                .Include(e => e.Container)
                .Include(e => e.Customer)
                .Include(e => e.Staff)
                .ToListAsync();
        }

        public async Task<EIR?> GetByIdAsync(int id)
        {
            return await _context.Eirs
                .Include(e => e.Container)
                .Include(e => e.Customer)
                .Include(e => e.Staff)
                .FirstOrDefaultAsync(e => e.EirId == id);
        }

        public async Task AddAsync(EIR eir)
        {
            _context.Eirs.Add(eir);
            await _context.SaveChangesAsync();
            await _context.Entry(eir).Reference(e => e.Container).LoadAsync();
            await _context.Entry(eir).Reference(e => e.Customer).LoadAsync();
            await _context.Entry(eir).Reference(e => e.Staff).LoadAsync();
        }

        public async Task UpdateAsync(EIR eir)
        {
            _context.Eirs.Update(eir);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(EIR eir)
        {
            _context.Eirs.Remove(eir);
            await _context.SaveChangesAsync();
        }
    }
}
