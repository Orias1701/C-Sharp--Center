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

                    if (status == "Completed")
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

        public void CompleteCheck(int checkId, int userId)
        {
            try
            {
                var check = _checkRepo.GetCheckById(checkId);
                if (check == null) throw new Exception("Không tìm thấy phiếu kiểm kê");
                if (check.Status == "Completed") throw new Exception("Phiếu kiểm kê đã hoàn tất");

                _checkRepo.UpdateStatus(checkId, "Completed");
                ProcessStockAdjustment(checkId, check.Details, userId);
                _logRepo.LogAction("COMPLETE_INVENTORY_CHECK", $"Hoàn tất phiếu kiểm kê ID {checkId}");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi hoàn tất kiểm kê: " + ex.Message);
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
                inventoryService.ImportStockBatch(importDetails, $"Cân bằng kiểm kê #{checkId} (+)");
            }

            if (exportDetails.Count > 0)
            {
                inventoryService.ExportStockBatch(exportDetails, $"Cân bằng kiểm kê #{checkId} (-)");
            }
        }
    }
}
