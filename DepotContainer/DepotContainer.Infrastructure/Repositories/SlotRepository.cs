using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Domain.Entities;
using DepotContainer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DepotContainer.Infrastructure.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly DepotDbContext _context;

        public SlotRepository(DepotDbContext context)
        {
            _context = context;
        }

        public async Task<Slot?> GetSlotAsync(int blockId, int bay, int row, int tier)
        {
            return await _context.Slots
                .Include(s => s.Block)
                .FirstOrDefaultAsync(s =>
                    s.BlockId == blockId &&
                    s.Bay == bay &&
                    s.Row == row &&
                    s.Tier == tier);
        }

        public async Task AddAsync(Slot slot)
        {
            await _context.Slots.AddAsync(slot);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Slot slot)
        {
            _context.Slots.Update(slot);
            await _context.SaveChangesAsync();
        }
    }
}
