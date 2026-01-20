namespace WarehouseManagement.Models
{
    /// <summary>
    /// Lớp thực thể Chi tiết kiểm kê
    /// </summary>
    public class InventoryCheckDetail
    {
        public int DetailID { get; set; }
        public int CheckID { get; set; }
        public int ProductID { get; set; }
        public int SystemQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public int Difference { get; set; } // ActualQuantity - SystemQuantity (Database Generated, but useful to have in model)
        public string Reason { get; set; }
    }
}
