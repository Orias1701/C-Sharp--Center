using System;
using System.Collections.Generic;
using WarehouseManagement.Services;
using WarehouseManagement.Models;

namespace WarehouseManagement.Controllers
{
    /// <summary>
    /// Controller Ä‘iá»u hÆ°á»›ng cÃ¡c thao tÃ¡c liÃªn quan Ä‘áº¿n kho (Nháº­p/Xuáº¥t)
    /// </summary>
    public class InventoryController
    {
        private readonly InventoryService _inventoryService;

        public InventoryController()
        {
            _inventoryService = new InventoryService();
        }

        /// <summary>
        /// Thá»±c hiá»‡n phiáº¿u nháº­p kho
        /// </summary>
        public bool Import(int productId, int quantity, decimal unitPrice, string note = "")
        {
            return _inventoryService.ImportStock(productId, quantity, unitPrice, note);
        }

        /// <summary>
        /// Thá»±c hiá»‡n phiáº¿u xuáº¥t kho
        /// </summary>
        public bool Export(int productId, int quantity, decimal unitPrice, string note = "")
        {
            return _inventoryService.ExportStock(productId, quantity, unitPrice, note);
        }

        /// <summary>
        /// Thá»±c hiá»‡n phiáº¿u nháº­p kho batch (nhiá»u sáº£n pháº©m, 1 transaction)
        /// </summary>
        public bool ImportBatch(List<(int ProductId, int Quantity, decimal UnitPrice)> details, string note = "")
        {
            return _inventoryService.ImportStockBatch(details, note);
        }

        /// <summary>
        /// Thá»±c hiá»‡n phiáº¿u xuáº¥t kho batch (nhiá»u sáº£n pháº©m, 1 transaction)
        /// </summary>
        public bool ExportBatch(List<(int ProductId, int Quantity, decimal UnitPrice)> details, string note = "")
        {
            return _inventoryService.ExportStockBatch(details, note);
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch sáº£n pháº©m cáº£nh bÃ¡o (tá»“n kho tháº¥p)
        /// </summary>
        public List<Product> GetLowStockProducts()
        {
            return _inventoryService.GetLowStockProducts();
        }

        /// <summary>
        /// TÃ­nh tá»•ng giÃ¡ trá»‹ tá»“n kho
        /// </summary>
        public decimal GetTotalInventoryValue()
        {
            return _inventoryService.GetTotalInventoryValue();
        }

        /// <summary>
        /// HoÃ n tÃ¡c thao tÃ¡c cuá»‘i cÃ¹ng
        /// </summary>
        public bool UndoLastAction()
        {
            return _inventoryService.UndoLastAction();
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ giao dá»‹ch
        /// </summary>
        public List<StockTransaction> GetAllTransactions()
        {
            return _inventoryService.GetAllTransactions();
        }

        /// <summary>
        /// Láº¥y giao dá»‹ch theo ID (bao gá»“m chi tiáº¿t)
        /// </summary>
        public StockTransaction GetTransactionById(int transactionId)
        {
            return _inventoryService.GetTransactionById(transactionId);
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch nháº­t kÃ½ hÃ nh Ä‘á»™ng
        /// </summary>
        public List<Actions> GetAllLogs()
        {
            return _inventoryService.GetAllLogs();
        }
    }
}





