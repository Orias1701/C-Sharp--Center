namespace WarehouseManagement.Models
{
    /// <summary>
    /// Lá»›p thá»±c thá»ƒ Sáº£n pháº©m (HÃ ng hÃ³a)
    /// </summary>
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int MinThreshold { get; set; }
        public decimal InventoryValue { get; set; } // Tá»•ng giÃ¡ trá»‹ tá»“n kho (Quantity * Price)

        /// <summary>
        /// Kiá»ƒm tra xem sáº£n pháº©m cÃ³ cáº£nh bÃ¡o tá»“n kho hay khÃ´ng
        /// </summary>
        public bool IsLowStock => Quantity < MinThreshold;
    }
}




