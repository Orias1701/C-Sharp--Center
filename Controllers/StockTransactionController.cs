using System;
using System.Collections.Generic;
using WarehouseManagement.Services;
using WarehouseManagement.Models;

namespace WarehouseManagement.Controllers
{
    public class StockTransactionController
    {
        private readonly StockTransactionService _transactionService;

        public StockTransactionController()
        {
            _transactionService = new StockTransactionService();
        }

        public List<Transaction> GetAllTransactions()
        {
            return _transactionService.GetAllTransactions();
        }

        public Transaction GetTransactionById(int transactionId)
        {
            return _transactionService.GetTransactionById(transactionId);
        }

        public int CreateTransaction(string type, string note = "")
        {
            return _transactionService.CreateTransaction(type, note);
        }

        public bool UpdateTransaction(int transactionId, string type, string note)
        {
            return _transactionService.UpdateTransaction(transactionId, type, note);
        }

        public bool DeleteTransaction(int transactionId)
        {
            return _transactionService.DeleteTransaction(transactionId);
        }

        public List<Transaction> GetTransactionsByType(string type)
        {
            return _transactionService.GetTransactionsByType(type);
        }

        public List<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _transactionService.GetTransactionsByDateRange(startDate, endDate);
        }

        public decimal GetTransactionTotalValue(int transactionId)
        {
            return _transactionService.GetTransactionTotalValue(transactionId);
        }

        public int CountTransactions()
        {
            return _transactionService.CountTransactions();
        }
    }
}