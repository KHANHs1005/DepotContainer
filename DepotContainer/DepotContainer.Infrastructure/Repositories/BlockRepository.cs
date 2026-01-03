using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Domain.Entities;
using DepotContainer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
public class BlockRepository : IBlockRepository
{
    private readonly DepotDbContext _context;

    public BlockRepository(DepotDbContext context)
    {
        _context = context;
    }
    public async Task<Block?> GetBlockWithSlotsAsync(int blockId)
    {
        return await _context.Blocks
            .Include(b => b.Slots!)
                .ThenInclude(s => s.Container)
            .FirstOrDefaultAsync(b => b.BlockId == blockId);
    }
    public async Task AddAsync(Block block)
    {
        await _context.Blocks.AddAsync(block);
        await _context.SaveChangesAsync();
    }

    public async Task<Block?> GetByIdAsync(int id)
    {
        return await _context.Blocks
            .Include(b => b.Slots) // eager load Slots nếu cần
            .FirstOrDefaultAsync(b => b.BlockId == id);
    }

    public async Task<Block?> GetByNameAsync(string name)
    {
        return await _context.Blocks.FirstOrDefaultAsync(b => b.BlockName == name);
    }

    public async Task<IEnumerable<Block>> GetAllAsync()
    {
        return await _context.Blocks
            .Include(b => b.Slots)
            .ToListAsync();
    }

    public async Task UpdateAsync(Block block)
    {
        _context.Blocks.Update(block);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Block block)
    {
        _context.Blocks.Remove(block);
        await _context.SaveChangesAsync();
    }
}
