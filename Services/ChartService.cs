using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xử lý dữ liệu thô thành dữ liệu biểu đồ
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
        /// Lấy dữ liệu tồn kho theo sản phẩm (cho biểu đồ cột)
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
                throw new Exception("Lỗi khi lấy dữ liệu tồn kho: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy dữ liệu giá trị tồn kho theo sản phẩm (cho biểu đồ cột)
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
                throw new Exception("Lỗi khi lấy giá trị tồn kho: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy dữ liệu Nhập/Xuất theo ngày
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
                throw new Exception("Lỗi khi lấy dữ liệu nhập/xuất: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy dữ liệu Nhập/Xuất theo loại (cho biểu đồ tròn)
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
                    { "Nhập kho", imports },
                    { "Xuất kho", exports }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu loại phiếu: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thống kê tổng quát
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
                throw new Exception("Lỗi khi lấy thống kê: " + ex.Message);
            }
        }
    }
}
