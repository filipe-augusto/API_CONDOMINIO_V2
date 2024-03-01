using API_CONDOMINIO_2.Models;

namespace API_CONDOMINIO_V2.Repositories.Contracts;

    public interface IBlockRepository
    {
        Task<List<Block>> GetAllBlocksAsync();
        Task<Block> GetBlockByIdAsync(int id);
        Task AddBlockAsync(Block block);
        Task UpdateBlockAsync(Block block);
    }

