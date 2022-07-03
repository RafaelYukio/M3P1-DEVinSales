using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.DTOs.ApiDTOs;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using DevInSales.Core.Services;
using DevInSales.Core.Tests.Database;
using FluentAssertions;
using Xunit;

namespace DevInSales.Core.Tests.Services
{
    public class StateServiceTest
    {
        private readonly DataContext _dataContext;
        private readonly DBInMemory _dbInMemory;
        private readonly StateService _stateService;

        public StateServiceTest()
        {
            _dbInMemory = new DBInMemory();
            _dataContext = _dbInMemory.GetContext();
            _stateService = new StateService(_dataContext);
        }

        [Fact]
        public void GetAll_RetornarStates_QuandoPesquisaVazia_RetornarTodosOsState()
        {
            //Arrange

            //Act
            var stateList = _stateService.GetAll(null);

            //Assert
            Assert.True(stateList.Any());
        }

        [Theory]
        [InlineData("aaaaa")]
        [InlineData("bbbbb")]
        [InlineData("ccccc")]
        [InlineData("ddddd")]
        [InlineData("eeeee")]
        public void GetAll_RetornarStates_QuandoNomeDeStateInvalido_RetornarListaVazia(string name)
        {
            //Arrange

            //Act
            var stateList = _stateService.GetAll(name);

            //Assert
            Assert.False(stateList.Any());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void GetById_RetornarStatePorIde_QuandoIdValido_RetornaState(int stateId)
        {
            //Arrange

            //Act
            var stateList = _stateService.GetById(stateId);

            //Assert
            Assert.NotNull(stateList);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(998)]
        [InlineData(997)]
        [InlineData(996)]
        [InlineData(995)]
        public void GetById_RetornarStatePorIde_QuandoIdInvalido_RetornaNull(int stateId)
        {
            //Arrange

            //Act
            var stateList = _stateService.GetById(stateId);

            //Assert
            Assert.Null(stateList);
        }
    }
}
