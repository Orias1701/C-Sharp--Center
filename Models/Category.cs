namespace WarehouseManagement.Models
{
    /// <summary>
    /// Lớp thực thể Danh mục sản phẩm
    /// </summary>
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; } = true;
    }
}