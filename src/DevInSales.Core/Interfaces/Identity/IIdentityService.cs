using DevInSales.Core.Data.DTOs.IdentityDTOs;
using DevInSales.Core.Entities;
using System.Security.Claims;

namespace DevInSales.Core.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task CadastrarUsuario(CadastroRequest usuarioCadastro);
        Task CadastrarGerente(CadastroRequest usuarioCadastro);
        Task<User> ObterUsuarioPorId(string id);
        List<User> ObterUsuarios();
        Task ExcluirUsuarioPorId(string id);
        Task<LoginResponse> Login(LoginRequest usuarioLogin);
        Task<String> RetornarIdPorEmail(string email);
    }
}
