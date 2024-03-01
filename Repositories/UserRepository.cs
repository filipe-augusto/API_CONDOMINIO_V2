using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_V2.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace API_CONDOMINIO_V2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        => _context = context;

        public async Task<List<User>> GetAllUsersAsync()
        => await _context.Users.ToListAsync();
        
        public async Task<User> GetUserByIdAsync(int id)
        =>  await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        
        public async Task AddUserAsync(User User)
        {
            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User User)
        {
            _context.Users.Update(User);
            await _context.SaveChangesAsync();
        }
    }
}
