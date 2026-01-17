using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;
using Newtonsoft.Json;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xá»­ lÃ½ logic Nháº­p/Xuáº¥t kho
    /// 
    /// CHá»¨C NÄ‚NG:
    /// - Nháº­p kho (ImportStock): Má»™t phiáº¿u nháº­p, má»™t sáº£n pháº©m
    /// - Xuáº¥t kho (ExportStock): Má»™t phiáº¿u xuáº¥t, má»™t sáº£n pháº©m
    /// - Nháº­p batch (ImportStockBatch): Má»™t phiáº¿u nháº­p, nhiá»u sáº£n pháº©m
    /// - Xuáº¥t batch (ExportStockBatch): Má»™t phiáº¿u xuáº¥t, nhiá»u sáº£n pháº©m
    /// - TÃ­nh toÃ¡n tá»“n kho: Tá»± Ä‘á»™ng cáº­p nháº­t sá»‘ lÆ°á»£ng
    /// 
    /// LUá»’NG:
    /// 1. Validation: Kiá»ƒm tra sáº£n pháº©m, sá»‘ lÆ°á»£ng, giÃ¡
    /// 2. CreateTransaction(): Táº¡o phiáº¿u nháº­p/xuáº¥t
    /// 3. AddTransactionDetail(): ThÃªm chi tiáº¿t phiáº¿u
    /// 4. UpdateQuantity(): Cáº­p nháº­t tá»“n kho
    /// 5. LogAction(): Ghi nháº­t kÃ½
    /// 6. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
    /// 7. Return: Tráº£ vá» káº¿t quáº£
    /// </summary>
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

        // ========== SINGLE IMPORT/EXPORT ==========

        /// <summary>
        /// Thá»±c hiá»‡n phiáº¿u nháº­p kho (má»™t sáº£n pháº©m)
        /// 
        /// LUá»’NG:
        /// 1. Validation: Kiá»ƒm tra sáº£n pháº©m, sá»‘ lÆ°á»£ng, giÃ¡, khÃ´ng vÆ°á»£t giá»›i háº¡n
        /// 2. CreateTransaction(): Táº¡o phiáº¿u nháº­p
        /// 3. AddTransactionDetail(): ThÃªm chi tiáº¿t phiáº¿u
        /// 4. UpdateQuantity(): Cáº­p nháº­t tá»“n kho = cÅ© + sá»‘ lÆ°á»£ng nháº­p
        /// 5. LogAction(): Ghi nháº­t kÃ½
        /// 6. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
        /// 7. Return: true náº¿u thÃ nh cÃ´ng
        /// </summary>
        public bool ImportStock(int productId, int quantity, decimal unitPrice, string note = "")
        {
            try
            {
                // Validation cÃ¡c trÆ°á»ng Ä‘áº§u vÃ o
                if (productId <= 0)
                    throw new ArgumentException("ID sáº£n pháº©m khÃ´ng há»£p lá»‡");
                if (quantity <= 0)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng nháº­p pháº£i lá»›n hÆ¡n 0");
                if (quantity > 999999)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng quÃ¡ lá»›n");
                if (unitPrice < 0)
                    throw new ArgumentException("ÄÆ¡n giÃ¡ khÃ´ng Ä‘Æ°á»£c Ã¢m");
                if (unitPrice > 999999999)
                    throw new ArgumentException("ÄÆ¡n giÃ¡ quÃ¡ lá»›n");

                var product = _productRepo.GetProductById(productId);
                if (product == null)
                    throw new ArgumentException("Sáº£n pháº©m khÃ´ng tá»“n táº¡i");

                // LÆ°u dá»¯ liá»‡u cÅ© trÆ°á»›c khi thay Ä‘á»•i (Ä‘á»ƒ ghi nháº­t kÃ½)
                var oldData = new { product.Quantity, product.ProductID };
                
                // Táº¡o phiáº¿u
                var transaction = new StockTransaction
                {
                    Type = "Import",
                    DateCreated = DateTime.Now,
                    CreatedByUserID = GlobalUser.CurrentUser?.UserID ?? 0,
                    Note = string.IsNullOrWhiteSpace(note) ? "" : note.Trim()
                };
                int transId = _transactionRepo.CreateTransaction(transaction);

                // ThÃªm chi tiáº¿t
                var detail = new TransactionDetail
                {
                    TransactionID = transId,
                    ProductID = productId,
                    ProductName = product.ProductName,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };
                _transactionRepo.AddTransactionDetail(detail);

                // Cáº­p nháº­t tá»“n kho
                int newQuantity = product.Quantity + quantity;
                if (newQuantity > 999999)
                    throw new Exception("Tá»“n kho sáº½ vÆ°á»£t quÃ¡ giá»›i háº¡n cho phÃ©p");

                _productRepo.UpdateQuantity(productId, newQuantity);

                // Ghi nháº­t kÃ½
                var newData = new { Quantity = newQuantity, ProductID = productId };
                _logRepo.LogAction("IMPORT_STOCK", 
                    $"Nháº­p {quantity} sáº£n pháº©m ID {productId}",
                    JsonConvert.SerializeObject(oldData));

                ActionsService.Instance.MarkAsChanged();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi nháº­p kho: " + ex.Message);
            }
        }

        /// <summary>
        /// Thá»±c hiá»‡n phiáº¿u xuáº¥t kho (má»™t sáº£n pháº©m)
        /// 
        /// LUá»’NG:
        /// 1. Validation: Kiá»ƒm tra sáº£n pháº©m, sá»‘ lÆ°á»£ng, giÃ¡
        /// 2. Check: Kiá»ƒm tra tá»“n kho cÃ³ Ä‘á»§ Ä‘á»ƒ xuáº¥t khÃ´ng
        /// 3. CreateTransaction(): Táº¡o phiáº¿u xuáº¥t
        /// 4. AddTransactionDetail(): ThÃªm chi tiáº¿t phiáº¿u
        /// 5. UpdateQuantity(): Cáº­p nháº­t tá»“n kho = cÅ© - sá»‘ lÆ°á»£ng xuáº¥t
        /// 6. LogAction(): Ghi nháº­t kÃ½
        /// 7. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
        /// 8. Return: true náº¿u thÃ nh cÃ´ng
        /// </summary>
        public bool ExportStock(int productId, int quantity, decimal unitPrice, string note = "")
        {
            try
            {
                // Validation cÃ¡c trÆ°á»ng Ä‘áº§u vÃ o
                if (productId <= 0)
                    throw new ArgumentException("ID sáº£n pháº©m khÃ´ng há»£p lá»‡");
                if (quantity <= 0)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng xuáº¥t pháº£i lá»›n hÆ¡n 0");
                if (quantity > 999999)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng quÃ¡ lá»›n");
                if (unitPrice < 0)
                    throw new ArgumentException("ÄÆ¡n giÃ¡ khÃ´ng Ä‘Æ°á»£c Ã¢m");
                if (unitPrice > 999999999)
                    throw new ArgumentException("ÄÆ¡n giÃ¡ quÃ¡ lá»›n");

                var product = _productRepo.GetProductById(productId);
                if (product == null)
                    throw new ArgumentException("Sáº£n pháº©m khÃ´ng tá»“n táº¡i");

                if (product.Quantity < quantity)
                    throw new Exception("Tá»“n kho khÃ´ng Ä‘á»§ Ä‘á»ƒ xuáº¥t (hiá»‡n cÃ³: " + product.Quantity + ")");

                // LÆ°u dá»¯ liá»‡u cÅ©
                var oldData = new { product.Quantity, product.ProductID };

                // Táº¡o phiáº¿u
                var transaction = new StockTransaction
                {
                    Type = "Export",
                    DateCreated = DateTime.Now,
                    CreatedByUserID = GlobalUser.CurrentUser?.UserID ?? 0,
                    Note = string.IsNullOrWhiteSpace(note) ? "" : note.Trim()
                };
                int transId = _transactionRepo.CreateTransaction(transaction);

                // ThÃªm chi tiáº¿t
                var detail = new TransactionDetail
                {
                    TransactionID = transId,
                    ProductID = productId,
                    ProductName = product.ProductName,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };
                _transactionRepo.AddTransactionDetail(detail);

                // Cáº­p nháº­t tá»“n kho
                int newQuantity = product.Quantity - quantity;
                _productRepo.UpdateQuantity(productId, newQuantity);

                // Ghi nháº­t kÃ½
                _logRepo.LogAction("EXPORT_STOCK",
                    $"Xuáº¥t {quantity} sáº£n pháº©m ID {productId}",
                    JsonConvert.SerializeObject(oldData));

                ActionsService.Instance.MarkAsChanged();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xuáº¥t kho: " + ex.Message);
            }
        }

        // ========== BATCH IMPORT/EXPORT ==========

        /// <summary>
        /// Thá»±c hiá»‡n phiáº¿u nháº­p kho batch (nhiá»u sáº£n pháº©m, 1 phiáº¿u)
        /// 
        /// LUá»’NG:
        /// 1. Validation: Kiá»ƒm tra danh sÃ¡ch khÃ´ng trá»‘ng, khÃ´ng trÃ¹ng láº·p ID
        /// 2. CreateTransaction(): Táº¡o 1 phiáº¿u nháº­p chung
        /// 3. Loop tá»«ng sáº£n pháº©m:
        ///    - Validation: Kiá»ƒm tra sáº£n pháº©m, sá»‘ lÆ°á»£ng, giÃ¡
        ///    - AddTransactionDetail(): ThÃªm chi tiáº¿t cho sáº£n pháº©m nÃ y
        ///    - UpdateQuantity(): Cáº­p nháº­t tá»“n kho += sá»‘ lÆ°á»£ng
        /// 4. LogAction(): Ghi nháº­t kÃ½ 1 láº§n cho cáº£ batch
        /// 5. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
        /// 6. Return: true náº¿u thÃ nh cÃ´ng
        /// </summary>
        public bool ImportStockBatch(List<(int ProductId, int Quantity, decimal UnitPrice)> details, string note = "")
        {
            try
            {
                if (details == null || details.Count == 0)
                    throw new ArgumentException("Danh sÃ¡ch sáº£n pháº©m khÃ´ng thá»ƒ rá»—ng");

                // Kiá»ƒm tra trÃ¹ng láº·p ID trong list vÃ  kiá»ƒm tra sáº£n pháº©m tá»“n táº¡i trÆ°á»›c khi xá»­ lÃ½
                var productIds = new List<int>();
                foreach (var (productId, quantity, unitPrice) in details)
                {
                    if (productIds.Contains(productId))
                    {
                        throw new ArgumentException($"Sáº£n pháº©m ID {productId} bá»‹ trÃ¹ng láº·p trong phiáº¿u nháº­p");
                    }
                    
                    if (!_productRepo.ProductIdExists(productId))
                    {
                        throw new ArgumentException($"Sáº£n pháº©m ID {productId} khÃ´ng tá»“n táº¡i trong há»‡ thá»‘ng");
                    }
                    
                    productIds.Add(productId);
                }

                // Táº¡o transaction
                var transaction = new StockTransaction
                {
                    Type = "Import",
                    DateCreated = DateTime.Now,
                    CreatedByUserID = GlobalUser.CurrentUser?.UserID ?? 0,
                    Note = string.IsNullOrWhiteSpace(note) ? "" : note.Trim()
                };
                int transId = _transactionRepo.CreateTransaction(transaction);

                // Xá»­ lÃ½ tá»«ng sáº£n pháº©m
                foreach (var (productId, quantity, unitPrice) in details)
                {
                    // Validation
                    if (productId <= 0)
                        throw new ArgumentException("ID sáº£n pháº©m khÃ´ng há»£p lá»‡");
                    if (quantity <= 0)
                        throw new ArgumentException("Sá»‘ lÆ°á»£ng nháº­p pháº£i lá»›n hÆ¡n 0");
                    if (quantity > 999999)
                        throw new ArgumentException("Sá»‘ lÆ°á»£ng quÃ¡ lá»›n");
                    if (unitPrice < 0)
                        throw new ArgumentException("ÄÆ¡n giÃ¡ khÃ´ng Ä‘Æ°á»£c Ã¢m");
                    if (unitPrice > 999999999)
                        throw new ArgumentException("ÄÆ¡n giÃ¡ quÃ¡ lá»›n");

                    var product = _productRepo.GetProductById(productId);
                    if (product == null)
                        throw new ArgumentException($"Sáº£n pháº©m ID {productId} khÃ´ng tá»“n táº¡i");

                    // ThÃªm chi tiáº¿t
                    var detail = new TransactionDetail
                    {
                        TransactionID = transId,
                        ProductID = productId,
                        ProductName = product.ProductName,
                        Quantity = quantity,
                        UnitPrice = unitPrice
                    };
                    _transactionRepo.AddTransactionDetail(detail);

                    // Cáº­p nháº­t tá»“n kho
                    int newQuantity = product.Quantity + quantity;
                    if (newQuantity > 999999)
                        throw new Exception("Tá»“n kho sáº½ vÆ°á»£t quÃ¡ giá»›i háº¡n cho phÃ©p");

                    _productRepo.UpdateQuantity(productId, newQuantity);
                }

                // Cáº­p nháº­t tá»•ng giÃ¡ trá»‹ cá»§a phiáº¿u sau khi thÃªm táº¥t cáº£ chi tiáº¿t
                _transactionRepo.UpdateTransactionTotalValue(transId);

                // Ghi nháº­t kÃ½
                _logRepo.LogAction("IMPORT_BATCH", 
                    $"Nháº­p {details.Count} sáº£n pháº©m, Transaction ID {transId}",
                    "");

                ActionsService.Instance.MarkAsChanged();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi nháº­p kho batch: " + ex.Message);
            }
        }

        /// <summary>
        /// Thá»±c hiá»‡n phiáº¿u xuáº¥t kho batch (nhiá»u sáº£n pháº©m, 1 phiáº¿u)
        /// 
        /// LUá»’NG:
        /// 1. Validation: Kiá»ƒm tra danh sÃ¡ch khÃ´ng trá»‘ng, khÃ´ng trÃ¹ng láº·p ID
        /// 2. Check: Kiá»ƒm tra tá»“n kho cá»§a táº¥t cáº£ sáº£n pháº©m cÃ³ Ä‘á»§ Ä‘á»ƒ xuáº¥t khÃ´ng
        /// 3. CreateTransaction(): Táº¡o 1 phiáº¿u xuáº¥t chung
        /// 4. Loop tá»«ng sáº£n pháº©m:
        ///    - Validation: Kiá»ƒm tra sáº£n pháº©m, sá»‘ lÆ°á»£ng, giÃ¡
        ///    - AddTransactionDetail(): ThÃªm chi tiáº¿t cho sáº£n pháº©m nÃ y
        ///    - UpdateQuantity(): Cáº­p nháº­t tá»“n kho -= sá»‘ lÆ°á»£ng
        /// 5. LogAction(): Ghi nháº­t kÃ½ 1 láº§n cho cáº£ batch
        /// 6. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
        /// 7. Return: true náº¿u thÃ nh cÃ´ng
        /// </summary>
        public bool ExportStockBatch(List<(int ProductId, int Quantity, decimal UnitPrice)> details, string note = "")
        {
            try
            {
                if (details == null || details.Count == 0)
                    throw new ArgumentException("Danh sÃ¡ch sáº£n pháº©m khÃ´ng thá»ƒ rá»—ng");

                // Kiá»ƒm tra trÃ¹ng láº·p ID trong list
                var productIds = new List<int>();
                foreach (var (productId, quantity, unitPrice) in details)
                {
                    if (productIds.Contains(productId))
                    {
                        throw new ArgumentException($"Sáº£n pháº©m ID {productId} bá»‹ trÃ¹ng láº·p trong phiáº¿u xuáº¥t");
                    }
                    productIds.Add(productId);
                }

                // Kiá»ƒm tra tá»“n kho trÆ°á»›c
                foreach (var (productId, quantity, unitPrice) in details)
                {
                    if (!_productRepo.ProductIdExists(productId))
                    {
                        throw new ArgumentException($"Sáº£n pháº©m ID {productId} khÃ´ng tá»“n táº¡i trong há»‡ thá»‘ng");
                    }

                    var product = _productRepo.GetProductById(productId);
                    if (product == null)
                        throw new ArgumentException($"Sáº£n pháº©m ID {productId} khÃ´ng tá»“n táº¡i");
                    if (product.Quantity < quantity)
                        throw new Exception($"Tá»“n kho {product.ProductName} khÃ´ng Ä‘á»§ (hiá»‡n cÃ³: {product.Quantity}, cáº§n xuáº¥t: {quantity})");
                }

                // Táº¡o transaction
                var transaction = new StockTransaction
                {
                    Type = "Export",
                    DateCreated = DateTime.Now,
                    CreatedByUserID = GlobalUser.CurrentUser?.UserID ?? 0,
                    Note = string.IsNullOrWhiteSpace(note) ? "" : note.Trim()
                };
                int transId = _transactionRepo.CreateTransaction(transaction);

                // Xá»­ lÃ½ tá»«ng sáº£n pháº©m
                foreach (var (productId, quantity, unitPrice) in details)
                {
                    // Validation
                    if (productId <= 0)
                        throw new ArgumentException("ID sáº£n pháº©m khÃ´ng há»£p lá»‡");
                    if (quantity <= 0)
                        throw new ArgumentException("Sá»‘ lÆ°á»£ng xuáº¥t pháº£i lá»›n hÆ¡n 0");
                    if (quantity > 999999)
                        throw new ArgumentException("Sá»‘ lÆ°á»£ng quÃ¡ lá»›n");
                    if (unitPrice < 0)
                        throw new ArgumentException("ÄÆ¡n giÃ¡ khÃ´ng Ä‘Æ°á»£c Ã¢m");
                    if (unitPrice > 999999999)
                        throw new ArgumentException("ÄÆ¡n giÃ¡ quÃ¡ lá»›n");

                    var product = _productRepo.GetProductById(productId);
                    
                    // ThÃªm chi tiáº¿t
                    var detail = new TransactionDetail
                    {
                        TransactionID = transId,
                        ProductID = productId,
                        ProductName = product.ProductName,
                        Quantity = quantity,
                        UnitPrice = unitPrice
                    };
                    _transactionRepo.AddTransactionDetail(detail);

                    // Cáº­p nháº­t tá»“n kho
                    int newQuantity = product.Quantity - quantity;
                    _productRepo.UpdateQuantity(productId, newQuantity);
                }

                // Cáº­p nháº­t tá»•ng giÃ¡ trá»‹ cá»§a phiáº¿u sau khi thÃªm táº¥t cáº£ chi tiáº¿t
                _transactionRepo.UpdateTransactionTotalValue(transId);

                // Ghi nháº­t kÃ½
                _logRepo.LogAction("EXPORT_BATCH",
                    $"Xuáº¥t {details.Count} sáº£n pháº©m, Transaction ID {transId}",
                    "");
                ActionsService.Instance.MarkAsChanged();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xuáº¥t kho batch: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch sáº£n pháº©m cáº£nh bÃ¡o (tá»“n kho tháº¥p)
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
                throw new Exception("Lá»—i khi láº¥y sáº£n pháº©m cáº£nh bÃ¡o: " + ex.Message);
            }
        }

        /// <summary>
        /// TÃ­nh tá»•ng giÃ¡ trá»‹ tá»“n kho
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
                throw new Exception("Lá»—i khi tÃ­nh giÃ¡ trá»‹ tá»“n kho: " + ex.Message);
            }
        }

        /// <summary>
        /// HoÃ n tÃ¡c thao tÃ¡c cuá»‘i cÃ¹ng (dá»±a trÃªn nháº­t kÃ½)
        /// Há»— trá»£ tá»‘i Ä‘a 10 hÃ nh Ä‘á»™ng gáº§n nháº¥t dÃ¹ng cáº¥u trÃºc Stack (LIFO)
        /// HÃ nh Ä‘á»™ng Ä‘Æ°á»£c hoÃ n tÃ¡c sáº½ bá»‹ xÃ³a khá»i stack (set Visible=FALSE) Ä‘á»ƒ trÃ¡nh conflict
        /// </summary>
        public bool UndoLastAction()
        {
            try
            {
                // Get the last 10 actions (LIFO stack) - only Visible=TRUE entries
                var logs = _logRepo.GetLastNLogs(10);
                if (logs == null || logs.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("UndoLastAction: No logs available");
                    return false;
                }

                // Pop the first item (most recent action)
                var lastLog = logs.First();
                if (lastLog == null)
                {
                    System.Diagnostics.Debug.WriteLine("UndoLastAction: LastLog is null");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"UndoLastAction: Processing LogID={lastLog.LogID}, ActionType={lastLog.ActionType}");
                
                // Check if data is available
                if (string.IsNullOrWhiteSpace(lastLog.DataBefore))
                {
                    // For ADD actions, we can try to delete the record
                    if (lastLog.ActionType != null && lastLog.ActionType.StartsWith("ADD_"))
                    {
                        try
                        {
                            // Remove from undo stack after processing
                            if (_logRepo != null)
                            {
                                _logRepo.RemoveFromUndoStack(lastLog.LogID);
                                _logRepo.LogAction("UNDO_ACTION", $"HoÃ n tÃ¡c hÃ nh Ä‘á»™ng {lastLog.ActionType}");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Lá»—i khi xÃ³a hÃ nh Ä‘á»™ng khá»i stack: {ex.Message}");
                        }
                        return true;
                    }
                    return false;
                }

                try
                {
                    Newtonsoft.Json.Linq.JObject jsonObj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(lastLog.DataBefore);
                    
                    if (jsonObj == null)
                        return false;

                    bool undoSuccess = false;

                    // Handle different action types
                    if (string.IsNullOrWhiteSpace(lastLog.ActionType))
                        return false;

                    switch (lastLog.ActionType)
                    {
                        case "IMPORT_STOCK":
                        case "EXPORT_STOCK":
                            // Restore product quantity
                            if (jsonObj.ContainsKey("ProductID") && jsonObj.ContainsKey("Quantity"))
                            {
                                try
                                {
                                    int productId = (int)jsonObj["ProductID"];
                                    int oldQuantity = (int)jsonObj["Quantity"];
                                    _productRepo?.UpdateQuantity(productId, oldQuantity);
                                    undoSuccess = true;
                                }
                                catch { /* Suppress error */ }
                            }
                            break;

                        case "UPDATE_PRODUCT":
                            // Restore all product fields
                            if (jsonObj.ContainsKey("ProductID"))
                            {
                                try
                                {
                                    var product = new Product
                                    {
                                        ProductID = (int)jsonObj["ProductID"],
                                        ProductName = (string)jsonObj["ProductName"],
                                        CategoryID = (int)jsonObj["CategoryID"],
                                        Price = (decimal)jsonObj["Price"],
                                        Quantity = (int)jsonObj["Quantity"],
                                        MinThreshold = (int)jsonObj["MinThreshold"]
                                    };
                                    _productRepo?.UpdateProduct(product);
                                    undoSuccess = true;
                                }
                                catch { /* Suppress error */ }
                            }
                            break;

                        case "DELETE_PRODUCT":
                            // Restore deleted product
                            if (jsonObj.ContainsKey("ProductID"))
                            {
                                try
                                {
                                    int productId = (int)jsonObj["ProductID"];
                                    string productName = (string)jsonObj["ProductName"];
                                    int categoryId = (int)jsonObj["CategoryID"];
                                    decimal price = (decimal)jsonObj["Price"];
                                    int quantity = (int)jsonObj["Quantity"];
                                    int minThreshold = (int)jsonObj["MinThreshold"];
                                    
                                    // Restore by updating product visibility
                                    var product = new Product
                                    {
                                        ProductID = productId,
                                        ProductName = productName,
                                        CategoryID = categoryId,
                                        Price = price,
                                        Quantity = quantity,
                                        MinThreshold = minThreshold
                                    };
                                    _productRepo?.UpdateProduct(product);
                                    undoSuccess = true;
                                }
                                catch { /* Suppress error */ }
                            }
                            break;

                        case "UPDATE_CATEGORY":
                            // Restore category
                            if (jsonObj.ContainsKey("CategoryID"))
                            {
                                try
                                {
                                    int categoryId = (int)jsonObj["CategoryID"];
                                    string categoryName = (string)jsonObj["CategoryName"];
                                    _categoryRepo?.UpdateCategory(new Category { CategoryID = categoryId, CategoryName = categoryName });
                                    undoSuccess = true;
                                }
                                catch { /* Suppress error */ }
                            }
                            break;

                        case "DELETE_CATEGORY":
                            // Restore deleted category
                            if (jsonObj.ContainsKey("CategoryID"))
                            {
                                try
                                {
                                    int categoryId = (int)jsonObj["CategoryID"];
                                    string categoryName = (string)jsonObj["CategoryName"];
                                    // Create a restored category
                                    _categoryRepo?.RestoreDeletedCategory(categoryId, categoryName);
                                    undoSuccess = true;
                                }
                                catch { /* Suppress error */ }
                            }
                            break;

                        default:
                            // Unknown action type
                            return false;
                    }

                    // Remove from undo stack after successful undo
                    if (undoSuccess)
                    {
                        try
                        {
                            // Ensure the action is marked as processed
                            if (_logRepo != null)
                            {
                                bool removeSuccess = _logRepo.RemoveFromUndoStack(lastLog.LogID);
                                System.Diagnostics.Debug.WriteLine($"UndoLastAction: Removed LogID={lastLog.LogID} from stack, success={removeSuccess}");
                                
                                _logRepo.LogAction("UNDO_ACTION", $"HoÃ n tÃ¡c hÃ nh Ä‘á»™ng {lastLog.ActionType}");
                                System.Diagnostics.Debug.WriteLine($"UndoLastAction: Logged UNDO_ACTION for {lastLog.ActionType}");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Lá»—i khi xÃ³a hÃ nh Ä‘á»™ng khá»i stack: {ex.Message}\n{ex.StackTrace}");
                        }
                        return true;
                    }

                    return false;
                }
                catch (JsonException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lá»—i khi hoÃ n tÃ¡c: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ giao dá»‹ch
        /// </summary>
        public List<StockTransaction> GetAllTransactions()
        {
            try
            {
                return _transactionRepo.GetAllTransactions();
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh sÃ¡ch giao dá»‹ch: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y giao dá»‹ch theo ID (bao gá»“m chi tiáº¿t)
        /// </summary>
        public StockTransaction GetTransactionById(int transactionId)
        {
            try
            {
                var result = _transactionRepo.GetTransactionById(transactionId);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lá»—i khi láº¥y giao dá»‹ch ID {transactionId}: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch nháº­t kÃ½ hÃ nh Ä‘á»™ng
        /// </summary>
        public List<Actions> GetAllLogs()
        {
            try
            {
                return _logRepo.GetAllLogs();
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh sÃ¡ch nháº­t kÃ½: " + ex.Message);
            }
        }
    }
}





