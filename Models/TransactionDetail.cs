namespace WarehouseManagement.Models
{
    /// <summary>
    /// Lá»›p thá»±c thá»ƒ Chi tiáº¿t phiáº¿u Nháº­p/Xuáº¥t
    /// </summary>
    public class TransactionDetail
    {
        public int DetailID { get; set; }
        public int TransactionID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } // Snapshot cá»§a tÃªn sáº£n pháº©m
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// TÃ­nh tá»•ng giÃ¡ trá»‹ cá»§a dÃ²ng chi tiáº¿t
        /// </summary>
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}




