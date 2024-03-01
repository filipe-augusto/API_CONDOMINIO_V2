using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_V2.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace API_CONDOMINIO_V2.Repositories;

    public class BlockRepository : IBlockRepository
    {
        private readonly DataContext _context;

        public BlockRepository(DataContext context)
        => _context = context;
        
        public async Task<List<Block>> GetAllBlocksAsync()
        => await _context.Blocks.ToListAsync();
        

        public async Task<Block> GetBlockByIdAsync(int id)
        =>  await _context.Blocks.FirstOrDefaultAsync(x => x.Id == id);
        

        public async Task AddBlockAsync(Block block)
        {
            await _context.Blocks.AddAsync(block);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBlockAsync(Block block)
        {
            _context.Blocks.Update(block);
            await _context.SaveChangesAsync();
        }
    }

