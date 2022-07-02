using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Entities
{
    public class Delivery : Entity
    {
        public int AddressId { get; private set; }
        public int SaleId { get; private set; }
        public DateTime DeliveryForecast { get; private set; }
        public Sale? Sale { get; private set; }
        public Address? Address { get; private set; }

        public Delivery(int addressId, int saleId, DateTime deliveryForecast)
        {
            AddressId = addressId;
            SaleId = saleId;
            DeliveryForecast = deliveryForecast;
        }

        public Delivery(int id, int addressId, int saleId, DateTime deliveryForecast)
        {
            Id = id;
            AddressId = addressId;
            SaleId = saleId;
            DeliveryForecast = deliveryForecast;
        }

    }
}
