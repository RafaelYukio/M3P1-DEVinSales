using System.ComponentModel.DataAnnotations;

namespace DevInSales.Core.Data.DTOs.IdentityDTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatória")]
        public string Senha { get; set; }
    }
}
