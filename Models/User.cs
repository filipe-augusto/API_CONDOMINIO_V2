using System.Text.Json.Serialization;

namespace API_CONDOMINIO_2.Models;


public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    [JsonIgnore]
    public string PasswordHash { get; set; }
    public string Image { get; set; }
    public bool Blocked { get; set; }
    public IList<Role> Roles { get; set; }
}

