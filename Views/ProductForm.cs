using System;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Form Thêm/Sửa sản phẩm
    /// </summary>
    public partial class ProductForm : Form
    {
        private ProductController _productController;
        private int? _productId = null;
        private TextBox txtProductName, txtPrice, txtQuantity, txtMinThreshold;
        private ComboBox cmbCategory;
        private Button btnSave, btnCancel;

        public ProductForm(int? productId = null)
        {
            _productId = productId;
            _productController = new ProductController();
            InitializeComponent();
            Text = productId.HasValue ? "Sửa sản phẩm" : "Thêm sản phẩm";
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // Labels và TextBoxes
            Label lblProductName = new Label { Text = "Tên sản phẩm:", Left = 20, Top = 20, Width = 120 };
            txtProductName = new TextBox { Left = 150, Top = 20, Width = 300, Height = 25 };

            Label lblCategory = new Label { Text = "Danh mục:", Left = 20, Top = 60, Width = 120 };
            cmbCategory = new ComboBox { Left = 150, Top = 60, Width = 300, Height = 25, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCategory.Items.AddRange(new[] { "Thực phẩm", "Điện tử", "Quần áo", "Khác" });

            Label lblPrice = new Label { Text = "Giá (VNĐ):", Left = 20, Top = 100, Width = 120 };
            txtPrice = new TextBox { Left = 150, Top = 100, Width = 300, Height = 25 };

            Label lblQuantity = new Label { Text = "Số lượng:", Left = 20, Top = 140, Width = 120 };
            txtQuantity = new TextBox { Left = 150, Top = 140, Width = 300, Height = 25 };

            Label lblMinThreshold = new Label { Text = "Ngưỡng tối thiểu:", Left = 20, Top = 180, Width = 120 };
            txtMinThreshold = new TextBox { Left = 150, Top = 180, Width = 300, Height = 25 };

            btnSave = new Button { Text = "💾 Lưu", Left = 150, Top = 220, Width = 100, Height = 35 };
            btnCancel = new Button { Text = "❌ Hủy", Left = 270, Top = 220, Width = 100, Height = 35, DialogResult = DialogResult.Cancel };

            btnSave.Click += BtnSave_Click;

            Controls.Add(lblProductName);
            Controls.Add(txtProductName);
            Controls.Add(lblCategory);
            Controls.Add(cmbCategory);
            Controls.Add(lblPrice);
            Controls.Add(txtPrice);
            Controls.Add(lblQuantity);
            Controls.Add(txtQuantity);
            Controls.Add(lblMinThreshold);
            Controls.Add(txtMinThreshold);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);

            Width = 500;
            Height = 300;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            CancelButton = btnCancel;

            Load += ProductForm_Load;
            ResumeLayout(false);
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            if (_productId.HasValue)
            {
                LoadProduct();
            }
            else
            {
                cmbCategory.SelectedIndex = 0;
            }
        }

        private void LoadProduct()
        {
            try
            {
                Product product = _productController.GetProductById(_productId.Value);
                if (product != null)
                {
                    txtProductName.Text = product.ProductName;
                    txtPrice.Text = product.Price.ToString();
                    txtQuantity.Text = product.Quantity.ToString();
                    txtMinThreshold.Text = product.MinThreshold.ToString();
                    cmbCategory.SelectedIndex = Math.Max(0, product.CategoryID - 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        /// <summary>
        /// Nút Lưu
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Giá không hợp lệ");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Số lượng không hợp lệ");
                return;
            }

            if (!int.TryParse(txtMinThreshold.Text, out int minThreshold) || minThreshold < 0)
            {
                MessageBox.Show("Ngưỡng tối thiểu không hợp lệ");
                return;
            }

            try
            {
                if (_productId.HasValue)
                {
                    _productController.UpdateProduct(new Product
                    {
                        ProductID = _productId.Value,
                        ProductName = txtProductName.Text,
                        CategoryID = cmbCategory.SelectedIndex + 1,
                        Price = price,
                        Quantity = quantity,
                        MinThreshold = minThreshold
                    });
                    MessageBox.Show("Cập nhật sản phẩm thành công!");
                }
                else
                {
                    _productController.AddProduct(new Product
                    {
                        ProductName = txtProductName.Text,
                        CategoryID = cmbCategory.SelectedIndex + 1,
                        Price = price,
                        Quantity = quantity,
                        MinThreshold = minThreshold
                    });
                    MessageBox.Show("Thêm sản phẩm thành công!");
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
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
