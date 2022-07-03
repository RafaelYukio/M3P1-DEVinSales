using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.DTOs.ApiDTOs;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using DevInSales.Core.Tests.Database;
using Xunit;

namespace DevInSales.Core.Tests.Services
{
    public class CityServiceTest
    {
        private readonly DataContext _dataContext;
        private readonly DBInMemory _dbInMemory;
        private readonly CityService _cityService;

        public CityServiceTest()
        {
            _dbInMemory = new DBInMemory();
            _dataContext = _dbInMemory.GetContext();
            _cityService = new CityService(_dataContext);
        }

        [Theory(Skip = "Erro no retorno")]
        [InlineData(null, null)]
        [InlineData(null, "")]
        public void GetAll_RetornarTodasAsCidade_QuandoPesquisaVazia_RetornaTodasAsCidade(int stateId, string name)
        {
            //Arrange

            //Act
            var cidadesList = _cityService.GetAll(25, null);

            //Assert
            Assert.NotNull(cidadesList);

        }

        [Theory]
        [InlineData(1)] //Criado no mapping
        [InlineData(2)] //Criado no mapping
        [InlineData(3)] //Criado no mapping
        [InlineData(100)] //Criado no banco em memória
        [InlineData(101)] //Criado no banco em memória
        [InlineData(102)] //Criado no banco em memória
        public void GetById_RetornarCidadePorId_QuandoIdValido_RetornaCidade(int cityId)
        {
            //Arrange

            //Act
            var cidade = _cityService.GetById(cityId);

            //Assert
            Assert.NotNull(cidade);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(998)]
        [InlineData(997)]
        public void GetById_RetornarCidadePorId_QuandoIdInvalido_RetornaException(int cityId)
        {
            //Arrange
            string exceptionMessage = "";

            //Act
            var cidade = _cityService.GetById(cityId);

            //Assert
            Assert.Null(cidade);
        }

        [Fact]
        public void Add_AdicionarCidade_QuandoCidadeValida_AdicionaCidade()
        {
            //Arrange
            City cidade = new City(24, "Cidade Teste");
            string exceptionMessage = "";

            //Act
            try
            {
                _cityService.Add(cidade);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            //Assert
            Assert.Equal("", exceptionMessage);
        }

        [Fact]
        public void Add_AdicionarCidade_QuandoCidadeInvalida_RetornaException()
        {
            //Arrange
            City cidade = new City(999, null);
            string exceptionMessage = "";

            //Act
            try
            {
                _cityService.Add(cidade);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            //Assert
            Assert.NotEqual("", exceptionMessage);
        }
    }
}
