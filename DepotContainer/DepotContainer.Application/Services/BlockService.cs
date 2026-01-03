using DepotContainer.Application.DTOs;
using DepotContainer.Application.Interfaces.Repositories;
using DepotContainer.Application.Interfaces.Services;
using DepotContainer.Domain.Entities;

namespace DepotContainer.Application.Services
{
    public class BlockService : IBlockService
    {
        private readonly IBlockRepository _blockRepository;

        public BlockService(IBlockRepository blockRepository)
        {
            _blockRepository = blockRepository;
        }
        public async Task<Block?> GetBlockWithSlotsAsync(int blockId)
        {
            return await _blockRepository.GetBlockWithSlotsAsync(blockId);
        }

        public async Task<IEnumerable<Block>> GetAllAsync()
        {
            return await _blockRepository.GetAllAsync();
        }

        public async Task<Block> GetByIdAsync(int id)
        {
            var block = await _blockRepository.GetByIdAsync(id);
            if (block == null)
                throw new Exception($"Block Id={id} không tồn tại.");
            return block;
        }

        public async Task<Block> CreateAsync(CreateBlockDto dto)
        {
            var exists = await _blockRepository.GetByNameAsync(dto.BlockName);
            if (exists != null)
                throw new Exception($"Block '{dto.BlockName}' đã tồn tại.");

            var block = new Block { BlockName = dto.BlockName };
            await _blockRepository.AddAsync(block);
            return block;
        }

        public async Task<Block> UpdateAsync(int id, UpdateBlockDto dto)
        {
            var block = await _blockRepository.GetByIdAsync(id);
            if (block == null)
                throw new Exception($"Block Id={id} không tồn tại.");

            block.BlockName = dto.BlockName;
            await _blockRepository.UpdateAsync(block);
            return block;
        }

        public async Task DeleteAsync(int id)
        {
            var block = await _blockRepository.GetByIdAsync(id);
            if (block == null)
                throw new Exception($"Block Id={id} không tồn tại.");

            await _blockRepository.DeleteAsync(block);
        }
    }
}
