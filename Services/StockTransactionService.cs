using System;
using System.Collections.Generic;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;
using Newtonsoft.Json;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xá»­ lÃ½ logic phiáº¿u Nháº­p/Xuáº¥t kho
    /// 
    /// CHá»¨C NÄ‚NG:
    /// - Quáº£n lÃ½ phiáº¿u (CRUD): ThÃªm, sá»­a, xÃ³a
    /// - TÃ¬m kiáº¿m phiáº¿u: Theo loáº¡i, ngÃ y thÃ¡ng
    /// - TÃ­nh toÃ¡n: TÃ­nh tá»•ng giÃ¡ trá»‹, tá»•ng sá»‘ lÆ°á»£ng
    /// 
    /// LUá»’NG:
    /// 1. Validation: Kiá»ƒm tra Ä‘áº§u vÃ o
    /// 2. Repository call: Gá»i DB Ä‘á»ƒ thá»±c hiá»‡n thao tÃ¡c
    /// 3. Logging: Ghi nháº­t kÃ½ Actions
    /// 4. Change tracking: Gá»i ActionsService.MarkAsChanged()
    /// 5. Return: Tráº£ vá» káº¿t quáº£
    /// </summary>
    public class StockTransactionService
    {
        private readonly StockTransactionRepository _transactionRepo;
        private readonly ActionsRepository _logRepo;

        public StockTransactionService()
        {
            _transactionRepo = new StockTransactionRepository();
            _logRepo = new ActionsRepository();
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ phiáº¿u
        /// </summary>
        public List<StockTransaction> GetAllTransactions()
        {
            try
            {
                return _transactionRepo.GetAllTransactions();
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh sÃ¡ch phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y phiáº¿u theo ID (bao gá»“m chi tiáº¿t)
        /// </summary>
        public StockTransaction GetTransactionById(int transactionId)
        {
            try
            {
                if (transactionId <= 0)
                    throw new ArgumentException("ID phiáº¿u khÃ´ng há»£p lá»‡");
                return _transactionRepo.GetTransactionById(transactionId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lá»—i khi láº¥y phiáº¿u ID {transactionId}: " + ex.Message);
            }
        }

        /// <summary>
        /// Táº¡o phiáº¿u nháº­p/xuáº¥t má»›i
        /// </summary>
        public int CreateTransaction(string type, string note = "")
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(type))
                    throw new ArgumentException("Loáº¡i phiáº¿u khÃ´ng Ä‘Æ°á»£c trá»‘ng");
                if (type != "Import" && type != "Export")
                    throw new ArgumentException("Loáº¡i phiáº¿u pháº£i lÃ  Import hoáº·c Export");

                var transaction = new StockTransaction
                {
                    Type = type.Trim(),
                    DateCreated = DateTime.Now,
                    CreatedByUserID = GlobalUser.CurrentUser?.UserID ?? 0,
                    Note = note ?? ""
                };

                int transId = _transactionRepo.CreateTransaction(transaction);

                // Ghi nháº­t kÃ½
                var log = new Actions
                {
                    ActionType = "CREATE_TRANSACTION",
                    Descriptions = $"Táº¡o phiáº¿u {type}: {note}",
                    DataBefore = "",
                    CreatedAt = DateTime.Now
                };
                _logRepo.LogAction(log);

                ActionsService.Instance.MarkAsChanged();
                return transId;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi táº¡o phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t phiáº¿u
        /// </summary>
        public bool UpdateTransaction(int transactionId, string type, string note)
        {
            try
            {
                if (transactionId <= 0)
                    throw new ArgumentException("ID phiáº¿u khÃ´ng há»£p lá»‡");
                if (string.IsNullOrWhiteSpace(type))
                    throw new ArgumentException("Loáº¡i phiáº¿u khÃ´ng Ä‘Æ°á»£c trá»‘ng");
                if (type != "Import" && type != "Export")
                    throw new ArgumentException("Loáº¡i phiáº¿u pháº£i lÃ  Import hoáº·c Export");

                var oldTransaction = _transactionRepo.GetTransactionById(transactionId);
                if (oldTransaction == null)
                    throw new ArgumentException("Phiáº¿u khÃ´ng tá»“n táº¡i");

                var beforeData = new
                {
                    TransactionID = oldTransaction.TransactionID,
                    Type = oldTransaction.Type,
                    Note = oldTransaction.Note
                };

                var transaction = new StockTransaction
                {
                    TransactionID = transactionId,
                    Type = type.Trim(),
                    Note = note ?? ""
                };

                bool result = _transactionRepo.UpdateTransaction(transaction);

                if (result)
                {
                    var log = new Actions
                    {
                        ActionType = "UPDATE_TRANSACTION",
                        Descriptions = $"Cáº­p nháº­t phiáº¿u ID {transactionId}",
                        DataBefore = JsonConvert.SerializeObject(beforeData),
                        CreatedAt = DateTime.Now
                    };
                    _logRepo.LogAction(log);

                    ActionsService.Instance.MarkAsChanged();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi cáº­p nháº­t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a phiáº¿u
        /// </summary>
        public bool DeleteTransaction(int transactionId)
        {
            try
            {
                if (transactionId <= 0)
                    throw new ArgumentException("ID phiáº¿u khÃ´ng há»£p lá»‡");

                var transaction = _transactionRepo.GetTransactionById(transactionId);
                if (transaction == null)
                    throw new ArgumentException("Phiáº¿u khÃ´ng tá»“n táº¡i");

                var beforeData = new
                {
                    TransactionID = transaction.TransactionID,
                    Type = transaction.Type,
                    DateCreated = transaction.DateCreated,
                    Note = transaction.Note,
                    DetailCount = transaction.Details.Count
                };

                bool result = _transactionRepo.DeleteTransaction(transactionId);

                if (result)
                {
                    var log = new Actions
                    {
                        ActionType = "DELETE_TRANSACTION",
                        Descriptions = $"XÃ³a phiáº¿u ID {transactionId}",
                        DataBefore = JsonConvert.SerializeObject(beforeData),
                        CreatedAt = DateTime.Now
                    };
                    _logRepo.LogAction(log);

                    ActionsService.Instance.MarkAsChanged();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch phiáº¿u theo loáº¡i (Import/Export)
        /// </summary>
        public List<StockTransaction> GetTransactionsByType(string type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(type))
                    throw new ArgumentException("Loáº¡i phiáº¿u khÃ´ng Ä‘Æ°á»£c trá»‘ng");
                if (type != "Import" && type != "Export")
                    throw new ArgumentException("Loáº¡i phiáº¿u pháº£i lÃ  Import hoáº·c Export");

                return _transactionRepo.GetTransactionsByType(type.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y phiáº¿u theo loáº¡i: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch phiáº¿u trong má»™t khoáº£ng thá»i gian
        /// </summary>
        public List<StockTransaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    throw new ArgumentException("NgÃ y báº¯t Ä‘áº§u khÃ´ng Ä‘Æ°á»£c lá»›n hÆ¡n ngÃ y káº¿t thÃºc");

                return _transactionRepo.GetTransactionsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y phiáº¿u theo ngÃ y: " + ex.Message);
            }
        }

        /// <summary>
        /// TÃ­nh tá»•ng giÃ¡ trá»‹ má»™t phiáº¿u
        /// </summary>
        public decimal GetTransactionTotalValue(int transactionId)
        {
            try
            {
                if (transactionId <= 0)
                    throw new ArgumentException("ID phiáº¿u khÃ´ng há»£p lá»‡");

                var transaction = _transactionRepo.GetTransactionById(transactionId);
                if (transaction == null)
                    throw new ArgumentException("Phiáº¿u khÃ´ng tá»“n táº¡i");

                decimal total = 0;
                foreach (var detail in transaction.Details)
                {
                    total += detail.Quantity * detail.UnitPrice;
                }
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi tÃ­nh tá»•ng giÃ¡ trá»‹ phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Äáº¿m tá»•ng sá»‘ phiáº¿u
        /// </summary>
        public int CountTransactions()
        {
            try
            {
                return _transactionRepo.GetAllTransactions().Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi Ä‘áº¿m phiáº¿u: " + ex.Message);
            }
        }
    }
}




