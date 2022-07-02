using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Data.DTOs.IdentityDTOs
{
    public class CadastroRequest
    {
        [Required(ErrorMessage = "Email obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatória")]
        [StringLength(30, ErrorMessage = "A senha precisa ter de 6 a 30 caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirmação de senha obrigatória")]
        [Compare(nameof(Senha), ErrorMessage = "Senha não confirmada")]
        public string SenhaConfirmacao { get; set; }
    }
}
