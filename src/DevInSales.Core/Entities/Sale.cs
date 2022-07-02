using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevInSales.Core.Entities
{
    public class Sale : Entity
    {
        public string BuyerId { get; private set; }
        public string SellerId { get; private set; }
        public DateTime SaleDate { get; private set; }

        public Sale(string buyerId, string sellerId, DateTime saleDate)
        {
            BuyerId = buyerId;
            SellerId = sellerId;
            SaleDate = saleDate;
        }

        public void SetSaleDateToToday()
        {
            SaleDate = DateTime.Now.ToUniversalTime();
        }
 
    }
}