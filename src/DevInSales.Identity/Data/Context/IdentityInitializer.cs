using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DevInSales.Core.Interfaces.Identity;

namespace DevInSales.Identity.Data.Context
{
    public class IdentityInitializer : IIdentityInitializer
    {
        private readonly IdentityDataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityInitializer(IdentityDataContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initializer()
        {
            await _context.Database.MigrateAsync();
            bool inicializado = await _context.Database.EnsureCreatedAsync();

            if (!inicializado)
            {
                await _roleManager.CreateAsync(new IdentityRole("Administrador"));
                await _roleManager.CreateAsync(new IdentityRole("Gerente"));
                await _roleManager.CreateAsync(new IdentityRole("Usuario"));

                List<IdentityUser> listaDeUsuarios = new List<IdentityUser> {
                new IdentityUser
                {
                UserName = "usuario1@dev.com.br",
                Email = "usuario1@dev.com.br",
                },
                new IdentityUser
                {
                UserName = "usuario2@dev.com.br",
                Email = "usuario2@dev.com.br",
                },
                new IdentityUser
                {
                UserName = "usuario3@dev.com.br",
                Email = "usuario3@dev.com.br",
                },
                new IdentityUser
                {
                UserName = "usuario4@dev.com.br",
                Email = "usuario4@dev.com.br",
                }
                };

                IdentityRole? identityRoleUsuario = new IdentityRole("Usuario");

                foreach (IdentityUser usuario in listaDeUsuarios)
                {
                    await _userManager.CreateAsync(usuario, "@Usuario1234");
                    await _userManager.AddToRoleAsync(usuario, identityRoleUsuario.Name);
                }

                List<IdentityUser> listaDeGerentes = new List<IdentityUser> {
                new IdentityUser
                {
                UserName = "gerente1@dev.com.br",
                Email = "gerente1@dev.com.br",
                },
                new IdentityUser
                {
                UserName = "gerente2@dev.com.br",
                Email = "gerente2@dev.com.br",
                },
                };

                IdentityRole? identityRoleGerente = new IdentityRole("Gerente");

                foreach (IdentityUser gerente in listaDeGerentes)
                {
                    await _userManager.CreateAsync(gerente, "@Gerente1234");
                    await _userManager.AddToRoleAsync(gerente, identityRoleGerente.Name);
                }

                IdentityUser? usuarioAdministrador = new IdentityUser
                {
                    UserName = "admin@dev.com.br",
                    Email = "admin@dev.com.br",
                };

                IdentityRole? identityRoleAdministrador = new IdentityRole("Administrador");

                await _userManager.CreateAsync(usuarioAdministrador, "@Admin1234");
                await _userManager.AddToRoleAsync(usuarioAdministrador, identityRoleAdministrador.Name);

            }

        }
    }
}
