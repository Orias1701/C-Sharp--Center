using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xá»­ lÃ½ dá»¯ liá»‡u thÃ´ thÃ nh dá»¯ liá»‡u biá»ƒu Ä‘á»“
    /// </summary>
    public class ChartService
    {
        private readonly ProductRepository _productRepo;
        private readonly TransactionRepository _transactionRepo;

        public ChartService()
        {
            _productRepo = new ProductRepository();
            _transactionRepo = new TransactionRepository();
        }

        /// <summary>
        /// Láº¥y dá»¯ liá»‡u tá»“n kho theo sáº£n pháº©m (cho biá»ƒu Ä‘á»“ cá»™t)
        /// </summary>
        public Dictionary<string, int> GetInventoryByProduct()
        {
            try
            {
                var products = _productRepo.GetAllProducts();
                return products.ToDictionary(p => p.ProductName, p => p.Quantity);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y dá»¯ liá»‡u tá»“n kho: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y dá»¯ liá»‡u giÃ¡ trá»‹ tá»“n kho theo sáº£n pháº©m (cho biá»ƒu Ä‘á»“ cá»™t)
        /// </summary>
        public Dictionary<string, decimal> GetInventoryValueByProduct()
        {
            try
            {
                var products = _productRepo.GetAllProducts();
                return products.ToDictionary(p => p.ProductName, p => p.Price * p.Quantity);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y giÃ¡ trá»‹ tá»“n kho: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y dá»¯ liá»‡u Nháº­p/Xuáº¥t theo ngÃ y
        /// </summary>
        public Dictionary<string, int> GetImportExportByDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var transactions = _transactionRepo.GetAllTransactions();
                var filtered = transactions.Where(t => t.DateCreated >= fromDate && t.DateCreated <= toDate).ToList();

                var result = new Dictionary<string, int>();
                foreach (var group in filtered.GroupBy(t => t.DateCreated.Date))
                {
                    int imports = group.Where(t => t.Type == "Import").Sum(t => t.Details.Sum(d => d.Quantity));
                    int exports = group.Where(t => t.Type == "Export").Sum(t => t.Details.Sum(d => d.Quantity));
                    result[group.Key.ToString("yyyy-MM-dd")] = imports - exports;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y dá»¯ liá»‡u nháº­p/xuáº¥t: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y dá»¯ liá»‡u Nháº­p/Xuáº¥t theo loáº¡i (cho biá»ƒu Ä‘á»“ trÃ²n)
        /// </summary>
        public Dictionary<string, int> GetTransactionByType()
        {
            try
            {
                var transactions = _transactionRepo.GetAllTransactions();
                var imports = transactions.Count(t => t.Type == "Import");
                var exports = transactions.Count(t => t.Type == "Export");

                return new Dictionary<string, int>
                {
                    { "Nháº­p kho", imports },
                    { "Xuáº¥t kho", exports }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y dá»¯ liá»‡u loáº¡i phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y thá»‘ng kÃª tá»•ng quÃ¡t
        /// </summary>
        public Dictionary<string, object> GetGeneralStatistics()
        {
            try
            {
                var products = _productRepo.GetAllProducts();
                var transactions = _transactionRepo.GetAllTransactions();

                return new Dictionary<string, object>
                {
                    { "TotalProducts", products.Count },
                    { "TotalQuantity", products.Sum(p => p.Quantity) },
                    { "TotalValue", products.Sum(p => p.Price * p.Quantity) },
                    { "LowStockCount", products.Count(p => p.IsLowStock) },
                    { "TotalTransactions", transactions.Count },
                    { "AvgProductPrice", products.Count > 0 ? products.Average(p => p.Price) : 0 }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y thá»‘ng kÃª: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y dá»¯ liá»‡u Nháº­p/Xuáº¥t theo thÃ¡ng (12 thÃ¡ng gáº§n nháº¥t)
        /// Tráº£ vá» Dictionary vá»›i key lÃ  thÃ¡ng (yyyy-MM), value lÃ  {Import: giÃ¡ trá»‹, Export: giÃ¡ trá»‹}
        /// </summary>
        public Dictionary<string, Dictionary<string, decimal>> GetImportExportByMonth()
        {
            var result = new Dictionary<string, Dictionary<string, decimal>>();
            try
            {
                var transactions = _transactionRepo.GetAllTransactions();
                Console.WriteLine($"[ChartService] GetImportExportByMonth: Total transactions = {transactions.Count}");

                // Láº¥y 12 thÃ¡ng gáº§n nháº¥t
                DateTime now = DateTime.Now;
                DateTime startDate = now.AddMonths(-11);

                // Táº¡o danh sÃ¡ch cÃ¡c thÃ¡ng
                for (int i = 0; i < 12; i++)
                {
                    DateTime monthDate = startDate.AddMonths(i);
                    string monthKey = monthDate.ToString("yyyy-MM");
                    result[monthKey] = new Dictionary<string, decimal>
                    {
                        { "Import", 0 },
                        { "Export", 0 }
                    };
                }

                // TÃ­nh toÃ¡n tá»•ng giÃ¡ trá»‹ nháº­p/xuáº¥t cho má»—i thÃ¡ng
                int processedCount = 0;
                foreach (var transaction in transactions)
                {
                    string monthKey = transaction.DateCreated.ToString("yyyy-MM");
                    
                    // Chá»‰ xá»­ lÃ½ cÃ¡c giao dá»‹ch trong 12 thÃ¡ng gáº§n nháº¥t
                    if (!result.ContainsKey(monthKey))
                        continue;

                    decimal totalValue = 0;
                    if (transaction.Details != null && transaction.Details.Count > 0)
                    {
                        totalValue = transaction.Details.Sum(d => d.UnitPrice * (decimal)d.Quantity);
                        Console.WriteLine($"[ChartService]   {monthKey} {transaction.Type}: {transaction.Details.Count} items, Total={totalValue}");
                    }

                    if (transaction.Type == "Import")
                    {
                        result[monthKey]["Import"] += totalValue;
                    }
                    else if (transaction.Type == "Export")
                    {
                        result[monthKey]["Export"] += totalValue;
                    }

                    processedCount++;
                }

                Console.WriteLine($"[ChartService] Processed {processedCount} transactions");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChartService] ERROR: {ex.Message}\n{ex.StackTrace}");
                throw new Exception("Lá»—i khi láº¥y dá»¯ liá»‡u nháº­p/xuáº¥t theo thÃ¡ng: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y dá»¯ liá»‡u Nháº­p/Xuáº¥t theo ngÃ y (30 ngÃ y gáº§n nháº¥t)
        /// Tráº£ vá» Dictionary vá»›i key lÃ  ngÃ y (yyyy-MM-dd), value lÃ  {Import: giÃ¡ trá»‹, Export: giÃ¡ trá»‹}
        /// </summary>
        public Dictionary<string, Dictionary<string, decimal>> GetImportExportByDay()
        {
            return GetImportExportByDay(DateTime.Now);
        }

        public Dictionary<string, Dictionary<string, decimal>> GetImportExportByDay(DateTime anchorDate)
        {
            var result = new Dictionary<string, Dictionary<string, decimal>>();
            try
            {
                var transactions = _transactionRepo.GetAllTransactions();
                Console.WriteLine($"[ChartService] GetImportExportByDay: Total transactions = {transactions.Count}");

                // Láº¥y 30 ngÃ y trÆ°á»›c tá»« ngÃ y Ä‘Æ°á»£c chá»n (anchorDate)
                DateTime startDate = anchorDate.AddDays(-29);

                // Táº¡o danh sÃ¡ch cÃ¡c ngÃ y
                for (int i = 0; i < 30; i++)
                {
                    DateTime dayDate = startDate.AddDays(i);
                    string dayKey = dayDate.ToString("yyyy-MM-dd");
                    result[dayKey] = new Dictionary<string, decimal>
                    {
                        { "Import", 0 },
                        { "Export", 0 }
                    };
                }

                // TÃ­nh toÃ¡n tá»•ng giÃ¡ trá»‹ nháº­p/xuáº¥t cho má»—i ngÃ y
                int processedCount = 0;
                foreach (var transaction in transactions)
                {
                    string dayKey = transaction.DateCreated.ToString("yyyy-MM-dd");
                    
                    // Chá»‰ xá»­ lÃ½ cÃ¡c giao dá»‹ch trong 30 ngÃ y Ä‘Ã£ chá»n
                    if (!result.ContainsKey(dayKey))
                        continue;

                    decimal totalValue = 0;
                    if (transaction.Details != null && transaction.Details.Count > 0)
                    {
                        totalValue = transaction.Details.Sum(d => d.UnitPrice * (decimal)d.Quantity);
                        Console.WriteLine($"[ChartService]   {dayKey} {transaction.Type}: {transaction.Details.Count} items, Total={totalValue}");
                    }

                    if (transaction.Type == "Import")
                    {
                        result[dayKey]["Import"] += totalValue;
                    }
                    else if (transaction.Type == "Export")
                    {
                        result[dayKey]["Export"] += totalValue;
                    }

                    processedCount++;
                }

                Console.WriteLine($"[ChartService] Processed {processedCount} transactions for days");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChartService] ERROR: {ex.Message}\n{ex.StackTrace}");
                throw new Exception("Lá»—i khi láº¥y dá»¯ liá»‡u nháº­p/xuáº¥t theo ngÃ y: " + ex.Message);
            }
        }
    }
}




