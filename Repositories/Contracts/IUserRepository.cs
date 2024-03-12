using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.ViewModel;

namespace API_CONDOMINIO_V2.Repositories.Contracts;

    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<string> AddUserAsync(RegisterViewModel User);
        Task UpdateUserAsync(User User);

}

