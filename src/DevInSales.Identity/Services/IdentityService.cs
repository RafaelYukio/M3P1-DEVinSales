using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using DevInSales.Core.Data.DTOs.IdentityDTOs;
using DevInSales.Identity.Configurations;
using DevInSales.Core.Interfaces.Identity;
using DevInSales.Core.Entities;

namespace DevInSales.Identity.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtOptions _jwtOptions;

        public IdentityService(SignInManager<IdentityUser> signInManager,
                               UserManager<IdentityUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IOptions<JwtOptions> jwtOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task CadastrarUsuario(CadastroRequest usuarioCadastro)
        {
            IdentityUser? identityUser = new IdentityUser
            {
                UserName = usuarioCadastro.Email,
                Email = usuarioCadastro.Email,
            };

            IdentityRole? identityRole = new IdentityRole("Usuario");

            var novoUsuario = await _userManager.CreateAsync(identityUser, usuarioCadastro.Senha);
            if (novoUsuario.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser, identityRole.Name);
                return;
            }

            throw new ArgumentException(novoUsuario.Errors.FirstOrDefault().Description);
        }

        public async Task<User> ObterUsuarioPorId(string id)
        {
            IdentityUser identityUser = await _userManager.FindByIdAsync(id);

            if(identityUser == null)
            {
                throw new ArgumentException("Usuário com ID não encontrado");
            }

            User usuario = new User(identityUser.Id, identityUser.Email, identityUser.PasswordHash, identityUser.UserName);

            return usuario;
        }

        public List<User> ObterUsuarios()
        {
            List<IdentityUser> identityUserList = _userManager.Users.ToList();
            List<User> userList = new List<User>();

            identityUserList.ForEach(identityUser => userList.Add(new User(identityUser.Id, identityUser.Email, identityUser.PasswordHash, identityUser.UserName)));

            return userList;
        }
        public async Task ExcluirUsuarioPorId(string id)
        {
            IdentityUser identityUser = await _userManager.FindByIdAsync(id);

            if (identityUser == null)
            {
                throw new ArgumentException("Usuário com ID não encontrado");
            }

            await _userManager.DeleteAsync(identityUser);
        }

        public async Task<LoginResponse> Login(LoginRequest usuarioLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, false);
            if (result.Succeeded)
                return await GerarCredenciais(usuarioLogin.Email);

            throw new ArgumentException("Usuário ou senha incorreto!");
        }

        private async Task<LoginResponse> GerarCredenciais(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var accessTokenClaims = await ObterClaims(user);

            var dataExpiracaoAccessToken = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);

            var accessToken = GerarToken(accessTokenClaims, dataExpiracaoAccessToken);

            return new LoginResponse(accessToken);
        }

        private string GerarToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
        {
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: dataExpiracao,
                signingCredentials: _jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<IList<Claim>> ObterClaims(IdentityUser user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(userClaims);

            foreach (var role in roles)
                claims.Add(new Claim("role", role));

            return claims;
        }

        public async Task<String> RetornarIdPorEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user.Id;
        }
    }
}
