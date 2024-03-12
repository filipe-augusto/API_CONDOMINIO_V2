using API_CONDOMINIO_2.Data;
using API_CONDOMINIO_2.Models;
using API_CONDOMINIO_2.Services;
using API_CONDOMINIO_2.ViewModel;
using API_CONDOMINIO_V2.Repositories.Contracts;
using API_CONDOMINIO_V2.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace API_CONDOMINIO_V2.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountRepository(DataContext context , ITokenService tokenService )
        {
            _context = context;
            _tokenService = tokenService;
        }

        public bool CheckPassWord(string password, string passwordModel)
        =>  PasswordHasher.Verify(password, passwordModel);

        public string GetToken(User user)
        =>  _tokenService.GenerateToken(user);
        

        public async Task<User> GetUser(string email) 
            => await  _context
        .Users
        .AsNoTracking()
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.Email == email);


        public Task Login(LoginViewModel User)
        {
            throw new NotImplementedException();
        }
    }
}
