using System;
using System.Collections.Generic;
using WarehouseManagement.Services;
using WarehouseManagement.Models;

namespace WarehouseManagement.Controllers
{
    /// <summary>
    /// Controller Ä‘iá»u hÆ°á»›ng cÃ¡c thao tÃ¡c liÃªn quan Ä‘áº¿n phiáº¿u Nháº­p/Xuáº¥t kho
    /// </summary>
    public class StockTransactionController
    {
        private readonly StockTransactionService _transactionService;

        public StockTransactionController()
        {
            _transactionService = new StockTransactionService();
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ phiáº¿u
        /// </summary>
        public List<StockTransaction> GetAllTransactions()
        {
            return _transactionService.GetAllTransactions();
        }

        /// <summary>
        /// Láº¥y phiáº¿u theo ID (bao gá»“m chi tiáº¿t)
        /// </summary>
        public StockTransaction GetTransactionById(int transactionId)
        {
            return _transactionService.GetTransactionById(transactionId);
        }

        /// <summary>
        /// Táº¡o phiáº¿u nháº­p/xuáº¥t má»›i
        /// </summary>
        public int CreateTransaction(string type, string note = "")
        {
            return _transactionService.CreateTransaction(type, note);
        }

        /// <summary>
        /// Cáº­p nháº­t phiáº¿u
        /// </summary>
        public bool UpdateTransaction(int transactionId, string type, string note)
        {
            return _transactionService.UpdateTransaction(transactionId, type, note);
        }

        /// <summary>
        /// XÃ³a phiáº¿u
        /// </summary>
        public bool DeleteTransaction(int transactionId)
        {
            return _transactionService.DeleteTransaction(transactionId);
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch phiáº¿u theo loáº¡i (Import/Export)
        /// </summary>
        public List<StockTransaction> GetTransactionsByType(string type)
        {
            return _transactionService.GetTransactionsByType(type);
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch phiáº¿u trong má»™t khoáº£ng thá»i gian
        /// </summary>
        public List<StockTransaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _transactionService.GetTransactionsByDateRange(startDate, endDate);
        }

        /// <summary>
        /// TÃ­nh tá»•ng giÃ¡ trá»‹ má»™t phiáº¿u
        /// </summary>
        public decimal GetTransactionTotalValue(int transactionId)
        {
            return _transactionService.GetTransactionTotalValue(transactionId);
        }

        /// <summary>
        /// Äáº¿m tá»•ng sá»‘ phiáº¿u
        /// </summary>
        public int CountTransactions()
        {
            return _transactionService.CountTransactions();
        }
    }
}




