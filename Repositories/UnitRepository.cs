using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;
using API_CONDOMINIO_V2.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace API_CONDOMINIO_V2.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly DataContext _context;

        public UnitRepository(DataContext context)
        =>  _context = context;
        
        public async Task<List<Unit>> GetAllUnitsAsync()
        => await _context.Units.ToListAsync();
        

        public async Task<Unit> GetUnitByIdAsync(int id)
        => await _context.Units.FirstOrDefaultAsync(x => x.Id == id);
        

        public async Task<bool> AddUnitAsync(UnitViewModel model)
        {
            var block = await _context.Blocks.FirstOrDefaultAsync(x=> x.Id == model.BlockId);
            if (block == null)
                throw new ArgumentException("Invalid unit");
            var unit = new Unit
            {
                NumberUnit = model.NumberUnit,
                Block = block,
                PeopleLiving = model.PeopleLiving,
                Observation = model.Observation,
                HasGarage = model.HasGarage
            };
            await _context.Units.AddAsync(unit);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task UpdateUnitAsync(Unit Unit)
        {
            _context.Units.Update(Unit);
            await _context.SaveChangesAsync();
        }
    }
}
