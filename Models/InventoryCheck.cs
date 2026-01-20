using System;
using System.Collections.Generic;

namespace WarehouseManagement.Models
{
    /// <summary>
    /// Lớp thực thể Phiếu kiểm kê
    /// </summary>
    public class InventoryCheck
    {
        public int CheckID { get; set; }
        public DateTime CheckDate { get; set; }
        public int CreatedByUserID { get; set; }
        public string Status { get; set; } // 'Pending', 'Completed', 'Cancelled'
        public string Note { get; set; }
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Danh sách chi tiết kiểm kê
        /// </summary>
        public List<InventoryCheckDetail> Details { get; set; } = new List<InventoryCheckDetail>();
    }
}
