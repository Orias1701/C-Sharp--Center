using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;
using Newtonsoft.Json;

namespace WarehouseManagement.Services
{
    public class InventoryService
    {
        private readonly ProductRepository _productRepo;
        private readonly TransactionRepository _transactionRepo;
        private readonly LogRepository _logRepo;
        private readonly CategoryRepository _categoryRepo;

        public InventoryService()
        {
            _productRepo = new ProductRepository();
            _transactionRepo = new TransactionRepository();
            _logRepo = new LogRepository();
            _categoryRepo = new CategoryRepository();
        }

        public bool ImportStock(int productId, int quantity, decimal unitPrice, string note = "")
        {
            try
            {
                if (productId <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");
                if (quantity <= 0) throw new ArgumentException("Số lượng nhập phải lớn hơn 0");
                if (quantity > 999999) throw new ArgumentException("Số lượng quá lớn");
                if (unitPrice < 0) throw new ArgumentException("Đơn giá không được âm");
                if (unitPrice > 999999999) throw new ArgumentException("Đơn giá quá lớn");

                var product = _productRepo.GetProductById(productId);
                if (product == null) throw new ArgumentException("Sản phẩm không tồn tại");

                var oldData = new { product.Quantity, product.ProductID };
                
                var transaction = new Transaction
                {
                    Type = "Import",
                    DateCreated = DateTime.Now,
                    CreatedByUserID = GlobalUser.CurrentUser?.UserID ?? 0,
                    Note = string.IsNullOrWhiteSpace(note) ? "" : note.Trim(),
                    Status = "Pending",
                    Visible = true
                };
                int transId = _transactionRepo.CreateTransaction(transaction);

                var detail = new TransactionDetail
                {
                    TransactionID = transId,
                    ProductID = productId,
                    ProductName = product.ProductName,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    Visible = true
                };
                _transactionRepo.AddTransactionDetail(detail);

                int newQuantity = product.Quantity + quantity;
                if (newQuantity > 999999) throw new Exception("Tồn kho sẽ vượt quá giới hạn cho phép");
                
                // DEFERRED: _productRepo.UpdateQuantity(productId, newQuantity);
                _transactionRepo.UpdateTransactionTotal(transId);

                var newData = new { Quantity = newQuantity, ProductID = productId };
                _logRepo.LogAction("IMPORT_STOCK", 
                    $"Nhập {quantity} sản phẩm ID {productId}",
                    JsonConvert.SerializeObject(oldData));

                ActionsService.Instance.MarkAsChanged();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi nhập kho: " + ex.Message);
            }
        }

        public bool ExportStock(int productId, int quantity, decimal unitPrice, string note = "")
        {
            try
            {
                if (productId <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");
                if (quantity <= 0) throw new ArgumentException("Số lượng xuất phải lớn hơn 0");
                if (quantity > 999999) throw new ArgumentException("Số lượng quá lớn");
                if (unitPrice < 0) throw new ArgumentException("Đơn giá không được âm");
                if (unitPrice > 999999999) throw new ArgumentException("Đơn giá quá lớn");

                var product = _productRepo.GetProductById(productId);
                if (product == null) throw new ArgumentException("Sản phẩm không tồn tại");

                if (product.Quantity < quantity)
                    throw new Exception("Tồn kho không đủ để xuất (hiện có: " + product.Quantity + ")");

                var oldData = new { product.Quantity, product.ProductID };

                var transaction = new Transaction
                {
                    Type = "Export",
                    DateCreated = DateTime.Now,
                    CreatedByUserID = GlobalUser.CurrentUser?.UserID ?? 0,
                    Note = string.IsNullOrWhiteSpace(note) ? "" : note.Trim(),
                    Status = "Pending",
                    Visible = true
                };
                int transId = _transactionRepo.CreateTransaction(transaction);

                var detail = new TransactionDetail
                {
                    TransactionID = transId,
                    ProductID = productId,
                    ProductName = product.ProductName,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    Visible = true
                };
                _transactionRepo.AddTransactionDetail(detail);

                int newQuantity = product.Quantity - quantity;
                // DEFERRED: _productRepo.UpdateQuantity(productId, newQuantity);
                _transactionRepo.UpdateTransactionTotal(transId);

                _logRepo.LogAction("EXPORT_STOCK",
                    $"Xuất {quantity} sản phẩm ID {productId}",
                    JsonConvert.SerializeObject(oldData));

                ActionsService.Instance.MarkAsChanged();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xuất kho: " + ex.Message);
            }
        }

        public int ImportStockBatch(List<(int ProductId, int Quantity, decimal UnitPrice)> details, string note = "", int supplierId = 0)
        {
            try
            {
                if (details == null || details.Count == 0)
                    throw new ArgumentException("Danh sách sản phẩm không thể rỗng");

                var productIds = new List<int>();
                foreach (var (productId, quantity, unitPrice) in details)
                {
                    if (productIds.Contains(productId))
                        throw new ArgumentException($"Sản phẩm ID {productId} bị trùng lặp trong phiếu nhập");
                    
                    if (!_productRepo.ProductIdExists(productId))
                        throw new ArgumentException($"Sản phẩm ID {productId} không tồn tại trong hệ thống");
                    
                    productIds.Add(productId);
                }

                var transaction = new Transaction
                {
                    Type = "Import",
                    DateCreated = DateTime.Now,
                    CreatedByUserID = GlobalUser.CurrentUser?.UserID ?? 0,
                    Note = string.IsNullOrWhiteSpace(note) ? "" : note.Trim(),
                    Status = "Pending",
                    Visible = true,
                    SupplierID = supplierId > 0 ? supplierId : (int?)null
                };
                int transId = _transactionRepo.CreateTransaction(transaction);

                foreach (var (productId, quantity, unitPrice) in details)
                {
                    if (productId <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");
                    if (quantity <= 0) throw new ArgumentException("Số lượng nhập phải lớn hơn 0");
                    if (quantity > 999999) throw new ArgumentException("Số lượng quá lớn");
                    if (unitPrice < 0) throw new ArgumentException("Đơn giá không được âm");
                    if (unitPrice > 999999999) throw new ArgumentException("Đơn giá quá lớn");

                    var product = _productRepo.GetProductById(productId);
                    if (product == null) throw new ArgumentException($"Sản phẩm ID {productId} không tồn tại");

                    var detail = new TransactionDetail
                    {
                        TransactionID = transId,
                        ProductID = productId,
                        ProductName = product.ProductName,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        Visible = true
                    };
                    _transactionRepo.AddTransactionDetail(detail);

                    int newQuantity = product.Quantity + quantity;
                    if (newQuantity > 999999) throw new Exception("Tồn kho sẽ vượt quá giới hạn cho phép");

                    // DEFERRED: _productRepo.UpdateQuantity(productId, newQuantity);
                }

                _transactionRepo.UpdateTransactionTotal(transId);

                _logRepo.LogAction("IMPORT_BATCH", 
                    $"Nhập {details.Count} sản phẩm, Transaction ID {transId}",
                    "");

                ActionsService.Instance.MarkAsChanged();
                return transId;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi nhập kho batch: " + ex.Message);
            }
        }

        public int ExportStockBatch(List<(int ProductId, int Quantity, decimal UnitPrice)> details, string note = "", int customerId = 0)
        {
            try
            {
                if (details == null || details.Count == 0)
                    throw new ArgumentException("Danh sách sản phẩm không thể rỗng");

                var productIds = new List<int>();
                foreach (var (productId, quantity, unitPrice) in details)
                {
                    if (productIds.Contains(productId))
                        throw new ArgumentException($"Sản phẩm ID {productId} bị trùng lặp trong phiếu xuất");
                    productIds.Add(productId);
                }

                foreach (var (productId, quantity, unitPrice) in details)
                {
                    if (!_productRepo.ProductIdExists(productId))
                        throw new ArgumentException($"Sản phẩm ID {productId} không tồn tại trong hệ thống");

                    var product = _productRepo.GetProductById(productId);
                    if (product == null) throw new ArgumentException($"Sản phẩm ID {productId} không tồn tại");
                    if (product.Quantity < quantity)
                        throw new Exception($"Tồn kho {product.ProductName} không đủ (hiện có: {product.Quantity}, cần xuất: {quantity})");
                }

                var transaction = new Transaction
                {
                    Type = "Export",
                    DateCreated = DateTime.Now,
                    CreatedByUserID = GlobalUser.CurrentUser?.UserID ?? 0,
                    Note = string.IsNullOrWhiteSpace(note) ? "" : note.Trim(),
                    Status = "Pending",
                    Visible = true,
                    CustomerID = customerId > 0 ? customerId : (int?)null
                };
                int transId = _transactionRepo.CreateTransaction(transaction);

                foreach (var (productId, quantity, unitPrice) in details)
                {
                    if (productId <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");
                    if (quantity <= 0) throw new ArgumentException("Số lượng xuất phải lớn hơn 0");
                    if (quantity > 999999) throw new ArgumentException("Số lượng quá lớn");
                    if (unitPrice < 0) throw new ArgumentException("Đơn giá không được âm");
                    if (unitPrice > 999999999) throw new ArgumentException("Đơn giá quá lớn");

                    var product = _productRepo.GetProductById(productId);
                    
                    var detail = new TransactionDetail
                    {
                        TransactionID = transId,
                        ProductID = productId,
                        ProductName = product.ProductName,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        Visible = true
                    };
                    _transactionRepo.AddTransactionDetail(detail);

                    int newQuantity = product.Quantity - quantity;
                    // DEFERRED: _productRepo.UpdateQuantity(productId, newQuantity);
                }

                _transactionRepo.UpdateTransactionTotal(transId);

                _logRepo.LogAction("EXPORT_BATCH",
                    $"Xuất {details.Count} sản phẩm, Transaction ID {transId}",
                    "");
                ActionsService.Instance.MarkAsChanged();
                return transId;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xuất kho batch: " + ex.Message);
            }
        }

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

        public bool UndoLastAction()
        {
             // Simplified Undo for now to match interface
             return false;
        }

        public List<Transaction> GetAllTransactions(bool includeHidden = false)
        {
            try
            {
                return _transactionRepo.GetAllTransactions(includeHidden);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách giao dịch: " + ex.Message);
            }
        }

        public Transaction GetTransactionById(int transactionId)
        {
            try
            {
                return _transactionRepo.GetTransactionById(transactionId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy giao dịch ID {transactionId}: " + ex.Message);
            }
        }

        public List<Actions> GetAllLogs()
        {
            try
            {
                return _logRepo.GetAllLogs();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách nhật ký: " + ex.Message);
            }
        }

        public bool HideTransaction(int transactionId)
        {
            try
            {
                return _transactionRepo.SoftDeleteTransaction(transactionId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi ẩn giao dịch: " + ex.Message);
            }
        }
        public bool ApproveTransaction(int transactionId)
        {
            try
            {
                var transaction = _transactionRepo.GetTransactionById(transactionId);
                if (transaction == null) throw new Exception("Giao dịch không tồn tại");
                if (transaction.Status != "Pending") throw new Exception("Chỉ có thể duyệt giao dịch đang chờ");

                // Execute Stock Updates
                foreach (var detail in transaction.Details)
                {
                    var product = _productRepo.GetProductById(detail.ProductID);
                    if (product == null) throw new Exception($"Sản phẩm ID {detail.ProductID} không tồn tại");

                    if (transaction.Type == "Import")
                    {
                        int newQty = product.Quantity + detail.Quantity;
                         _productRepo.UpdateQuantity(detail.ProductID, newQty);
                    }
                    else if (transaction.Type == "Export")
                    {
                        if (product.Quantity < detail.Quantity)
                            throw new Exception($"Tồn kho không đủ cho sản phẩm {product.ProductName}");
                        
                        int newQty = product.Quantity - detail.Quantity;
                        _productRepo.UpdateQuantity(detail.ProductID, newQty);
                    }
                }

                _transactionRepo.UpdateTransactionStatus(transactionId, "Approved");
                
                _logRepo.LogAction("APPROVE_TRANSACTION", 
                    $"Duyệt giao dịch #{transactionId} ({transaction.Type})", "");
                
                ActionsService.Instance.MarkAsChanged();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi duyệt giao dịch: " + ex.Message);
            }
        }

        public bool CancelTransaction(int transactionId)
        {
            try
            {
                var transaction = _transactionRepo.GetTransactionById(transactionId);
                if (transaction == null) throw new Exception("Giao dịch không tồn tại");
                if (transaction.Status != "Pending") throw new Exception("Chỉ có thể hủy giao dịch đang chờ");

                _transactionRepo.UpdateTransactionStatus(transactionId, "Cancelled");
                
                _logRepo.LogAction("CANCEL_TRANSACTION", 
                    $"Hủy giao dịch #{transactionId}", "");

                ActionsService.Instance.MarkAsChanged();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi hủy giao dịch: " + ex.Message);
            }
        }
    }
}