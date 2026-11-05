using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Tyuiu.PlontikovPI.Sprint7.Lib.Models;

namespace Tyuiu.PlontikovPI.Sprint7.Lib.Services
{
    public class OrderCsvRepository
    {
        private const string Header =
            "OrderId,ClientFullName,AccountNumber,Address,Phone,OrderDate,ExecutionDate,Cost,ProductName,ProductPrice,Quantity";

        public List<Order> Load(string filePath)
        {
            var result = new List<Order>();

            if (!File.Exists(filePath))
                return result;

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length == 0)
                return result;

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                var fields = ParseLine(lines[i]);
                if (fields.Count < 11)
                    continue;

                var order = new Order
                {
                    OrderId = ParseInt(fields[0]),
                    ClientFullName = fields[1],
                    AccountNumber = fields[2],
                    Address = fields[3],
                    Phone = fields[4],
                    OrderDate = ParseDate(fields[5]),
                    ExecutionDate = ParseDate(fields[6]),
                    Cost = ParseDecimal(fields[7]),
                    ProductName = fields[8],
                    ProductPrice = ParseDecimal(fields[9]),
                    Quantity = ParseInt(fields[10])
                };

                result.Add(order);
            }

            return result;
        }

        public void Save(string filePath, IEnumerable<Order> orders)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Header);

            foreach (var o in orders ?? Enumerable.Empty<Order>())
            {
                sb.AppendLine(string.Join(",", new[]
                {
                    o.OrderId.ToString(CultureInfo.InvariantCulture),
                    Escape(o.ClientFullName),
                    Escape(o.AccountNumber),
                    Escape(o.Address),
                    Escape(o.Phone),
                    o.OrderDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    o.ExecutionDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    o.Cost.ToString(CultureInfo.InvariantCulture),
                    Escape(o.ProductName),
                    o.ProductPrice.ToString(CultureInfo.InvariantCulture),
                    o.Quantity.ToString(CultureInfo.InvariantCulture)
                }));
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private static string Escape(string value)
        {
            value = value ?? string.Empty;

            if (value.Contains(",") || value.Contains("\"") ||
                value.Contains("\n") || value.Contains("\r"))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }

            return value;
        }

        private static List<string> ParseLine(string line)
        {
            var result = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            sb.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else if (c == ',')
                    {
                        result.Add(sb.ToString());
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }

            result.Add(sb.ToString());
            return result;
        }

        private static int ParseInt(string value)
        {
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result)
                ? result
                : 0;
        }

        private static decimal ParseDecimal(string value)
        {
            return decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result)
                ? result
                : 0m;
        }

        private static DateTime ParseDate(string value)
        {
            return DateTime.TryParseExact(
                value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime result)
                ? result
                : DateTime.Today;
        }
    }
}