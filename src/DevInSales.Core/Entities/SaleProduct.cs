namespace DevInSales.Core.Entities
{
    public class SaleProduct : Entity
    {
        public int SaleId { get; private set; }
        public int ProductId { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Amount { get; private set; }
        public Sale? Sales { get; private set; }
        public Product? Products { get; private set; }

        public SaleProduct(int saleId, int productId, decimal unitPrice, int amount)
        {
            SaleId = saleId;
            ProductId = productId;
            UnitPrice = unitPrice;
            Amount = amount;
        }

        public SaleProduct(int id, int saleId, int productId, decimal unitPrice, int amount)
        {
            Id = id;
            SaleId = saleId;
            ProductId = productId;
            UnitPrice = unitPrice;
            Amount = amount;
        }

        public void UpdateUnitPrice(decimal unitPrice)
        {            
            UnitPrice = unitPrice;            
        }          

        public void UpdateAmount(int amount)
        {
            Amount = amount;
        }
    }
}