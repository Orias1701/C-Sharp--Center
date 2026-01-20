namespace WarehouseManagement.Models
{
    /// <summary>
    /// Lớp thực thể Khách hàng
    /// </summary>
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool Visible { get; set; } = true;
    }
}
