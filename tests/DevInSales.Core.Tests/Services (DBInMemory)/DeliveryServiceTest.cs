using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.DTOs.ApiDTOs;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using DevInSales.Core.Tests.Database;
using FluentAssertions;
using Xunit;

namespace DevInSales.Core.Tests.Services
{
    public class DeliveryServiceTest
    {
        private readonly DataContext _dataContext;
        private readonly DBInMemory _dbInMemory;
        private readonly DeliveryService _deliveryService;

        public DeliveryServiceTest()
        {
            _dbInMemory = new DBInMemory();
            _dataContext = _dbInMemory.GetContext();
            _deliveryService = new DeliveryService(_dataContext);
        }

        [Theory(Skip = "Erro ao criar banco em memória")]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        public void GetBy_RetornarListaDeDeliveries_QuandoPesquisaValida_RetornarListaComTodosOsDeliveries(int idAddress, int saleId)
        {
            //Arrange

            //Act
            var deliveries = _deliveryService.GetBy(idAddress, saleId);

            //Assert
            deliveries.Should().HaveCount(4);
        }

    }
}
