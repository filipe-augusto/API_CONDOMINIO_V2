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

        public async Task<List<Resident>> GetAllResidentsWithDetailsAsync()
        {
            return await _context.Residents
                .Include(x => x.Sex)
                .Include(x => x.Unit)
                    .ThenInclude(x => x.Block)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Unit> GetUnitByIdAsync(int id)
        {
            return await _context.Units.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Sex> GetSexByIdAsync(int id)
        {
            return await _context.Sex.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Resident> GetResidentByIdWithDetailsAsync(int id)
        {
            return await _context.Residents
                .Include(x => x.Sex)
                .Include(x => x.Unit)
                    .ThenInclude(x => x.Block)
                .FirstOrDefaultAsync(y => y.Id == id);
        }

        public async Task DeleteResidentAsync(Resident resident)
        {
        
            if (resident != null)
            {
                resident.Excluded = true;
                resident.ExclusionDate = DateTime.Now;
                _context.Residents.Update(resident);
                await _context.SaveChangesAsync();
            }
        }
    }
}
