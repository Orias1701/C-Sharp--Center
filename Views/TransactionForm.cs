using System;
using System.Windows.Forms;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Form Tạo phiếu Nhập/Xuất kho
    /// </summary>
    public partial class TransactionForm : Form
    {
        private string _transactionType; // "Import" hoặc "Export"

        public TransactionForm(string type)
        {
            InitializeComponent();
            _transactionType = type;
            Text = type == "Import" ? "Phiếu Nhập Kho" : "Phiếu Xuất Kho";
        }

        private void InitializeComponent()
        {
            // TODO: Thêm các control:
            // - ComboBox sản phẩm
            // - TextBox số lượng
            // - TextBox đơn giá
            // - TextBox ghi chú
            // - DataGridView để thêm chi tiết
            // - Button Thêm / Xóa / Lưu / Hủy
            SuspendLayout();
            ResumeLayout(false);
        }

        private void TransactionForm_Load(object sender, EventArgs e)
        {
            // Nạp danh sách sản phẩm vào ComboBox
        }

        /// <summary>
        /// Nút Thêm dòng chi tiết
        /// </summary>
        private void BtnAddDetail_Click(object sender, EventArgs e)
        {
            // Thêm sản phẩm vào DataGridView
            // Kiểm tra tồn kho nếu là Xuất
        }

        /// <summary>
        /// Nút Xóa dòng chi tiết
        /// </summary>
        private void BtnRemoveDetail_Click(object sender, EventArgs e)
        {
            // Xóa dòng được chọn
        }

        /// <summary>
        /// Nút Lưu phiếu
        /// </summary>
        private void BtnSaveTransaction_Click(object sender, EventArgs e)
        {
            // Kiểm tra chi tiết
            // Gọi InventoryController
            // Đóng form
        }

        /// <summary>
        /// Nút Hủy
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
