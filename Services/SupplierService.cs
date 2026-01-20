using System;
using System.Collections.Generic;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;

namespace WarehouseManagement.Services
{
    public class SupplierService
    {
        private readonly SupplierRepository _supplierRepo;
        private readonly LogRepository _logRepo;

        public SupplierService()
        {
            _supplierRepo = new SupplierRepository();
            _logRepo = new LogRepository();
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _supplierRepo.GetAllSuppliers();
        }

        public Supplier GetSupplierById(int id)
        {
            return _supplierRepo.GetSupplierById(id);
        }

        public bool AddSupplier(Supplier supplier)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(supplier.SupplierName))
                    throw new ArgumentException("Tên nhà cung cấp không được để trống");

                if (_supplierRepo.AddSupplier(supplier) > 0)
                {
                    _logRepo.LogAction("ADD_SUPPLIER", $"Thêm nhà cung cấp: {supplier.SupplierName}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm nhà cung cấp: " + ex.Message);
            }
        }

        public bool UpdateSupplier(Supplier supplier)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(supplier.SupplierName))
                    throw new ArgumentException("Tên nhà cung cấp không được để trống");

                if (_supplierRepo.UpdateSupplier(supplier))
                {
                    _logRepo.LogAction("UPDATE_SUPPLIER", $"Cập nhật nhà cung cấp ID {supplier.SupplierID}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật nhà cung cấp: " + ex.Message);
            }
        }

        public bool DeleteSupplier(int id)
        {
            try
            {
                // Soft delete
                if (_supplierRepo.SoftDeleteSupplier(id))
                {
                    _logRepo.LogAction("DELETE_SUPPLIER", $"Xóa (ẩn) nhà cung cấp ID {id}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa nhà cung cấp: " + ex.Message);
            }
        }
    }
}
