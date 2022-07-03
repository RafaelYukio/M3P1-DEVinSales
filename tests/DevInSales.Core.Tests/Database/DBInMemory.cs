using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Tests.Database
{
    public class DBInMemory
    {
        private readonly DataContext _dataContext;
        private readonly SqliteConnection _connection;

        public DBInMemory()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;

            _dataContext = new DataContext(options);
            InsertFakeData();
        }

        public DataContext GetContext() => _dataContext;

        public void Cleanup() =>
            _connection.Close();

        private void InsertFakeData()
        {
            if (_dataContext.Database.EnsureCreated())
            {
                var ids = new[] { 100, 101, 102, 103, 104 };
                var idsCriadosPeloBancoEmMemoria = new[] { 1, 2, 3, 4, 5 };
                
                ids.ToList().ForEach(id =>
                {
                     _dataContext.Addresses.Add(
                        new Address(id, $"Rua {id}", "12345678", id, "Casa", 2)
                    );
                });

                ids.ToList().ForEach(id =>
                {
                    _dataContext.Cities.Add(
                        new City(id, 25, $"Cidade {id}")
                    );
                });

                ids.ToList().ForEach(id =>
                {
                    _dataContext.Products.Add(
                        new Product(id, $"Produto {id}", id)
                    );
                });

                ids.ToList().ForEach(id =>
                {
                    _dataContext.Sales.Add(
                        new Sale($"BuyerId {id}", $"SellerId {id}", DateTime.Now)
                    );
                });

                ids.ToList().ForEach(id =>
                {
                    _dataContext.SaleProducts.Add(
                        new SaleProduct(id, idsCriadosPeloBancoEmMemoria[Array.IndexOf(ids, id)], id, 999, 99)
                    );
                });

                ids.ToList().ForEach(id =>
                {
                    _dataContext.Deliveries.Add(
                        new Delivery(id, idsCriadosPeloBancoEmMemoria[Array.IndexOf(ids, id)], new DateTime(2023, 01, 01))
                    );
                });

                _dataContext.SaveChanges();
            }
        }
    }
}
