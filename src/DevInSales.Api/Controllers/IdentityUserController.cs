using DevInSales.Core.Data.DTOs.ApiDTOs;
using DevInSales.Core.Data.DTOs.IdentityDTOs;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevInSales.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityUserController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityUserController(IIdentityService identityService) =>
        _identityService = identityService;

        /// <summary>
        /// [Administrador, Gerente] Cadastra um novo usuário.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exemplo de resposta:
        /// [
        ///   {
        ///     "id": 1
        ///   }
        /// ]
        /// </para>
        /// </remarks>
        /// <returns>Lista de endereços</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="204">Pesquisa realizada com sucesso porém não retornou nenhum resultado</response>
        /// <response code="400">Formato invalido</response>
        [Authorize(Roles = "Administrador, Gerente")]
        [HttpPost("criarUsuario")]
        public async Task<ActionResult> CriarUsuario(CadastroRequest usuarioCadastro)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                            .SelectMany(valores => valores.Errors)
                            .Select(erro => erro.ErrorMessage);
                return BadRequest(erros);
            }

            try
            {
                await _identityService.CadastrarUsuario(usuarioCadastro);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// [Administrador] Cadastra um novo gerente.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Exemplo de resposta:
        /// [
        ///   {
        ///     "id": 1
        ///   }
        /// ]
        /// </para>
        /// </remarks>
        /// <returns>Lista de endereços</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="204">Pesquisa realizada com sucesso porém não retornou nenhum resultado</response>
        /// <response code="400">Formato invalido</response>
        [Authorize(Roles = "Administrador")]
        [HttpPost("criarGerente")]
        public async Task<ActionResult> CriarGerente(CadastroRequest usuarioCadastro)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                            .SelectMany(valores => valores.Errors)
                            .Select(erro => erro.ErrorMessage);
                return BadRequest(erros);
            }

            try
            {
                await _identityService.CadastrarGerente(usuarioCadastro);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// [Administrador, Gerente, Usuario] Busca um usuário por id.
        /// </summary>
        /// <returns>Lista de endereços</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="404">Not Found, estado não encontrado no stateId informado.</response>
        [Authorize(Roles = "Administrador, Gerente, Usuario")]
        [HttpGet("id/{id}")]
        public async Task<ActionResult> ObterUserPorId(string id)
        {
            try
            {
                User usuario = await _identityService.ObterUsuarioPorId(id);
                UserResponse userDto = UserResponse.ConverterParaEntidade(usuario);

                return Ok(userDto);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// [Administrador, Gerente, Usuario] Retorna ID de usuário pelo Email.
        /// </summary>
        /// <returns>ID do usuário.</returns>
        /// <response code="204">ID com Email encontrado.</response>
        /// <response code="400">Erro ao buscar ID pelo Email.</response>
        [Authorize(Roles = "Administrador, Gerente, Usuario")]
        [HttpGet(("email/{email}"))]
        public async Task<ActionResult> RetornarIdPorEmail(string email)
        {
            try
            {
                string id = await _identityService.RetornarIdPorEmail(email);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// [Administrador, Gerente, Usuario] Busca uma lista de usuários.
        /// </summary>
        /// <returns>Lista de usuários</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="204">Pesquisa realizada com sucesso porém não retornou nenhum resultado</response>
        [Authorize(Roles = "Administrador, Gerente, Usuario")]
        [HttpGet("users")]
        public ActionResult<List<User>> ObterUsers()
        {
            List<User> users = _identityService.ObterUsuarios();

            if (users == null || users.Count == 0)
                return NoContent();

            var ListaDto = users.Select(user => UserResponse.ConverterParaEntidade(user)).ToList();

            return Ok(ListaDto);
        }

        /// <summary>
        /// [Administrador] Deleta um usuário.
        /// </summary>
        /// <response code="204">Endereço deletado com sucesso</response>
        /// <response code="404">Not Found, ID não encontrado.</response>
        [Authorize(Roles = "Administrador")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> ExcluirUser(string id)
        {
            try
            {
                await _identityService.ExcluirUsuarioPorId(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                    return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Realiza login.
        /// </summary>
        /// <response code="204">Login realizado com sucesso.</response>
        /// <response code="400">Erro ao realizar o login.</response>
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest usuarioLogin)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                            .SelectMany(valores => valores.Errors)
                            .Select(erro => erro.ErrorMessage);
                return BadRequest(erros);
            }

            try
            {
                var resultado = await _identityService.Login(usuarioLogin);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
