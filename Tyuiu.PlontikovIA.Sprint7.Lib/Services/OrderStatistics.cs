using System.Collections.Generic;
using System.Linq;
using Tyuiu.PlontikovPI.Sprint7.Lib.Models;

namespace Tyuiu.PlontikovPI.Sprint7.Lib.Services
{
    public class OrderStatistics
    {
        public int Count { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public decimal MinCost { get; set; }
        public decimal MaxCost { get; set; }
        public int TotalQuantity { get; set; }
        public decimal AverageProductPrice { get; set; }

        public static OrderStatistics Calculate(IEnumerable<Order> orders)
        {
            var list = orders?.ToList() ?? new List<Order>();

            if (list.Count == 0)
                return new OrderStatistics();

            return new OrderStatistics
            {
                Count = list.Count,
                TotalCost = list.Sum(o => o.Cost),
                AverageCost = list.Average(o => o.Cost),
                MinCost = list.Min(o => o.Cost),
                MaxCost = list.Max(o => o.Cost),
                TotalQuantity = list.Sum(o => o.Quantity),
                AverageProductPrice = list.Average(o => o.ProductPrice)
            };
        }

        public static Dictionary<string, decimal> GetCostByProduct(IEnumerable<Order> orders)
        {
            return (orders ?? Enumerable.Empty<Order>())
                .GroupBy(o => string.IsNullOrWhiteSpace(o.ProductName)
                    ? "(без названия)"
                    : o.ProductName)
                .ToDictionary(g => g.Key, g => g.Sum(o => o.Cost));
        }
    }
}