using System;
using System.Collections.Generic;
using WarehouseManagement.Services;
using WarehouseManagement.Models;

namespace WarehouseManagement.Controllers
{
    public class InventoryController
    {
        private readonly InventoryService _inventoryService;

        public InventoryController()
        {
            _inventoryService = new InventoryService();
        }

        public bool Import(int productId, int quantity, decimal unitPrice, string note = "")
        {
            return _inventoryService.ImportStock(productId, quantity, unitPrice, note);
        }

        public bool Export(int productId, int quantity, decimal unitPrice, string note = "")
        {
            return _inventoryService.ExportStock(productId, quantity, unitPrice, note);
        }

        public int ImportBatch(List<(int ProductId, int Quantity, decimal UnitPrice, double DiscountRate)> details, string note = "", int supplierId = 0)
        {
            return _inventoryService.ImportStockBatch(details, note, supplierId);
        }

        public int ExportBatch(List<(int ProductId, int Quantity, decimal UnitPrice, double DiscountRate)> details, string note = "", int customerId = 0)
        {
            return _inventoryService.ExportStockBatch(details, note, customerId);
        }

        public List<Product> GetLowStockProducts()
        {
            return _inventoryService.GetLowStockProducts();
        }

        public decimal GetTotalInventoryValue()
        {
            return _inventoryService.GetTotalInventoryValue();
        }

        public bool UndoLastAction()
        {
            return _inventoryService.UndoLastAction();
        }

        public List<Transaction> GetAllTransactions(bool includeHidden = false)
        {
            return _inventoryService.GetAllTransactions(includeHidden);
        }

        public Transaction GetTransactionById(int transactionId)
        {
            return _inventoryService.GetTransactionById(transactionId);
        }

        public List<Actions> GetAllLogs()
        {
            return _inventoryService.GetAllLogs();
        }

        public bool HideTransaction(int transactionId)
        {
            return _inventoryService.HideTransaction(transactionId);
        }

        public bool ApproveTransaction(int transactionId)
        {
            return _inventoryService.ApproveTransaction(transactionId);
        }

        public bool CancelTransaction(int transactionId)
        {
            return _inventoryService.CancelTransaction(transactionId);
        }
    }
}