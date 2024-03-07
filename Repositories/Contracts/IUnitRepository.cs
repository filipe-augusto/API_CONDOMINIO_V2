using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;

namespace API_CONDOMINIO_V2.Repositories.Contracts;

    public interface IUnitRepository
    {
        Task<List<Unit>> GetAllUnitsAsync();
        Task<Unit> GetUnitByIdAsync(int id);
        Task<bool> AddUnitAsync(UnitViewModel model);
        Task UpdateUnitAsync(Unit Unit);
     
}

