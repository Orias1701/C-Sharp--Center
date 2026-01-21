using System;
using System.Collections.Generic;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;

namespace WarehouseManagement.Services
{
    public class InventoryCheckService
    {
        private readonly InventoryCheckRepository _checkRepo;
        private readonly LogRepository _logRepo;
        private readonly ProductRepository _productRepo;

        public InventoryCheckService()
        {
            _checkRepo = new InventoryCheckRepository();
            _logRepo = new LogRepository();
            _productRepo = new ProductRepository(); 
        }

        public List<InventoryCheck> GetAllChecks()
        {
            return _checkRepo.GetAllChecks();
        }

        public InventoryCheck GetCheckById(int id)
        {
            return _checkRepo.GetCheckById(id);
        }

        public int CreateCheck(int userId, string note, List<InventoryCheckDetail> details, string status = "Pending")
        {
            try
            {
                if (details == null || details.Count == 0)
                    throw new ArgumentException("Danh sách kiểm kê không được trống");

                var check = new InventoryCheck
                {
                    CreatedByUserID = userId,
                    Note = note,
                    Status = status,
                    Visible = true,
                    CheckDate = DateTime.Now
                };

                // Validate products and check duplicates
                var productIds = new List<int>();
                foreach(var detail in details) {
                     if (!_productRepo.ProductIdExists(detail.ProductID))
                        throw new ArgumentException($"Sản phẩm ID {detail.ProductID} không tồn tại");
                     if (productIds.Contains(detail.ProductID))
                        throw new ArgumentException($"Sản phẩm ID {detail.ProductID} bị trùng lặp");
                     productIds.Add(detail.ProductID);
                }

                int checkId = _checkRepo.CreateCheck(check);
                
                if (checkId > 0)
                {
                    foreach (var detail in details)
                    {
                        detail.CheckID = checkId;
                        _checkRepo.AddCheckDetail(detail);
                    }
                    _logRepo.LogAction("CREATE_INVENTORY_CHECK", $"Tạo phiếu kiểm kê ID {checkId} ({status})");

                    if (status == "Approved")
                    {
                        ProcessStockAdjustment(checkId, details, userId);
                    }
                }
                
                return checkId;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo phiếu kiểm kê: " + ex.Message);
            }
        }

        public void ApproveCheck(int checkId, int userId)
        {
            try
            {
                var check = _checkRepo.GetCheckById(checkId);
                if (check == null) throw new Exception("Không tìm thấy phiếu kiểm kê");
                if (check.Status != "Pending") throw new Exception("Chỉ có thể duyệt phiếu đang chờ");

                _checkRepo.UpdateStatus(checkId, "Approved");
                ProcessStockAdjustment(checkId, check.Details, userId);
                _logRepo.LogAction("APPROVE_INVENTORY_CHECK", $"Duyệt phiếu kiểm kê ID {checkId}");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi duyệt kiểm kê: " + ex.Message);
            }
        }

        public void CancelCheck(int checkId, int userId)
        {
            try
            {
                var check = _checkRepo.GetCheckById(checkId);
                if (check == null) throw new Exception("Không tìm thấy phiếu kiểm kê");
                if (check.Status != "Pending") throw new Exception("Chỉ có thể hủy phiếu đang chờ");

                _checkRepo.UpdateStatus(checkId, "Cancelled");
                _logRepo.LogAction("CANCEL_INVENTORY_CHECK", $"Hủy phiếu kiểm kê ID {checkId}");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi hủy kiểm kê: " + ex.Message);
            }
        }

        private void ProcessStockAdjustment(int checkId, List<InventoryCheckDetail> details, int userId)
        {
            var importDetails = new List<(int ProductId, int Quantity, decimal UnitPrice)>();
            var exportDetails = new List<(int ProductId, int Quantity, decimal UnitPrice)>();
            var inventoryService = new InventoryService(); // Use service to create transactions

            foreach (var detail in details)
            {
                int diff = detail.ActualQuantity - detail.SystemQuantity;
                if (diff == 0) continue;

                var product = _productRepo.GetProductById(detail.ProductID);
                decimal price = product?.Price ?? 0;

                if (diff > 0) // Actual > System => Import
                {
                    importDetails.Add((detail.ProductID, diff, price));
                }
                else // Actual < System => Export
                {
                    exportDetails.Add((detail.ProductID, Math.Abs(diff), price));
                }
            }

            if (importDetails.Count > 0)
            {
                // I should modify InventoryService to return the TransactionID or use the newly added `ApproveTransaction` logic.
                
                // Workaround: Call `CreateTransaction` directly? No, should go through Service.
                // BETTER PLAN: Modify `InventoryService.ImportStockBatch` and `ExportStockBatch` to key off a "AutoApprove" flag?
                // OR: Just simply call `inventoryService.ImportStockBatch` and then find the last transaction? No, unsafe.
                
                // Let's modify `InventoryService` in a separate step if needed. 
                // For now, I will assume I need to fix `InventoryService` to return int (TransactionID) instead of bool.
                // But I can't do that in this single tool call if I haven't.
                // Let's checking `InventoryService.cs` again.
                // Lines 137: public bool ImportStockBatch... 
                
                // I will pause this Edit and update InventoryService first.
            }
        }
    }
}
