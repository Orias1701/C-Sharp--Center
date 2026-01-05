using System;
using System.Windows.Forms;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Form chính - Giao diện chính ứng dụng với TabControl
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Text = "Quản Lý Kho Hàng";
            WindowState = FormWindowState.Maximized;
        }

        private void InitializeComponent()
        {
            // TODO: Thêm các control như TabControl, DataGridView, Button, etc.
            // 1. TabControl chứa các tab: Sản phẩm, Phiếu, Báo cáo
            // 2. Tab Sản phẩm: Hiển thị danh sách SP, nút Thêm/Sửa/Xóa
            // 3. Tab Phiếu: Hiển thị danh sách phiếu Nhập/Xuất
            // 4. Tab Báo cáo: Biểu đồ, thống kê
            SuspendLayout();
            ResumeLayout(false);
        }

        /// <summary>
        /// Hàm khởi tạo form (gọi khi form load)
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Kiểm tra kết nối database
            // Tải dữ liệu sản phẩm
            // Hiển thị danh sách sản phẩm
        }

        /// <summary>
        /// Nút Thêm sản phẩm
        /// </summary>
        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            // Mở form ProductForm để thêm sản phẩm mới
        }

        /// <summary>
        /// Nút Sửa sản phẩm
        /// </summary>
        private void BtnEditProduct_Click(object sender, EventArgs e)
        {
            // Mở form ProductForm để chỉnh sửa sản phẩm chọn
        }

        /// <summary>
        /// Nút Xóa sản phẩm
        /// </summary>
        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            // Xóa sản phẩm chọn (có xác nhận)
        }

        /// <summary>
        /// Nút Nhập kho
        /// </summary>
        private void BtnImport_Click(object sender, EventArgs e)
        {
            // Mở form TransactionForm để tạo phiếu nhập
        }

        /// <summary>
        /// Nút Xuất kho
        /// </summary>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            // Mở form TransactionForm để tạo phiếu xuất
        }

        /// <summary>
        /// Nút Báo cáo
        /// </summary>
        private void BtnReport_Click(object sender, EventArgs e)
        {
            // Mở form ReportForm để xem biểu đồ
        }
    }
}
