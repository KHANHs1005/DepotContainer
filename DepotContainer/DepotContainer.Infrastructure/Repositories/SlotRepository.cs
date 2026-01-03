using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Domain.Entities;
using DepotContainer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


public class SlotRepository : ISlotRepository
{
    private readonly DepotDbContext _context;

    public SlotRepository(DepotDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Slot slot)
    {
        await _context.Slots.AddAsync(slot);
        await _context.SaveChangesAsync();
    }

    public async Task<Slot?> GetByIdAsync(int id)
    {
        return await _context.Slots
            .Include(s => s.Block)
            .FirstOrDefaultAsync(s => s.SlotId == id);
    }

    public async Task<IEnumerable<Slot>> GetAllAsync()
    {
        return await _context.Slots
            .Include(s => s.Block)
            .ToListAsync();
    }

    public async Task UpdateAsync(Slot slot)
    {
        _context.Slots.Update(slot);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Slot slot)
    {
        _context.Slots.Remove(slot);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Slot>> GetByBlockAsync(int blockId)
    {
        return await _context.Slots
            .Include(s => s.Block)
            .Where(s => s.BlockId == blockId)
            .ToListAsync();
    }

    public async Task<Slot?> GetSlotAsync(int bay, int row, int tier, int blockId)
    {
        return await _context.Slots
            .Include(s => s.Block)
            .FirstOrDefaultAsync(s =>
                s.Bay == bay &&
                s.Row == row &&
                s.Tier == tier &&
                s.BlockId == blockId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
