    using DepotContainer.Application.Interfaces.Repositories;
    using DepotContainer.Domain.Entities;
    using DepotContainer.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    namespace DepotContainer.Infrastructure.Repositories
    {
        public class ContainerRepository : IContainerRepository
        {
            private readonly DepotDbContext _context;

            public ContainerRepository(DepotDbContext context)
            {
                _context = context;
            }

            // ✅ Lấy toàn bộ container kèm vị trí (Slot + Block)
            public async Task<IEnumerable<Container>> GetAllAsync()
            {
                return await _context.Containers
                    .Include(c => c.Slot)
                    .ThenInclude(s => s.Block)
                    .Include(c => c.Booking) // <-- Fixed here
                    .ToListAsync();
            }

            // ✅ Lấy theo ID kèm Slot + Block
            public async Task<Container?> GetByIdAsync(int id)
            {
                return await _context.Containers
                    .Include(c => c.Slot)
                    .ThenInclude(s => s.Block)
                    .Include(c => c.Booking) // <-- Fixed here
                    .FirstOrDefaultAsync(c => c.ContainerId == id);
            }

            public async Task AddAsync(Container container)
            {
                await _context.Containers.AddAsync(container);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(Container container)
            {
                _context.Containers.Update(container);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var container = await _context.Containers.FindAsync(id);
                if (container != null)
                {
                    _context.Containers.Remove(container);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
