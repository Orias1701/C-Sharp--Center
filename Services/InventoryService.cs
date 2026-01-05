using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;
using Newtonsoft.Json;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xử lý logic Nhập/Xuất kho, tính toán tồn kho
    /// </summary>
    public class InventoryService
    {
        private readonly ProductRepository _productRepo;
        private readonly TransactionRepository _transactionRepo;
        private readonly LogRepository _logRepo;

        public InventoryService()
        {
            _productRepo = new ProductRepository();
            _transactionRepo = new TransactionRepository();
            _logRepo = new LogRepository();
        }

        /// <summary>
        /// Thực hiện phiếu nhập kho
        /// </summary>
        public bool ImportStock(int productId, int quantity, decimal unitPrice, string note = "")
        {
            try
            {
                var product = _productRepo.GetProductById(productId);
                if (product == null)
                    return false;

                // Lưu dữ liệu cũ trước khi thay đổi
                var oldData = new { product.Quantity, product.ProductID };
                
                // Tạo phiếu
                var transaction = new StockTransaction
                {
                    Type = "Import",
                    DateCreated = DateTime.Now,
                    Note = note
                };
                int transId = _transactionRepo.CreateTransaction(transaction);

                // Thêm chi tiết
                var detail = new TransactionDetail
                {
                    TransactionID = transId,
                    ProductID = productId,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };
                _transactionRepo.AddTransactionDetail(detail);

                // Cập nhật tồn kho
                int newQuantity = product.Quantity + quantity;
                _productRepo.UpdateQuantity(productId, newQuantity);

                // Ghi nhật ký
                var newData = new { Quantity = newQuantity, ProductID = productId };
                _logRepo.LogAction("IMPORT_STOCK", 
                    $"Nhập {quantity} sản phẩm ID {productId}",
                    JsonConvert.SerializeObject(oldData));

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi nhập kho: " + ex.Message);
            }
        }

        /// <summary>
        /// Thực hiện phiếu xuất kho
        /// </summary>
        public bool ExportStock(int productId, int quantity, decimal unitPrice, string note = "")
        {
            try
            {
                var product = _productRepo.GetProductById(productId);
                if (product == null)
                    return false;

                if (product.Quantity < quantity)
                    throw new Exception("Tồn kho không đủ để xuất");

                // Lưu dữ liệu cũ
                var oldData = new { product.Quantity, product.ProductID };

                // Tạo phiếu
                var transaction = new StockTransaction
                {
                    Type = "Export",
                    DateCreated = DateTime.Now,
                    Note = note
                };
                int transId = _transactionRepo.CreateTransaction(transaction);

                // Thêm chi tiết
                var detail = new TransactionDetail
                {
                    TransactionID = transId,
                    ProductID = productId,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };
                _transactionRepo.AddTransactionDetail(detail);

                // Cập nhật tồn kho
                int newQuantity = product.Quantity - quantity;
                _productRepo.UpdateQuantity(productId, newQuantity);

                // Ghi nhật ký
                _logRepo.LogAction("EXPORT_STOCK",
                    $"Xuất {quantity} sản phẩm ID {productId}",
                    JsonConvert.SerializeObject(oldData));

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xuất kho: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm cảnh báo (tồn kho thấp)
        /// </summary>
        public List<Product> GetLowStockProducts()
        {
            try
            {
                var products = _productRepo.GetAllProducts();
                return products.Where(p => p.IsLowStock).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy sản phẩm cảnh báo: " + ex.Message);
            }
        }

        /// <summary>
        /// Tính tổng giá trị tồn kho
        /// </summary>
        public decimal GetTotalInventoryValue()
        {
            try
            {
                var products = _productRepo.GetAllProducts();
                return products.Sum(p => p.Price * p.Quantity);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính giá trị tồn kho: " + ex.Message);
            }
        }

        /// <summary>
        /// Hoàn tác thao tác cuối cùng (dựa trên nhật ký)
        /// </summary>
        public bool UndoLastAction()
        {
            try
            {
                var logs = _logRepo.GetAllLogs();
                if (logs.Count == 0)
                    return false;

                var lastLog = logs.First();
                if (lastLog.DataBefore == "")
                    return false;

                // Deserialize JSON thành JObject thay vì dynamic
                var jsonObj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(lastLog.DataBefore);
                if (jsonObj == null)
                    return false;

                int productId = (int)jsonObj["ProductID"];
                int oldQuantity = (int)jsonObj["Quantity"];

                _productRepo.UpdateQuantity(productId, oldQuantity);
                _logRepo.LogAction("UNDO_ACTION", $"Hoàn tác hành động {lastLog.ActionType}");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi hoàn tác: " + ex.Message);
            }
        }
    }
}
