using System;
using System.Collections.Generic;
using WarehouseManagement.Services;
using WarehouseManagement.Models;

namespace WarehouseManagement.Controllers
{
    /// <summary>
    /// Controller Ä‘iá»u hÆ°á»›ng cÃ¡c thao tÃ¡c liÃªn quan Ä‘áº¿n chi tiáº¿t phiáº¿u Nháº­p/Xuáº¥t kho
    /// </summary>
    public class TransactionDetailController
    {
        private readonly TransactionDetailService _detailService;

        public TransactionDetailController()
        {
            _detailService = new TransactionDetailService();
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch chi tiáº¿t theo Transaction ID
        /// </summary>
        public List<TransactionDetail> GetDetailsByTransactionId(int transactionId)
        {
            return _detailService.GetDetailsByTransactionId(transactionId);
        }

        /// <summary>
        /// Láº¥y chi tiáº¿t theo ID
        /// </summary>
        public TransactionDetail GetDetailById(int detailId)
        {
            return _detailService.GetDetailById(detailId);
        }

        /// <summary>
        /// ThÃªm chi tiáº¿t vÃ o phiáº¿u
        /// </summary>
        public int AddTransactionDetail(int transactionId, int productId, string productName, int quantity, decimal unitPrice)
        {
            return _detailService.AddTransactionDetail(transactionId, productId, productName, quantity, unitPrice);
        }

        /// <summary>
        /// Cáº­p nháº­t chi tiáº¿t phiáº¿u
        /// </summary>
        public bool UpdateTransactionDetail(int detailId, int quantity, decimal unitPrice)
        {
            return _detailService.UpdateTransactionDetail(detailId, quantity, unitPrice);
        }

        /// <summary>
        /// XÃ³a chi tiáº¿t phiáº¿u
        /// </summary>
        public bool DeleteTransactionDetail(int detailId)
        {
            return _detailService.DeleteTransactionDetail(detailId);
        }

        /// <summary>
        /// XÃ³a táº¥t cáº£ chi tiáº¿t cá»§a má»™t phiáº¿u
        /// </summary>
        public bool DeleteAllDetails(int transactionId)
        {
            return _detailService.DeleteAllDetails(transactionId);
        }

        /// <summary>
        /// TÃ­nh tá»•ng sá»‘ lÆ°á»£ng trong phiáº¿u
        /// </summary>
        public int GetTotalQuantity(int transactionId)
        {
            return _detailService.GetTotalQuantity(transactionId);
        }

        /// <summary>
        /// TÃ­nh tá»•ng giÃ¡ trá»‹ phiáº¿u chi tiáº¿t
        /// </summary>
        public decimal GetTotalValue(int transactionId)
        {
            return _detailService.GetTotalValue(transactionId);
        }

        /// <summary>
        /// Äáº¿m tá»•ng sá»‘ chi tiáº¿t trong phiáº¿u
        /// </summary>
        public int CountDetails(int transactionId)
        {
            return _detailService.CountDetails(transactionId);
        }
    }
}




