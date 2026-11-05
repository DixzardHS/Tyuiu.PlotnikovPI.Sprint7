using System;
using System.ComponentModel;

namespace Tyuiu.PlontikovPI.Sprint7.Lib.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string ClientFullName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Today;
        public DateTime ExecutionDate { get; set; } = DateTime.Today;
        public decimal Cost { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }

        [Browsable(false)]
        public decimal CalculatedCost => ProductPrice * Quantity;
    }
}