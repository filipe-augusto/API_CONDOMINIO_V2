using API_CONDOMINIO_2.Models;

namespace API_CONDOMINIO_V2.Services.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
