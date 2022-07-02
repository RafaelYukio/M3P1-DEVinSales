using DevInSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Data.DTOs.ApiDTOs
{
    public class SaleByBuyerRequest
    {
        public string SellerId { get; private set; }
        public DateTime SaleDate { get; private set; }

        public SaleByBuyerRequest(string sellerId, DateTime saleDate)
        {
            SellerId = sellerId;
            SaleDate = saleDate;
        }

        public Sale ConvertToEntity(string userId)
        {
            return new Sale(userId, SellerId, SaleDate);
        }
    }
}
