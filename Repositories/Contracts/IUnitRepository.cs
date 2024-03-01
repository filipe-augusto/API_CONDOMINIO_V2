using API_CONDOMINIO_2.Models;

namespace API_CONDOMINIO_V2.Repositories.Contracts;

    public interface IUnitRepository
    {
        Task<List<Unit>> GetAllUnitsAsync();
        Task<Unit> GetUnitByIdAsync(int id);
        Task AddUnitAsync(Unit Unit);
        Task UpdateUnitAsync(Unit Unit);
    }

