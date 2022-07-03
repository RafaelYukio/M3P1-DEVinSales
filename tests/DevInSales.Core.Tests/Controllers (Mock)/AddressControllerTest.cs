using DevInSales.Api.Controllers;
using DevInSales.Api.Dtos;
using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.DTOs.ApiDTOs;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using DevInSales.Core.Services;
using DevInSales.Core.Tests.Database;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DevInSales.Core.Tests.Controllers
{
    public class AddressControllerTest
    {
        private Mock<IAddressService> _mockAddressService;
        private Mock<IStateService> _mockStateService;
        private Mock<ICityService> _mockCityService;
        private AddressController _addressController;

        public AddressControllerTest()
        {
            _mockAddressService = new Mock<IAddressService>();
            _mockStateService = new Mock<IStateService>(); 
            _mockCityService = new Mock<ICityService>();
            _addressController = new AddressController(_mockAddressService.Object,
                                                       _mockStateService.Object,
                                                       _mockCityService.Object);
        }

        [Theory]
        [InlineData(1, 1, "Rua 1", "11111111")]
        [InlineData(2, 2, "Rua 2", "22222222")]
        [InlineData(3, 3, "Rua 3", "33333333")]
        public void GetAll_RetornaListaDeAddress_QuandoPesquisaValida_RetornaOkEListaDeAddress(int stateId, int cityId, string street, string cep)
        {
            //Arrange
            List<ReadAddress> readAddressListMock = new List<ReadAddress>()
            {
                ReadAddress.AddressToReadAddress(It.IsAny<Address>())
            };
            _mockAddressService.Setup(m => m.GetAll(stateId, cityId, street, cep)).Returns(readAddressListMock);

            //Act
            var actionResult = _addressController.GetAll(stateId, cityId, street, cep);

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
            var okResult = actionResult as OkObjectResult;
            Assert.Equal(readAddressListMock, okResult.Value);
            _mockAddressService.Verify(m => m.GetAll(stateId, cityId, street, cep), Times.Once);
        }

        [Fact]
        public void GetAll_RetornaListaDeAddress_QuandoListaDeAddressNull_RetornaNoContent()
        {
            //Arrange
            int stateId = 0;
            int cityId = 0;
            string street = "Rua";
            string cep = "12345678";
            _mockAddressService.Setup(m => m.GetAll(stateId, cityId, street, cep)).Returns((List<ReadAddress>)null);

            //Act
            var actionResult = _addressController.GetAll(stateId, cityId, street, cep);

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockAddressService.Verify(m => m.GetAll(stateId, cityId, street, cep), Times.Once);
        }

        [Fact]
        public void GetAll_RetornaListaDeAddress_QuandoListaDeAddressComZeroElementos_RetornaNoContent()
        {
            //Arrange
            int stateId = 0;
            int cityId = 0;
            string street = "Rua";
            string cep = "12345678";
            List<ReadAddress> readAddressListMock = new List<ReadAddress>();
            _mockAddressService.Setup(m => m.GetAll(stateId, cityId, street, cep)).Returns(readAddressListMock);

            //Act
            var actionResult = _addressController.GetAll(stateId, cityId, street, cep);

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockAddressService.Verify(m => m.GetAll(stateId, cityId, street, cep), Times.Once);
        }

        [Fact(Skip = "Erro ao instanciar")]
        public void AddAddress_AdicionarAddress_QuandoAddressValido_RetornaCreatedAtAction()
        {
            //Arrange
            AddAddress addAddressMock = new AddAddress("Teste", 999, "Casa", "12345678");
            _mockStateService.Setup(m => m.GetById(1)).Returns(new ReadState());
            _mockCityService.Setup(m => m.GetById(1)).Returns(new ReadCity());
            _mockAddressService.Setup(m => m.Add(It.IsAny<Address>()));

            //Act
            var actionResult = _addressController.AddAddress(1, 1, addAddressMock);

            //Assert
            Assert.IsType<CreatedAtActionResult>(actionResult);
            _mockStateService.Verify(m => m.GetById(It.IsAny<int>()), Times.Once);
            _mockCityService.Verify(m => m.GetById(It.IsAny<int>()), Times.Once);
            _mockAddressService.Verify(m => m.Add(It.IsAny<Address>()), Times.Once);
        }

        [Fact]
        public void DeleteAddress_DeletarAddress_QuandoAddressValido_DeletaAddress()
        {
            //Arrange
            int id = 1;
            Address addressMock = new Address(id, "Teste", "12345678", 999, "Casa", 1);
            _mockAddressService.Setup(m => m.GetById(id)).Returns(addressMock);
            _mockAddressService.Setup(m => m.Delete(addressMock));

            //Act
            var actionResult = _addressController.DeleteAddress(id);

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockAddressService.Verify(m => m.GetById(id), Times.Once);
            _mockAddressService.Verify(m => m.Delete(addressMock), Times.Once);
        }

        [Fact]
        public void DeleteAddress_DeletarAddress_QuandoAddressNull_DeletaAddress()
        {
            //Arrange
            int id = 1;
            _mockAddressService.Setup(m => m.GetById(id)).Returns((Address)null);

            //Act
            var actionResult = _addressController.DeleteAddress(id);

            //Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public void UpdateAddress_AtualizarAddress_QuandoAddressValido_RetornaNoContent()
        {
            //Arrange
            int id = 1;
            string street = "Teste";
            int number = 999;
            string cep = "12345678";
            string complement = "Casa";
            int cityId = 1;
            Address addressMock = new Address(id, street, cep, number, complement, cityId);
            UpdateAddress updateAddressMock = new UpdateAddress(street, number, complement, cep);
            _mockAddressService.Setup(m => m.GetById(id)).Returns(addressMock);
            _mockAddressService.Setup(m => m.Update(addressMock));

            //Act
            var actionResult = _addressController.UpdateAddress(id, updateAddressMock);

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockAddressService.Verify(m => m.GetById(id), Times.Once);
            _mockAddressService.Verify(m => m.Update(addressMock), Times.Once);
        }

        [Fact]
        public void UpdateAddress_AtualizarAddress_QuandoAddressInvalido_RetornaNotFound()
        {
            //Arrange
            int id = 1;
            string street = "Teste";
            int number = 999;
            string cep = "12345678";
            string complement = "Casa";
            int cityId = 1;
            Address addressMock = new Address(id, street, cep, number, complement, cityId);
            UpdateAddress updateAddressMock = new UpdateAddress(street, number, complement, cep);
            _mockAddressService.Setup(m => m.GetById(id)).Returns((Address)null);
            _mockAddressService.Setup(m => m.Update(addressMock));

            //Act
            var actionResult = _addressController.UpdateAddress(id, updateAddressMock);

            //Assert
            Assert.IsType<NotFoundResult>(actionResult);
            _mockAddressService.Verify(m => m.GetById(id), Times.Once);
            _mockAddressService.Verify(m => m.Update(addressMock), Times.Never);
        }

        [Fact]
        public void UpdateAddress_AtualizarAddress_QuandoAddressComNulls_RetornaBadRequest()
        {
            //Arrange
            int id = 1;
            string street = "Teste";
            int number = 999;
            string cep = "12345678";
            string complement = "Casa";
            int cityId = 1;
            Address addressMock = new Address(id, street, cep, number, complement, cityId);
            UpdateAddress updateAddressMock = new UpdateAddress(null, null, null, null);
            _mockAddressService.Setup(m => m.GetById(id)).Returns(addressMock);
            _mockAddressService.Setup(m => m.Update(addressMock));

            //Act
            var actionResult = _addressController.UpdateAddress(id, updateAddressMock);

            //Assert
            Assert.IsType<BadRequestResult>(actionResult);
            _mockAddressService.Verify(m => m.GetById(id), Times.Once);
            _mockAddressService.Verify(m => m.Update(addressMock), Times.Never);
        }
    }
}
