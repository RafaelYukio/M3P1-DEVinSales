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
    public class CityControllerTest
    {
        private Mock<IStateService> _mockStateService;
        private Mock<ICityService> _mockCityService;
        private CityController _cityController;

        public CityControllerTest()
        {
            _mockStateService = new Mock<IStateService>();
            _mockCityService = new Mock<ICityService>();
            _cityController = new CityController(_mockStateService.Object,
                                                 _mockCityService.Object);
        }

        [Fact]
        public void GetCityByStateId_RetornaCityListPeloStateId_QuandoIdValido_RetornaOk()
        {
            //Arrange
            int stateId = 1;
            string name = "Teste";
            ReadState readStateMock = new ReadState();
            List<ReadCity> readCityListMock = new List<ReadCity>()
            {
                new ReadCity()
            };
            _mockStateService.Setup(m => m.GetById(stateId)).Returns(readStateMock);
            _mockCityService.Setup(m => m.GetAll(stateId, name)).Returns(readCityListMock);

            //Act
            var actionResult = _cityController.GetCityByStateId(stateId, name);

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
            var okResult = actionResult as OkObjectResult;
            Assert.Equal(readCityListMock, okResult.Value);
            _mockStateService.Verify(m => m.GetById(stateId), Times.Once);
            _mockCityService.Verify(m => m.GetAll(stateId, name), Times.Once);
        }

        [Fact]
        public void GetCityByStateId_RetornaCityListPeloStateId_QuandoStateNull_RetornaNotFound()
        {
            //Arrange
            int stateId = 1;
            string name = "Teste";
            ReadState readStateMock = new ReadState();
            _mockStateService.Setup(m => m.GetById(stateId)).Returns((ReadState)null);

            //Act
            var actionResult = _cityController.GetCityByStateId(stateId, name);

            //Assert
            Assert.IsType<NotFoundResult>(actionResult);
            _mockStateService.Verify(m => m.GetById(stateId), Times.Once);
            _mockCityService.Verify(m => m.GetAll(stateId, name), Times.Never);
        }

        [Fact]
        public void GetCityByStateId_RetornaCityListPeloStateId_QuandoCitiesListNull_RetornaNoContent()
        {
            //Arrange
            int stateId = 1;
            string name = "Teste";
            ReadState readStateMock = new ReadState();
            _mockStateService.Setup(m => m.GetById(stateId)).Returns(readStateMock);
            _mockCityService.Setup(m => m.GetAll(stateId, name)).Returns((List<ReadCity>)null);

            //Act
            var actionResult = _cityController.GetCityByStateId(stateId, name);

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockStateService.Verify(m => m.GetById(stateId), Times.Once);
            _mockCityService.Verify(m => m.GetAll(stateId, name), Times.Once);
        }

        [Fact]
        public void GetCityByStateId_RetornaCityListPeloStateId_QuandoCitiesListComZeroElementos_RetornaNoContent()
        {
            //Arrange
            int stateId = 1;
            string name = "Teste";
            ReadState readStateMock = new ReadState();
            _mockStateService.Setup(m => m.GetById(stateId)).Returns(readStateMock);
            _mockCityService.Setup(m => m.GetAll(stateId, name)).Returns(new List<ReadCity>());

            //Act
            var actionResult = _cityController.GetCityByStateId(stateId, name);

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockStateService.Verify(m => m.GetById(stateId), Times.Once);
            _mockCityService.Verify(m => m.GetAll(stateId, name), Times.Once);
        }

        [Fact]
        public void GetCityById_RetornarCityPorId_QuandoIdValido_RetornaOk()
        {
            //Arrange
            int stateId = 1;
            int cityId = 1;
            ReadState readStateMock = new ReadState()
            {
                Id = stateId
            };
            ReadCity readCityMock = new ReadCity()
            {
                State = new ReadCityState()
                {
                    Id = stateId
                }
            };
            _mockStateService.Setup(m => m.GetById(stateId)).Returns(readStateMock);
            _mockCityService.Setup(m => m.GetById(cityId)).Returns(readCityMock);

            //Act
            var actionResult = _cityController.GetCityById(stateId, cityId);

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
            _mockStateService.Verify(m => m.GetById(stateId), Times.Once);
            _mockCityService.Verify(m => m.GetById(cityId), Times.Once);
        }

        [Fact]
        public void GetCityById_RetornarCityPorId_QuandoStateNull_RetornaNotFound()
        {
            //Arrange
            int stateId = 1;
            int cityId = 1;
            ReadCity readCityMock = new ReadCity()
            {
                State = new ReadCityState()
                {
                    Id = stateId
                }
            };
            _mockStateService.Setup(m => m.GetById(stateId)).Returns((ReadState)null);
            _mockCityService.Setup(m => m.GetById(cityId)).Returns(readCityMock);

            //Act
            var actionResult = _cityController.GetCityById(stateId, cityId);

            //Assert
            Assert.IsType<NotFoundResult>(actionResult);
            _mockStateService.Verify(m => m.GetById(stateId), Times.Once);
            _mockCityService.Verify(m => m.GetById(cityId), Times.Never);
        }

        [Fact]
        public void GetCityById_RetornarCityPorId_QuandoCityNull_RetornaNotFound()
        {
            //Arrange
            int stateId = 1;
            int cityId = 1;
            ReadState readStateMock = new ReadState()
            {
                Id = stateId
            };
            _mockStateService.Setup(m => m.GetById(stateId)).Returns(readStateMock);
            _mockCityService.Setup(m => m.GetById(cityId)).Returns((ReadCity)null);

            //Act
            var actionResult = _cityController.GetCityById(stateId, cityId);

            //Assert
            Assert.IsType<NotFoundResult>(actionResult);
            _mockStateService.Verify(m => m.GetById(stateId), Times.Once);
            _mockCityService.Verify(m => m.GetById(cityId), Times.Once);
        }

        [Fact]
        public void GetCityById_RetornarCityPorId_QuandoStateIdDiferentedoCity_RetornaBadRequest()
        {
            //Arrange
            int stateId = 1;
            int cityId = 1;
            ReadState readStateMock = new ReadState()
            {
                Id = stateId
            };
            ReadCity readCityMock = new ReadCity()
            {
                State = new ReadCityState()
                {
                    Id = 2
                }
            };
            _mockStateService.Setup(m => m.GetById(stateId)).Returns(readStateMock);
            _mockCityService.Setup(m => m.GetById(cityId)).Returns(readCityMock);

            //Act
            var actionResult = _cityController.GetCityById(stateId, cityId);

            //Assert
            Assert.IsType<BadRequestResult>(actionResult);
            _mockStateService.Verify(m => m.GetById(stateId), Times.Once);
            _mockCityService.Verify(m => m.GetById(cityId), Times.Once);
        }

        [Fact]
        public void AddCity_AdicionarCity_QuandoCityValida_RetornaCreatedAtAction()
        {
            //Arrange
            int stateId = 1;
            AddCity addCityMock = new AddCity("Teste");
            ReadState readStateMock = new ReadState();
            _mockStateService.Setup(m => m.GetById(It.IsAny<int>())).Returns(readStateMock);
            _mockCityService.Setup(m => m.GetAll(It.IsAny<int>(), It.IsAny<string>())).Returns((List<ReadCity>)null);
            _mockCityService.Setup(m => m.Add(It.IsAny<City>()));

            //Act
            var actionResult = _cityController.AddCity(stateId, addCityMock);

            //Assert
            Assert.IsType<CreatedAtActionResult>(actionResult);
            _mockStateService.Verify(m => m.GetById(It.IsAny<int>()), Times.Once);
            _mockCityService.Verify(m => m.GetAll(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            _mockCityService.Verify(m => m.Add(It.IsAny<City>()), Times.Once);
        }
    }
}
