using System;
using System.Collections.Generic;

namespace WarehouseManagement.Models
{
    /// <summary>
    /// Lá»›p thá»±c thá»ƒ Phiáº¿u Nháº­p/Xuáº¥t kho
    /// </summary>
    public class StockTransaction
    {
        public int TransactionID { get; set; }
        public string Type { get; set; } // 'Import' hoáº·c 'Export'
        public DateTime DateCreated { get; set; }
        public int CreatedByUserID { get; set; } // ID ngÆ°á»i táº¡o phiáº¿u
        public string Note { get; set; }
        public decimal TotalValue { get; set; } // Tá»•ng giÃ¡ trá»‹ cá»§a Ä‘Æ¡n hÃ ng

        /// <summary>
        /// Danh sÃ¡ch chi tiáº¿t sáº£n pháº©m trong phiáº¿u
        /// </summary>
        public List<TransactionDetail> Details { get; set; } = new List<TransactionDetail>();

        /// <summary>
        /// Kiá»ƒm tra phiáº¿u lÃ  phiáº¿u nháº­p hay xuáº¥t
        /// </summary>
        public bool IsImport => Type == "Import";
    }
}




