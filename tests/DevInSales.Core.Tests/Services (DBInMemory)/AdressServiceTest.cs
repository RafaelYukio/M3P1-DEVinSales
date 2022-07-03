using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.DTOs.ApiDTOs;
using DevInSales.Core.Entities;
using DevInSales.Core.Services;
using DevInSales.Core.Tests.Database;
using FluentAssertions;
using Xunit;
 
namespace DevInSales.Core.Tests.Services
{
    public class AdressServiceTest
    {
        private readonly DataContext _dataContext;
        private readonly DBInMemory _dbInMemory;
        private readonly AddressService _addressService;

        public AdressServiceTest()
        {
            _dbInMemory = new DBInMemory();
            _dataContext = _dbInMemory.GetContext();
            _addressService = new AddressService(_dataContext);
        }

        [Fact]
        public void GetAll_RetornarListaDeAddresses_QuandoPesquisaVazia_RetornaTodosAdresses()
        {
            //Arrage

            //Act
            var addressesList = _addressService.GetAll(null, null, null, null);

            //Assert (5x Addresses do mapping + 5x criados no banco em memória)
            addressesList.Should().HaveCount(10);
        }

        [Fact]
        public void GetAll_RetornarListaDeAddresses_QuandoPesquisaApenasIdDoEstado_RetornaAdressesComIdDoEstado()
        {
            //Arrage

            //Act
            var addressesList = _addressService.GetAll(25, null, null, null);

            //Assert (2x Addresses do mapping + 5x criados no banco em memória)
            addressesList.Should().HaveCount(7);
        }

        [Fact]
        public void GetAll_RetornarListaDeAddresses_QuandoPesquisaApenasIdDaCidade_RetornaAdressesComIdDaCidade()
        {
            //Arrage

            //Act
            var addressesList = _addressService.GetAll(null, 2, null, null);

            //Assert (1x Addresses do mapping + 5x criados no banco em memória)
            addressesList.Should().HaveCount(6);
        }

        [Fact]
        public void Add_AdicionarAddress_QuandoEnviadoAddress_CriarAddress()
        {
            //Arrage
            Address address = new Address("Teste", "12345678", 123, "Teste", 1);
            string exceptionMessage = "";

            //Act
            try
            {
                _addressService.Add(address);
            } catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            //Assert
            Assert.Equal("", exceptionMessage);
        }

        [Theory]
        [InlineData(null, "12345678", 123, "Casa", 1)]
        [InlineData("Rua", null, 123, "Casa", 1)]
        [InlineData("Rua", "12345678", 123, "Casa", null)]
        public void Add_AdicionarAddress_QuandoEnviadoAddressErrado_RetornaException(string street, string cep, int number, string complement, int cityId)
        {
            //Arrage
            Address address = new Address(street, cep, number, complement, cityId);
            string exceptionMessage = "";

            //Act
            try
            {
                _addressService.Add(address);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            //Assert
            Assert.NotEqual("", exceptionMessage);
        }

        [Theory]
        [InlineData(1)] //Criado no mapping
        [InlineData(2)] //Criado no mapping
        [InlineData(3)] //Criado no mapping
        [InlineData(100)] //Criado no banco em memória
        [InlineData(101)] //Criado no banco em memória
        [InlineData(102)] //Criado no banco em memória
        public void GetById_RetornarAddressPorId_QuandoIdValido_RetornaAdress(int addressId)
        {
            //Arrage

            //Act
            var address = _addressService.GetById(addressId);

            //Assert (1x Addresses do mapping + 5x criados no banco em memória)
            Assert.NotNull(address);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(22)]
        [InlineData(33)]
        [InlineData(1000)]
        [InlineData(1010)]
        [InlineData(1020)]
        public void GetById_RetornarAddressPorId_QuandoIdInvalido_RetornaNull(int addressId)
        {
            //Arrage

            //Act
            var address = _addressService.GetById(addressId);

            //Assert (1x Addresses do mapping + 5x criados no banco em memória)
            Assert.Null(address);
        }

    }
}
