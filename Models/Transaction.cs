using System;
using System.Collections.Generic;

namespace WarehouseManagement.Models
{
    /// <summary>
    /// Lớp thực thể Phiếu Nhập/Xuất kho (Cập nhật cho bảng Transactions)
    /// </summary>
    public class Transaction
    {
        public int TransactionID { get; set; }
        public string Type { get; set; } // 'Import' hoặc 'Export'
        public DateTime DateCreated { get; set; }
        public int CreatedByUserID { get; set; } // Người lập phiếu
        
        // CẬP NHẬT MỚI: Đối tượng giao dịch
        public int? SupplierID { get; set; }
        public int? CustomerID { get; set; }

        // CẬP NHẬT MỚI: Tài chính
        public decimal TotalAmount { get; set; } // Tổng tiền hàng
        public decimal Discount { get; set; } // Chiết khấu
        public decimal FinalAmount { get; set; } // Thành tiền sau CK

        public string Note { get; set; }
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Danh sách chi tiết sản phẩm trong phiếu
        /// </summary>
        public List<TransactionDetail> Details { get; set; } = new List<TransactionDetail>();

        /// <summary>
        /// Kiểm tra phiếu là phiếu nhập hay xuất
        /// </summary>
        public bool IsImport => Type == "Import";
    }
}
