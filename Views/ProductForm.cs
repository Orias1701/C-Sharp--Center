using System;
using System.Windows.Forms;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Form Thêm/Sửa sản phẩm
    /// </summary>
    public partial class ProductForm : Form
    {
        private int _productId = 0; // 0 = Thêm mới, >0 = Sửa
        public bool IsSuccess { get; set; } = false;

        public ProductForm(int productId = 0)
        {
            InitializeComponent();
            _productId = productId;
            Text = productId == 0 ? "Thêm sản phẩm" : "Sửa sản phẩm";
        }

        private void InitializeComponent()
        {
            // TODO: Thêm các control:
            // - TextBox tên sản phẩm
            // - ComboBox danh mục
            // - TextBox giá
            // - TextBox tồn kho
            // - TextBox ngưỡng báo
            // - Button Lưu / Hủy
            SuspendLayout();
            ResumeLayout(false);
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            // Nạp danh mục vào ComboBox
            if (_productId > 0)
            {
                // Nạp dữ liệu sản phẩm hiện tại
            }
        }

        /// <summary>
        /// Nút Lưu
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            // Gọi ProductController để lưu
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
