using DevInSales.Api.Controllers;
using DevInSales.Api.Dtos;
using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.DTOs.ApiDTOs;
using DevInSales.Core.Data.DTOs.IdentityDTOs;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using DevInSales.Core.Interfaces.Identity;
using DevInSales.Core.Services;
using DevInSales.Core.Tests.Database;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DevInSales.Core.Tests.Controllers
{
    public class IdentityUserControllerTest
    {
        private Mock<IIdentityService> _mockIdentityService;
        private IdentityUserController _identityUserController;

        public IdentityUserControllerTest()
        {
            _mockIdentityService = new Mock<IIdentityService>();
            _identityUserController = new IdentityUserController(_mockIdentityService.Object);
        }

        [Fact]
        public async void CriarUsuario_CriarUsuario_QuandoUsuarioValido_RetornaOk()
        {
            //Arrange
            _mockIdentityService.Setup(m => m.CadastrarUsuario(It.IsAny<CadastroRequest>()));

            //Act
            var actionResult = await _identityUserController.CriarUsuario(It.IsAny<CadastroRequest>());

            //Assert
            Assert.IsType<OkResult>(actionResult);
            _mockIdentityService.Verify(m => m.CadastrarUsuario(It.IsAny<CadastroRequest>()), Times.Once);
        }

        [Fact]
        public async void CriarUsuario_CriarUsuario_QuandoModelInvalido_RetornaBadRequest()
        {
            //Arrange
            _identityUserController.ModelState.AddModelError("Email", "Email obrigatório");
            _mockIdentityService.Setup(m => m.CadastrarUsuario(It.IsAny<CadastroRequest>()));

            //Act
            
            var actionResult = await _identityUserController.CriarUsuario(It.IsAny<CadastroRequest>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
            _mockIdentityService.Verify(m => m.CadastrarUsuario(It.IsAny<CadastroRequest>()), Times.Never);
        }

        [Fact]
        public async void CriarUsuario_CriarUsuario_QuandoErroAoSalvar_RetornaBadRequest()
        {
            //Arrange
            _mockIdentityService.Setup(m => m.CadastrarUsuario(It.IsAny<CadastroRequest>())).Throws<Exception>();

            //Act

            var actionResult = await _identityUserController.CriarUsuario(It.IsAny<CadastroRequest>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
            _mockIdentityService.Verify(m => m.CadastrarUsuario(It.IsAny<CadastroRequest>()), Times.Once);
        }

        //Testes de cria gerente iguais ao de criar usuário

        [Fact]
        public async void ObterUserPorId_ObterUsuarioPeloId_QuandoIdValido_RetornaUser()
        {
            //Arrange
            string userId = "1";
            User userMock = new User(userId, "teste", "teste", "teste", DateTime.Now);
            _mockIdentityService.Setup(m => m.ObterUsuarioPorId(It.IsAny<string>())).ReturnsAsync(userMock);

            //Act
            var actionResult = await _identityUserController.ObterUserPorId(userId);

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
            _mockIdentityService.Verify(m => m.ObterUsuarioPorId(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void ObterUserPorId_ObterUsuarioPeloId_QuandoIdInvalido_RetornaUser()
        {
            //Arrange
            string userId = "1";
            _mockIdentityService.Setup(m => m.ObterUsuarioPorId(It.IsAny<string>())).Throws<Exception>();

            //Act
            var actionResult = await _identityUserController.ObterUserPorId(userId);

            //Assert
            Assert.IsType<NotFoundResult>(actionResult);
            _mockIdentityService.Verify(m => m.ObterUsuarioPorId(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void RetornarIdPorEmail_RetornarIdPorEmail_QuandoIdValido_RetornarOkId()
        {
            //Arrange
            string userId = "1";
            _mockIdentityService.Setup(m => m.RetornarIdPorEmail(It.IsAny<string>())).ReturnsAsync(userId);

            //Act
            var actionResult = await _identityUserController.RetornarIdPorEmail(userId);

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
            var okResult = actionResult as OkObjectResult;
            Assert.Equal(userId, okResult.Value);
            _mockIdentityService.Verify(m => m.RetornarIdPorEmail(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void RetornarIdPorEmail_RetornarIdPorEmail_QuandoIdInvalido_RetornaBadRequest()
        {
            //Arrange
            _mockIdentityService.Setup(m => m.RetornarIdPorEmail(It.IsAny<string>())).Throws<Exception>();

            //Act
            var actionResult = await _identityUserController.RetornarIdPorEmail(It.IsAny<string>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
            _mockIdentityService.Verify(m => m.RetornarIdPorEmail(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void ObterUsers_RetornarUsersList_QuandoUsersEncontrados_RetornaUsersList()
        {
            //Arrange
            List<User> usersListMock = new List<User>()
            {
                new User("Teste", "Teste", "Teste", DateTime.Now)
            };
            _mockIdentityService.Setup(m => m.ObterUsuarios()).Returns(usersListMock);

            //Act
            var actionResult = _identityUserController.ObterUsers();

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
            _mockIdentityService.Verify(m => m.ObterUsuarios(), Times.Once);
        }

        [Fact]
        public async void ObterUsers_RetornarUsersList_QuandoUsersListComZeroElementos_RetornaUsersList()
        {
            //Arrange
            List<User> usersListMock = new List<User>();
            _mockIdentityService.Setup(m => m.ObterUsuarios()).Returns(usersListMock);

            //Act
            var actionResult = _identityUserController.ObterUsers();

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockIdentityService.Verify(m => m.ObterUsuarios(), Times.Once);
        }

        [Fact]
        public async void ExcluirUser_ExcluirUser_QuandoIdValido_ExcluiUser()
        {
            //Arrange
            string userId = "1";
            _mockIdentityService.Setup(m => m.ExcluirUsuarioPorId(userId));

            //Act
            var actionResult = await _identityUserController.ExcluirUser(userId);

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockIdentityService.Verify(m => m.ExcluirUsuarioPorId(userId), Times.Once);
        }

        [Fact]
        public async void ExcluirUser_ExcluirUser_QuandoIdInvalido_ExcluiUser()
        {
            //Arrange
            string userId = "1";
            _mockIdentityService.Setup(m => m.ExcluirUsuarioPorId(userId)).Throws<Exception>();

            //Act
            var actionResult = await _identityUserController.ExcluirUser(userId);

            //Assert
            Assert.IsType<NotFoundObjectResult>(actionResult);
            _mockIdentityService.Verify(m => m.ExcluirUsuarioPorId(userId), Times.Once);
        }

        [Fact]
        public async void Login_RealizarLogin_QuandoLoginValido_RealizaLogin()
        {
            //Arrange
            LoginResponse loginResponseMock = new LoginResponse("Teste");
            _mockIdentityService.Setup(m => m.Login(It.IsAny<LoginRequest>())).ReturnsAsync(loginResponseMock);
            
            //Act
            var actionResult = await _identityUserController.Login(It.IsAny<LoginRequest>());

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
            var okResult = actionResult as OkObjectResult;
            Assert.Equal(loginResponseMock, okResult.Value);
            _mockIdentityService.Verify(m => m.Login(It.IsAny<LoginRequest>()), Times.Once);
        }

        [Fact]
        public async void Login_RealizarLogin_QuandoModelInvalido_RetornaBadRequest()
        {
            //Arrange
            _identityUserController.ModelState.AddModelError("Email", "Email obrigatório");
            _mockIdentityService.Setup(m => m.Login(It.IsAny<LoginRequest>()));

            //Act
            var actionResult = await _identityUserController.Login(It.IsAny<LoginRequest>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
            _mockIdentityService.Verify(m => m.Login(It.IsAny<LoginRequest>()), Times.Never);
        }

        [Fact]
        public async void Login_RealizarLogin_QuandoErroAoLogar_RetornaBadRequest()
        {
            //Arrange
            _mockIdentityService.Setup(m => m.Login(It.IsAny<LoginRequest>())).Throws<Exception>();

            //Act
            var actionResult = await _identityUserController.Login(It.IsAny<LoginRequest>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
            _mockIdentityService.Verify(m => m.Login(It.IsAny<LoginRequest>()), Times.Once);
        }

    }

}
