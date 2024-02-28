using System.ComponentModel.DataAnnotations;

namespace API_CONDOMINIO_2.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatorio")]
        [EmailAddress(ErrorMessage = "O e-mail é inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatorio")]
        public string Password { get; set; }

    }
}
