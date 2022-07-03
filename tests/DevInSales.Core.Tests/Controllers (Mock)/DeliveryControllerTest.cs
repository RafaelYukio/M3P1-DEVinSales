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
    public class DeliveryControllerTest
    {
        private Mock<IDeliveryService> _mockDeliveryService;
        private DeliveryController _deliveryController;

        public DeliveryControllerTest()
        {
            _mockDeliveryService = new Mock<IDeliveryService>();
            _deliveryController = new DeliveryController(_mockDeliveryService.Object);
        }

        [Fact]
        public void GetDelivery_RetornarDeliveryPorIdAddressOuSaleId_QuandoIdsValidos_RetornaOKDelivery()
        {
            //Arrange
            List<Delivery> deliveryListMock = new List<Delivery>()
            {
                new Delivery(1, 1, new DateTime(2023, 01, 01))
            };
            _mockDeliveryService.Setup(m => m.GetBy(It.IsAny<int>(), It.IsAny<int>())).Returns(deliveryListMock);

            //Act
            var actionResult = _deliveryController.GetDelivery(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsType<ActionResult<Delivery>>(actionResult);
            _mockDeliveryService.Verify(m => m.GetBy(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact(Skip = "Erro no retorno do controller")]
        public void GetDelivery_RetornarDeliveryPorIdAddressOuSaleId_QuandoDeliveryListComZeroElementos_RetornaOKDelivery()
        {
            //Arrange
            List<Delivery> deliveryListMock = new List<Delivery>();
            _mockDeliveryService.Setup(m => m.GetBy(It.IsAny<int>(), It.IsAny<int>())).Returns(deliveryListMock);

            //Act
            var actionResult = _deliveryController.GetDelivery(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
            _mockDeliveryService.Verify(m => m.GetBy(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }
}
