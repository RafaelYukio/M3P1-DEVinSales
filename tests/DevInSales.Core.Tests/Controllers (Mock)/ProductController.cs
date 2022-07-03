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
    public class ProductControllerTest
    {
        private Mock<IProductService> _mockProductService;
        private ProductController _productController;

        public ProductControllerTest()
        {
            _mockProductService = new Mock<IProductService>();
            _productController = new ProductController(_mockProductService.Object);
        }

        [Fact]
        public void ObterProdutoPorId_RetornarProdutoPeloId_QuandoIdValido_RetornaProduct()
        {
            //Arrange
            Product productMock = new Product("Teste", 99);
            _mockProductService.Setup(m => m.ObterProductPorId(It.IsAny<int>())).Returns(productMock);

            //Act
            var actionResult = _productController.ObterProdutoPorId(It.IsAny<int>());

            //Assert
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.Equal(productMock, okResult.Value);
            _mockProductService.Verify(m => m.ObterProductPorId(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ObterProdutoPorId_RetornarProdutoPeloId_QuandoIdInvalido_RetornaNotFound()
        {
            //Arrange
            _mockProductService.Setup(m => m.ObterProductPorId(It.IsAny<int>())).Returns((Product)null);

            //Act
            var actionResult = _productController.ObterProdutoPorId(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
            _mockProductService.Verify(m => m.ObterProductPorId(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void AtualizarProduto_AtualizarProduto_QuandoProdutoValido_RetornaNoContent()
        {
            //Arrange
            Product productMock = new Product("Teste", 99);
            AddProduct addProductMock = new AddProduct("Teste Teste", 100);
            _mockProductService.Setup(m => m.ObterProductPorId(It.IsAny<int>())).Returns(productMock);

            //Act
            var actionResult = _productController.AtualizarProduto(addProductMock, It.IsAny<int>());

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockProductService.Verify(m => m.ObterProductPorId(It.IsAny<int>()), Times.Once);
        }
    }
}
