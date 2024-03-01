using API_CONDOMINIO_2.Models;

namespace API_CONDOMINIO_V2.Repositories.Contracts;

    public interface IResidentRepository
    {
        Task<List<Resident>> GetAllResidentsAsync();
        Task<Resident> GetResidentByIdAsync(int id);
        Task AddResidentAsync(Resident Resident);
        Task UpdateResidentAsync(Resident Resident);
    }

