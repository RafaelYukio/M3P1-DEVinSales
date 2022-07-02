using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Data.DTOs.ApiDTOs
{
    public class SaleResponse
    {
        public int SaleId { get; private set; }
        public string SellerName { get; private set; }
        public string BuyerName { get; private set; }
        public DateTime SaleDate { get; private set; }
        public List<SaleProductResponse> SaleProducts { get; private set; }

        public SaleResponse(int saleId, DateTime saleDate, List<SaleProductResponse> saleProducts)
        {
            SaleId = saleId;
            SaleDate = saleDate;
            SaleProducts = saleProducts;
        }
    }
}
