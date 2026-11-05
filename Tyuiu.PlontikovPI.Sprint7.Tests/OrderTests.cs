using System;
using System.Collections.Generic;
using System.IO;
using Tyuiu.PlontikovPI.Sprint7.Lib.Models;
using Tyuiu.PlontikovPI.Sprint7.Lib.Services;
using Xunit;

namespace Tyuiu.PlontikovPI.Sprint7.Tests
{
    public class OrderTests
    {
        [Fact]
        public void Statistics_Calculate_ReturnsCorrectValues()
        {
            var orders = new List<Order>
            {
                new Order { Cost = 100m, ProductPrice = 10m, Quantity = 2 },
                new Order { Cost = 300m, ProductPrice = 30m, Quantity = 3 }
            };

            var stats = OrderStatistics.Calculate(orders);

            Assert.Equal(2, stats.Count);
            Assert.Equal(400m, stats.TotalCost);
            Assert.Equal(200m, stats.AverageCost);
            Assert.Equal(100m, stats.MinCost);
            Assert.Equal(300m, stats.MaxCost);
            Assert.Equal(5, stats.TotalQuantity);
            Assert.Equal(20m, stats.AverageProductPrice);
        }

        [Fact]
        public void Statistics_EmptyList_ReturnsZeros()
        {
            var stats = OrderStatistics.Calculate(new List<Order>());

            Assert.Equal(0, stats.Count);
            Assert.Equal(0m, stats.TotalCost);
            Assert.Equal(0m, stats.AverageCost);
            Assert.Equal(0m, stats.MinCost);
            Assert.Equal(0m, stats.MaxCost);
        }

        [Fact]
        public void CsvRepository_SaveAndLoad_RoundTrip()
        {
            string path = Path.GetTempFileName();

            try
            {
                var repository = new OrderCsvRepository();
                var orders = new List<Order>
                {
                    new Order
                    {
                        OrderId = 1,
                        ClientFullName = "Иванов, Иван Иванович",
                        AccountNumber = "40817810000000000001",
                        Address = "г. Москва, ул. Ленина, д. 1",
                        Phone = "+79990000001",
                        OrderDate = new DateTime(2024, 5, 1),
                        ExecutionDate = new DateTime(2024, 5, 10),
                        Cost = 1500.50m,
                        ProductName = "Клавиатура",
                        ProductPrice = 500.25m,
                        Quantity = 3
                    }
                };

                repository.Save(path, orders);
                var loaded = repository.Load(path);

                Assert.Single(loaded);
                Assert.Equal(1, loaded[0].OrderId);
                Assert.Equal("Иванов, Иван Иванович", loaded[0].ClientFullName);
                Assert.Equal(1500.50m, loaded[0].Cost);
                Assert.Equal("Клавиатура", loaded[0].ProductName);
                Assert.Equal(3, loaded[0].Quantity);
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Fact]
        public void GetCostByProduct_GroupsByProduct()
        {
            var orders = new List<Order>
            {
                new Order { ProductName = "Мышь", Cost = 100m },
                new Order { ProductName = "Мышь", Cost = 200m },
                new Order { ProductName = "Клавиатура", Cost = 300m }
            };

            var result = OrderStatistics.GetCostByProduct(orders);

            Assert.Equal(300m, result["Мышь"]);
            Assert.Equal(300m, result["Клавиатура"]);
        }
    }
}