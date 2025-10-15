using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Domain.Entities;
using DepotContainer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DepotContainer.Infrastructure.Repositories
{
    public class BlockRepository : IBlockRepository
    {
        private readonly DepotDbContext _context;

        public BlockRepository(DepotDbContext context)
        {
            _context = context;
        }

        public async Task<Block?> GetByNameAsync(string blockName)
        {
            return await _context.Blocks.FirstOrDefaultAsync(b => b.BlockName == blockName);
        }

    }
}
