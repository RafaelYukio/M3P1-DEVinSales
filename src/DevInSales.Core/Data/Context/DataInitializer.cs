using DevInSales.Core.Data.DTOs.ApiDTOs;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using DevInSales.Core.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Data.Context
{
    public class DataInitializer : IDataInitializer
    {
        private readonly DataContext _context;
        private readonly ISaleService _saleService;
        private readonly IIdentityService _identityService;
        private readonly ISaleProductService _saleProductService;

        public DataInitializer(DataContext context,
                               ISaleService saleService,
                               IIdentityService identityService,
                               ISaleProductService saleProductService)
        {
            _context = context;
            _saleService = saleService;
            _identityService = identityService;
            _saleProductService = saleProductService;
        }

        public async Task Initializer()
        {
            await _context.Database.MigrateAsync();
            await _context.Database.EnsureCreatedAsync();

            if (_saleService.GetSaleById(1) == null)
            {
                List<string> listaDeIdsUsuarios = new List<string>
            {
                await _identityService.RetornarIdPorEmail("usuario1@dev.com.br"),
                await _identityService.RetornarIdPorEmail("usuario2@dev.com.br"),
                await _identityService.RetornarIdPorEmail("usuario3@dev.com.br"),
                await _identityService.RetornarIdPorEmail("usuario4@dev.com.br"),
            };

                List<int> listaDeIdsSales = new List<int>{
            _saleService.CreateSaleByUserId(new Sale(listaDeIdsUsuarios[0], listaDeIdsUsuarios[1], new DateTime(2022, 01, 01))),
            _saleService.CreateSaleByUserId(new Sale(listaDeIdsUsuarios[1], listaDeIdsUsuarios[2], new DateTime(2022, 02, 02))),
            _saleService.CreateSaleByUserId(new Sale(listaDeIdsUsuarios[2], listaDeIdsUsuarios[3], new DateTime(2022, 03, 03))),
            _saleService.CreateSaleByUserId(new Sale(listaDeIdsUsuarios[3], listaDeIdsUsuarios[2], new DateTime(2022, 03, 03))),
            };

                List<SaleProductRequest> listaDeSaleProducts = new List<SaleProductRequest>
            {
                new SaleProductRequest(1, 25, 2),
                new SaleProductRequest(2, 15, 3),
                new SaleProductRequest(3, 10, 4),
                new SaleProductRequest(4, 5, 5),
            };

                _saleProductService.CreateSaleProduct(listaDeIdsSales[0], listaDeSaleProducts[0]);
                _saleProductService.CreateSaleProduct(listaDeIdsSales[1], listaDeSaleProducts[1]);
                _saleProductService.CreateSaleProduct(listaDeIdsSales[2], listaDeSaleProducts[2]);
                _saleProductService.CreateSaleProduct(listaDeIdsSales[3], listaDeSaleProducts[3]);

                _saleService.CreateDeliveryForASale(new Delivery(1, listaDeIdsSales[0], new DateTime(2023, 01, 01)));
                _saleService.CreateDeliveryForASale(new Delivery(2, listaDeIdsSales[1], new DateTime(2023, 02, 02)));
                _saleService.CreateDeliveryForASale(new Delivery(3, listaDeIdsSales[2], new DateTime(2023, 03, 03)));
                _saleService.CreateDeliveryForASale(new Delivery(4, listaDeIdsSales[3], new DateTime(2023, 04, 04)));

            }

        }
    }
}
