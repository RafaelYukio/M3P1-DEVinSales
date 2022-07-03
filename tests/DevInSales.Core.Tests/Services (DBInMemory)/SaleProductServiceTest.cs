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
    public class SaleProductServiceTest
    {
        private readonly DataContext _dataContext;
        private readonly DBInMemory _dbInMemory;
        private readonly SaleProductService _saleProductService;

        public SaleProductServiceTest()
        {
            _dbInMemory = new DBInMemory();
            _dataContext = _dbInMemory.GetContext();
            _saleProductService = new SaleProductService(_dataContext);
        }

        [Theory]
        [InlineData(1)] //Criado no banco em memória
        [InlineData(2)] //Criado no banco em memória
        [InlineData(3)] //Criado no banco em memória
        [InlineData(4)] //Criado no banco em memória
        [InlineData(5)] //Criado no banco em memória
        public void CreateSaleProduct_CriarSaleProduct_QuandoSaleProductValido_CriaSaleProduct(int saleId)
        {
            //Arrange
            SaleProductRequest saleProductRequest = new SaleProductRequest(10, 999, 99);
            string exceptionMessage = "";

            //Act
            try
            {
                var saleProductId = _saleProductService.CreateSaleProduct(saleId, saleProductRequest);
            } catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            //Assert
            Assert.Equal("", exceptionMessage);
        }

        [Theory]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void CreateSaleProduct_CriarSaleProduct_QuandoIdDaSaleInvalido_RetornaException(int saleId)
        {
            //Arrange
            SaleProductRequest saleProductRequest = new SaleProductRequest(10, 999, 99);

            //Act
            var exception = Assert.Throws<ArgumentException>(() => _saleProductService.CreateSaleProduct(saleId, saleProductRequest));

            //Assert
            Assert.Equal("ProductId ou SaleId não encontrado.", exception.Message);
        }

        [Theory]
        [InlineData(1, 0, 0)]
        [InlineData(2, -99, 0)]
        [InlineData(3, 0, -99)]
        [InlineData(4, -99, -99)]
        public void CreateSaleProduct_CriarSaleProduct_QuandoUnitPriceOuAmountMenorIgualAZero_RetornaException(int saleId, int unitPrice, int amount)
        {
            //Arrange
            SaleProductRequest saleProductRequest = new SaleProductRequest(10, unitPrice, amount);

            //Act
            var exception = Assert.Throws<ArgumentException>(() => _saleProductService.CreateSaleProduct(saleId, saleProductRequest));

            //Assert
            Assert.Equal("Preço ou quantidade não podem ser negativos.", exception.Message);
        }

        [Theory]
        [InlineData(100)] //Criado no banco em memória
        [InlineData(101)] //Criado no banco em memória
        [InlineData(102)] //Criado no banco em memória
        [InlineData(103)] //Criado no banco em memória
        [InlineData(104)] //Criado no banco em memória
        public void GetSaleProductById_RetornarIdDoSaleProduct_QuandoIdValido_RetornaSaleProduct(int saleProductId)
        {
            //Arrange

            //Act
            var saleProductIdValido = _saleProductService.GetSaleProductById(saleProductId);

            //Assert
            Assert.NotNull(saleProductIdValido);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(998)]
        [InlineData(997)]
        [InlineData(996)]
        [InlineData(995)]
        public void GetSaleProductById_RetornarIdDoSaleProduct_QuandoIdInvalido_RetornaZero(int saleProductId)
        {
            //Arrange

            //Act
            var saleProductIdValido = _saleProductService.GetSaleProductById(saleProductId);

            //Assert
            Assert.Equal(0, saleProductIdValido);
        }
    }
}
