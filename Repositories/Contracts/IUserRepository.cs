using API_CONDOMINIO_2.Models;

namespace API_CONDOMINIO_V2.Repositories.Contracts;

    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User User);
        Task UpdateUserAsync(User User);
    }

