using System;
using System.Collections.Generic;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;
using Newtonsoft.Json;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xá»­ lÃ½ logic chi tiáº¿t phiáº¿u Nháº­p/Xuáº¥t kho
    /// 
    /// CHá»¨C NÄ‚NG:
    /// - Quáº£n lÃ½ chi tiáº¿t phiáº¿u (CRUD): ThÃªm, sá»­a, xÃ³a
    /// - TÃ¬m kiáº¿m chi tiáº¿t: Theo phiáº¿u, sáº£n pháº©m
    /// - TÃ­nh toÃ¡n: TÃ­nh tá»•ng sá»‘ lÆ°á»£ng, tá»•ng giÃ¡ trá»‹
    /// 
    /// LUá»’NG:
    /// 1. Validation: Kiá»ƒm tra Ä‘áº§u vÃ o
    /// 2. Repository call: Gá»i DB Ä‘á»ƒ thá»±c hiá»‡n thao tÃ¡c
    /// 3. Logging: Ghi nháº­t kÃ½ Actions
    /// 4. Change tracking: Gá»i ActionsService.MarkAsChanged()
    /// 5. Return: Tráº£ vá» káº¿t quáº£
    /// </summary>
    public class TransactionDetailService
    {
        private readonly TransactionDetailRepository _detailRepo;
        private readonly ActionsRepository _logRepo;

        public TransactionDetailService()
        {
            _detailRepo = new TransactionDetailRepository();
            _logRepo = new ActionsRepository();
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch chi tiáº¿t theo Transaction ID
        /// </summary>
        public List<TransactionDetail> GetDetailsByTransactionId(int transactionId)
        {
            try
            {
                if (transactionId <= 0)
                    throw new ArgumentException("ID phiáº¿u khÃ´ng há»£p lá»‡");
                return _detailRepo.GetDetailsByTransactionId(transactionId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y chi tiáº¿t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y chi tiáº¿t theo ID
        /// </summary>
        public TransactionDetail GetDetailById(int detailId)
        {
            try
            {
                if (detailId <= 0)
                    throw new ArgumentException("ID chi tiáº¿t khÃ´ng há»£p lá»‡");
                return _detailRepo.GetDetailById(detailId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y chi tiáº¿t: " + ex.Message);
            }
        }

        /// <summary>
        /// ThÃªm chi tiáº¿t vÃ o phiáº¿u
        /// </summary>
        public int AddTransactionDetail(int transactionId, int productId, string productName, int quantity, decimal unitPrice)
        {
            try
            {
                // Validation
                if (transactionId <= 0)
                    throw new ArgumentException("ID phiáº¿u khÃ´ng há»£p lá»‡");
                if (productId <= 0)
                    throw new ArgumentException("ID sáº£n pháº©m khÃ´ng há»£p lá»‡");
                if (quantity <= 0)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng pháº£i lá»›n hÆ¡n 0");
                if (quantity > 999999)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng quÃ¡ lá»›n");
                if (unitPrice < 0)
                    throw new ArgumentException("ÄÆ¡n giÃ¡ khÃ´ng Ä‘Æ°á»£c Ã¢m");
                if (unitPrice > 999999999)
                    throw new ArgumentException("ÄÆ¡n giÃ¡ quÃ¡ lá»›n");
                if (string.IsNullOrWhiteSpace(productName))
                    throw new ArgumentException("TÃªn sáº£n pháº©m khÃ´ng Ä‘Æ°á»£c trá»‘ng");

                var detail = new TransactionDetail
                {
                    TransactionID = transactionId,
                    ProductID = productId,
                    ProductName = productName.Trim(),
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };

                int detailId = _detailRepo.AddTransactionDetail(detail);

                // Ghi nháº­t kÃ½
                var log = new Actions
                {
                    ActionType = "ADD_DETAIL",
                    Descriptions = $"ThÃªm chi tiáº¿t sáº£n pháº©m ID {productId} vÃ o phiáº¿u ID {transactionId}",
                    DataBefore = "",
                    CreatedAt = DateTime.Now
                };
                _logRepo.LogAction(log);

                ActionsService.Instance.MarkAsChanged();
                return detailId;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi thÃªm chi tiáº¿t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t chi tiáº¿t phiáº¿u
        /// </summary>
        public bool UpdateTransactionDetail(int detailId, int quantity, decimal unitPrice)
        {
            try
            {
                if (detailId <= 0)
                    throw new ArgumentException("ID chi tiáº¿t khÃ´ng há»£p lá»‡");
                if (quantity <= 0)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng pháº£i lá»›n hÆ¡n 0");
                if (quantity > 999999)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng quÃ¡ lá»›n");
                if (unitPrice < 0)
                    throw new ArgumentException("ÄÆ¡n giÃ¡ khÃ´ng Ä‘Æ°á»£c Ã¢m");
                if (unitPrice > 999999999)
                    throw new ArgumentException("ÄÆ¡n giÃ¡ quÃ¡ lá»›n");

                var oldDetail = _detailRepo.GetDetailById(detailId);
                if (oldDetail == null)
                    throw new ArgumentException("Chi tiáº¿t khÃ´ng tá»“n táº¡i");

                var beforeData = new
                {
                    DetailID = oldDetail.DetailID,
                    Quantity = oldDetail.Quantity,
                    UnitPrice = oldDetail.UnitPrice
                };

                var detail = new TransactionDetail
                {
                    DetailID = detailId,
                    TransactionID = oldDetail.TransactionID,
                    ProductID = oldDetail.ProductID,
                    ProductName = oldDetail.ProductName,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };

                bool result = _detailRepo.UpdateTransactionDetail(detail);

                if (result)
                {
                    var log = new Actions
                    {
                        ActionType = "UPDATE_DETAIL",
                        Descriptions = $"Cáº­p nháº­t chi tiáº¿t ID {detailId}",
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
                throw new Exception("Lá»—i khi cáº­p nháº­t chi tiáº¿t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a chi tiáº¿t phiáº¿u
        /// </summary>
        public bool DeleteTransactionDetail(int detailId)
        {
            try
            {
                if (detailId <= 0)
                    throw new ArgumentException("ID chi tiáº¿t khÃ´ng há»£p lá»‡");

                var detail = _detailRepo.GetDetailById(detailId);
                if (detail == null)
                    throw new ArgumentException("Chi tiáº¿t khÃ´ng tá»“n táº¡i");

                var beforeData = new
                {
                    DetailID = detail.DetailID,
                    TransactionID = detail.TransactionID,
                    ProductID = detail.ProductID,
                    ProductName = detail.ProductName,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                };

                bool result = _detailRepo.DeleteTransactionDetail(detailId);

                if (result)
                {
                    var log = new Actions
                    {
                        ActionType = "DELETE_DETAIL",
                        Descriptions = $"XÃ³a chi tiáº¿t ID {detailId}",
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
                throw new Exception("Lá»—i khi xÃ³a chi tiáº¿t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a táº¥t cáº£ chi tiáº¿t cá»§a má»™t phiáº¿u
        /// </summary>
        public bool DeleteAllDetails(int transactionId)
        {
            try
            {
                if (transactionId <= 0)
                    throw new ArgumentException("ID phiáº¿u khÃ´ng há»£p lá»‡");

                bool result = _detailRepo.DeleteAllDetails(transactionId);

                if (result)
                {
                    var log = new Actions
                    {
                        ActionType = "DELETE_ALL_DETAILS",
                        Descriptions = $"XÃ³a táº¥t cáº£ chi tiáº¿t cá»§a phiáº¿u ID {transactionId}",
                        DataBefore = "",
                        CreatedAt = DateTime.Now
                    };
                    _logRepo.LogAction(log);

                    ActionsService.Instance.MarkAsChanged();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a chi tiáº¿t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// TÃ­nh tá»•ng sá»‘ lÆ°á»£ng trong phiáº¿u
        /// </summary>
        public int GetTotalQuantity(int transactionId)
        {
            try
            {
                if (transactionId <= 0)
                    throw new ArgumentException("ID phiáº¿u khÃ´ng há»£p lá»‡");

                return _detailRepo.GetTotalQuantity(transactionId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi tÃ­nh tá»•ng sá»‘ lÆ°á»£ng: " + ex.Message);
            }
        }

        /// <summary>
        /// TÃ­nh tá»•ng giÃ¡ trá»‹ phiáº¿u chi tiáº¿t
        /// </summary>
        public decimal GetTotalValue(int transactionId)
        {
            try
            {
                if (transactionId <= 0)
                    throw new ArgumentException("ID phiáº¿u khÃ´ng há»£p lá»‡");

                var details = _detailRepo.GetDetailsByTransactionId(transactionId);
                decimal total = 0;
                foreach (var detail in details)
                {
                    total += detail.Quantity * detail.UnitPrice;
                }
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi tÃ­nh tá»•ng giÃ¡ trá»‹: " + ex.Message);
            }
        }

        /// <summary>
        /// Äáº¿m tá»•ng sá»‘ chi tiáº¿t trong phiáº¿u
        /// </summary>
        public int CountDetails(int transactionId)
        {
            try
            {
                if (transactionId <= 0)
                    throw new ArgumentException("ID phiáº¿u khÃ´ng há»£p lá»‡");

                return _detailRepo.GetDetailsByTransactionId(transactionId).Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi Ä‘áº¿m chi tiáº¿t: " + ex.Message);
            }
        }
    }
}




