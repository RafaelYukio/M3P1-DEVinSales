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
    public class ProductServiceTest
    {
        private readonly DataContext _dataContext;
        private readonly DBInMemory _dbInMemory;
        private readonly ProductService _productService;

        public ProductServiceTest()
        {
            _dbInMemory = new DBInMemory();
            _dataContext = _dbInMemory.GetContext();
            _productService = new ProductService(_dataContext);
        }

        [Theory]
        [InlineData(1)] //Criado no mapping
        [InlineData(2)] //Criado no mapping
        [InlineData(3)] //Criado no mapping
        [InlineData(100)] //Criado no banco em memória
        [InlineData(101)] //Criado no banco em memória
        [InlineData(102)] //Criado no banco em memória
        public void ObterProductPorId_RetornarProdutoPorId_QuandoIdValido_RetornarProduto(int id)
        {
            //Arrange

            //Act
            var produto = _productService.ObterProductPorId(id);

            //Assert
            Assert.NotNull(produto);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(888)]
        [InlineData(777)]

        public void ObterProductPorId_RetornarProdutoPorId_QuandoIdInvalido_RetornarNull(int id)
        {
            //Arrange

            //Act
            var produto = _productService.ObterProductPorId(id);

            //Assert
            Assert.Null(produto);
        }

        [Theory]
        [InlineData("Coca-Cola")] //Criado no mapping
        [InlineData("Cerveja Itaipava")] //Criado no mapping
        [InlineData("Cerveja Corona")] //Criado no mapping
        [InlineData("Produto 100")] //Criado no banco em memória
        [InlineData("Produto 101")] //Criado no banco em memória
        [InlineData("Produto 102")] //Criado no banco em memória
        public void ProdutoExiste_RetornaProdutoPeloNome_QuandoNomeValido_RetornaProduto(string nome)
        {
            //Arrange

            //Act
            var retorno = _productService.ProdutoExiste(nome);

            //Assert
            Assert.True(retorno);
        }

        [Theory]
        [InlineData("aaaaa")]
        [InlineData("bbbbb")]
        [InlineData("ccccc")]
        public void ProdutoExiste_RetornaProdutoPeloNome_QuandoNomeInvalido_RetornaNull(string nome)
        {
            //Arrange

            //Act
            var retorno = _productService.ProdutoExiste(nome);

            //Assert
            Assert.False(retorno);
        }

        [Fact]
        public void Delete_DeletarProdutoComId_QuandoIdValido_DeletarProduto()
        {
            //Arrange
            int id = 1;
            string exceptionMessage = "";

            //Act
            try
            {
                _productService.Delete(id);
            } catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            //Assert
            Assert.Equal("", exceptionMessage);
        }

        [Fact]
        public void Delete_DeletarProdutoComId_QuandoIdInvalido_RetoraException()
        {
            //Arrange
            int id = 999;

            //Act
            var exception = Assert.Throws<Exception>(() => _productService.Delete(id));

            //Assert
            Assert.Equal("o Produto não existe", exception.Message);
        }
    }
}
