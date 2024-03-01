using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_V2.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace API_CONDOMINIO_V2.Repositories
{
    public class ResidentRepository : IResidentRepository
    {
        private readonly DataContext _context;

        public ResidentRepository(DataContext context)
            => _context = context;
        
        public async Task<List<Resident>> GetAllResidentsAsync()
              =>  await _context.Residents.ToListAsync();
        

        public async Task<Resident> GetResidentByIdAsync(int id)
        => await _context.Residents.FirstOrDefaultAsync(x => x.Id == id);
        

        public async Task AddResidentAsync(Resident Resident)
        {
            await _context.Residents.AddAsync(Resident);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateResidentAsync(Resident Resident)
        {
            _context.Residents.Update(Resident);
            await _context.SaveChangesAsync();
        }
    }
}
