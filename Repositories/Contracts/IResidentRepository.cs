using API_CONDOMINIO_2.Models;

namespace API_CONDOMINIO_V2.Repositories.Contracts;

public interface IResidentRepository
{
    Task<List<Resident>> GetAllResidentsAsync();
    Task<Resident> GetResidentByIdAsync(int id);
    Task<Resident> GetResidentByIdWithDetailsAsync(int id);
    Task<List<Resident>> GetAllResidentsWithDetailsAsync();
    Task AddResidentAsync(Resident Resident);
    Task UpdateResidentAsync(Resident Resident);
    Task DeleteResidentAsync(Resident Resident);
    Task<Unit> GetUnitByIdAsync(int id);
    Task<Sex> GetSexByIdAsync(int id);
}

