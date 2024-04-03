using System.ComponentModel.DataAnnotations;

namespace API_CONDOMINIO_2.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatorio")]
        [EmailAddress(ErrorMessage = "O e-mail é inválido")]
        public string Email { get; set; }

        public string password { get; set; }

        public int IdRole { get; set; }

        public string? Image { get; set; }
    }
}
