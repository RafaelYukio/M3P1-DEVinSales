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
    public class SaleServiceTest
    {
        private readonly DataContext _dataContext;
        private readonly DBInMemory _dbInMemory;
        private readonly SaleService _saleService;

        public SaleServiceTest()
        {
            _dbInMemory = new DBInMemory();
            _dataContext = _dbInMemory.GetContext();
            _saleService = new SaleService(_dataContext);
        }

        [Theory]
        [InlineData("1", "2")]
        [InlineData("3", "4")]
        [InlineData("5", "6")]
        [InlineData("7", "8")]
        [InlineData("9", "10")]
        public void CreateSaleByUserId_CriarSalePeloId_QuandoSaleValido_RetornaSaleId(string buyerId, string sellerId)
        {
            //Arrange
            Sale sale = new Sale(buyerId, sellerId, DateTime.Now);

            //Act
            int? saleId = _saleService.CreateSaleByUserId(sale);

            //Assert
            Assert.NotNull(saleId);
        }

        [Theory]
        [InlineData("0", "0")]
        [InlineData("1", "0")]
        [InlineData("0", "1")]
        public void CreateSaleByUserId_CriarSalePeloId_QuandoBuyerIdOuSaleIdZero_RetornaException(string buyerId, string sellerId)
        {
            //Arrange
            Sale sale = new Sale(buyerId, sellerId, DateTime.Now);

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => _saleService.CreateSaleByUserId(sale));

            //Assert
            Assert.Equal("Id não pode ser nulo nem zero.", exception.ParamName);
        }

        [Theory]
        [InlineData(1)] //Criado no banco em memória
        [InlineData(2)] //Criado no banco em memória
        [InlineData(3)] //Criado no banco em memória
        [InlineData(4)] //Criado no banco em memória
        [InlineData(5)] //Criado no banco em memória
        public void GetSaleById_RetornarSaleResponsePorId_QuandoIdValido_RetornaSale(int id)
        {
            //Arrange

            //Act
            var saleResponse = _saleService.GetSaleById(id);

            //Assert
            Assert.NotNull(saleResponse);
        }

        [Theory]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void GetSaleById_RetornarSaleResponsePorId_QuandoIdInvalido_RetornaNull(int id)
        {
            //Arrange

            //Act
            var saleResponse = _saleService.GetSaleById(id);

            //Assert
            Assert.Null(saleResponse);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void GetSaleProductsBySaleId_RetornarSaleProductsPeloSaleId_QuandoSaleIdValido_RetornaListaDeSaleProducts(int id)
        {
            //Arrange

            //Act
            var saleProductResponseList = _saleService.GetSaleProductsBySaleId(id);

            //Assert
            Assert.NotNull(saleProductResponseList);
        }

        [Theory]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void GetSaleProductsBySaleId_RetornarSaleProductsPeloSaleId_QuandoSaleIdInvalido_RetornaListaVaziaDeSaleProducts(int id)
        {
            //Arrange

            //Act
            var saleProductResponseList = _saleService.GetSaleProductsBySaleId(id);

            //Assert
            Assert.False(saleProductResponseList.Any());
        }

        [Theory]
        [InlineData("100")]
        [InlineData("101")]
        [InlineData("102")]
        [InlineData("103")]
        [InlineData("104")]
        public void GetSaleByBuyerId_RetornarSalePeloBuyerId_QuandoBuyerIdValido_RetornaSaleList(string id)
        {
            //Arrange (teste pegando dados direto do DBInMemory)
            List<Sale> saleDBList = _dataContext.Sales.Where(p => p.BuyerId == id).ToList();

            //Act
            var saleList = _saleService.GetSaleByBuyerId(id);

            //Assert
            Assert.Equal(saleDBList, saleList);
        }

        [Theory]
        [InlineData("999")]
        [InlineData("998")]
        [InlineData("997")]
        [InlineData("996")]
        [InlineData("995")]
        public void GetSaleByBuyerId_RetornarSalePeloBuyerId_QuandoBuyerIdInvalido_RetornaSale(string id)
        {
            //Arrange

            //Act
            var saleList = _saleService.GetSaleByBuyerId(id);

            //Assert
            Assert.False(saleList.Any());
        }

        [Theory]
        [InlineData(1, 100, 11)]
        [InlineData(2, 101, 22)]
        [InlineData(3, 102, 33)]
        [InlineData(4, 103, 44)]
        [InlineData(5, 104, 55)]
        public void UpdateAmount_AtualizarQuantidadeDoProdutoNaSale_QuandoSaleIdProductIdAmountValido_AtualizaValores(int saleId, int productId, int amount)
        {
            //Arrange
            string exceptionMessage = "";

            //Act
            try
            {
                _saleService.UpdateAmount(saleId, productId, amount);
            } catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            //Assert
            Assert.Equal("", exceptionMessage);
        }

        [Theory]
        [InlineData(6, 100, 11)]
        [InlineData(7, 101, 22)]
        [InlineData(8, 102, 33)]
        [InlineData(9, 103, 44)]
        [InlineData(10, 104, 55)]
        public void UpdateAmount_AtualizarQuantidadeDoProdutoNaSale_QuandoSaleIdInvalido_RetornaException(int saleId, int productId, int amount)
        {
            //Arrange
            var expectedException = new System.ArgumentException("Não existe venda com esse Id.", "saleId");

            //Act
            var exception = Assert.Throws<ArgumentException>(() => _saleService.UpdateAmount(saleId, productId, amount));

            //Assert
            Assert.Equal(expectedException.Message, exception.Message);
            Assert.Equal(expectedException.ParamName, exception.ParamName);
        }

        [Theory]
        [InlineData(1, 999, 11)]
        [InlineData(2, 998, 22)]
        [InlineData(3, 997, 33)]
        [InlineData(4, 996, 44)]
        [InlineData(5, 995, 55)]
        public void UpdateAmount_AtualizarQuantidadeDoProdutoNaSale_QuandoProductIdInvalido_RetornaException(int saleId, int productId, int amount)
        {
            //Arrange
            var expectedException = new System.ArgumentException("Não existe este produto nesta venda.", "productId");

            //Act
            var exception = Assert.Throws<ArgumentException>(() => _saleService.UpdateAmount(saleId, productId, amount));

            //Assert
            Assert.Equal(expectedException.Message, exception.Message);
            Assert.Equal(expectedException.ParamName, exception.ParamName);
        }

        [Theory]
        [InlineData(1, 101, 0)]
        [InlineData(2, 102, -99)]
        public void UpdateAmount_AtualizarQuantidadeDoProdutoNaSale_QuandoQuantidadeMenorOuIgualZero_RetornaException(int saleId, int productId, int amount)
        {
            //Arrange
            var expectedException = new System.ArgumentException("Quantidade não pode ser menor ou igual a zero.", "amount");

            //Act
            var exception = Assert.Throws<ArgumentException>(() => _saleService.UpdateAmount(saleId, productId, amount));

            //Assert
            Assert.Equal(expectedException.Message, exception.Message);
            Assert.Equal(expectedException.ParamName, exception.ParamName);
        }

        [Theory]
        [InlineData(100, 1)]
        [InlineData(101, 2)]
        [InlineData(102, 3)]
        [InlineData(103, 4)]
        [InlineData(104, 5)]
        public void CreateDeliveryForASale_CriaDeliveryParaUmaSale_QuandoDeliveryValido_RetornaDeliveryId(int addressId, int saleId)
        {
            //Arrange
            Delivery delivery = new Delivery(addressId, saleId, new DateTime(2023, 01, 01));
            string exceptionMessage = "";

            //Act
            try
            {
                _saleService.CreateDeliveryForASale(delivery);
            } catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            //Assert
            Assert.Equal("", exceptionMessage);
        }

        [Theory]
        [InlineData(100, 6)]
        [InlineData(101, 7)]
        [InlineData(102, 8)]
        [InlineData(103, 9)]
        [InlineData(104, 10)]
        public void CreateDeliveryForASale_CriaDeliveryParaUmaSale_QuandoSaleIdInvalido_RetornaException(int addressId, int saleId)
        {
            //Arrange
            Delivery delivery = new Delivery(addressId, saleId, new DateTime(2023, 01, 01));
            var expectedException = new System.ArgumentException("Não existe venda com esse Id.", "saleId");

            //Act
            var exception = Assert.Throws<ArgumentException>(() => _saleService.CreateDeliveryForASale(delivery));

            //Assert
            Assert.Equal(expectedException.Message, exception.Message);
            Assert.Equal(expectedException.ParamName, exception.ParamName);
        }

        [Theory]
        [InlineData(999, 1)]
        [InlineData(998, 2)]
        [InlineData(997, 3)]
        [InlineData(996, 4)]
        [InlineData(995, 5)]
        public void CreateDeliveryForASale_CriaDeliveryParaUmaSale_QuandoAddressIdInvalido_RetornaException(int addressId, int saleId)
        {
            //Arrange
            Delivery delivery = new Delivery(addressId, saleId, new DateTime(2023, 01, 01));
            var expectedException = new System.ArgumentException("Não existe endereço com esse Id.", "AddressId");

            //Act
            var exception = Assert.Throws<ArgumentException>(() => _saleService.CreateDeliveryForASale(delivery));

            //Assert
            Assert.Equal(expectedException.Message, exception.Message);
            Assert.Equal(expectedException.ParamName, exception.ParamName);
        }

        [Theory]
        [InlineData(1)] //Criado no banco em memória
        [InlineData(2)] //Criado no banco em memória
        [InlineData(3)] //Criado no banco em memória
        [InlineData(4)] //Criado no banco em memória
        [InlineData(5)] //Criado no banco em memória
        public void GetDeliveryById_RetornarDeiveryPeloId_QuandoDeliveryIdValido_RetornaDelivery(int deliveryId)
        {
            //Arrange

            //Act
            var delivery = _saleService.GetDeliveryById(deliveryId);

            //Assert
            Assert.NotNull(delivery);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(998)]
        [InlineData(997)]
        [InlineData(996)]
        [InlineData(995)]
        public void GetDeliveryById_RetornarDeiveryPeloId_QuandoDeliveryIdInvalido_RetornaDelivery(int deliveryId)
        {
            //Arrange

            //Act
            var delivery = _saleService.GetDeliveryById(deliveryId);

            //Assert
            Assert.Null(delivery);
        }
    }
}
