using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;
using API_CONDOMINIO_V2.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Security.Cryptography;

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
        => await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        
        public async Task<string> AddUserAsync(RegisterViewModel model)
        {
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
            };
            var role = await _context.Role.FirstOrDefaultAsync(x => x.Id == model.IdRole);
            user.Roles = new List<Role> { role };

            user.PasswordHash = PasswordHasher.Hash(model.password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return model.password;
        }


        public async Task UpdateUserAsync(User User)
        {
            _context.Users.Update(User);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User User)
        {
            _context.Users.Remove(User);
            await _context.SaveChangesAsync();  
        }
    }
}
