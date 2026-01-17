using System;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;

namespace WarehouseManagement.Views.Forms
{
    /// <summary>
    /// Form Thêm/Sửa sản phẩm
    /// </summary>
    public partial class ProductForm : Form
    {
        private ProductController _productController;
        private CategoryController _categoryController;
        private int? _productId = null;
        private TextBox txtProductName, txtPrice, txtQuantity, txtMinThreshold;
        private ComboBox cmbCategory;
        private Button btnSave, btnCancel;

        public ProductForm(int? productId = null)
        {
            _productId = productId;
            _productController = new ProductController();
            _categoryController = new CategoryController();
            InitializeComponent();
            Text = productId.HasValue ? "Sửa sản phẩm" : "Thêm sản phẩm";
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // Layout standard: Label 100px, Input 300px, spacing 20px
            const int LABEL_WIDTH = 100;
            const int INPUT_WIDTH = 300;
            const int LABEL_LEFT = 20;
            const int INPUT_LEFT = 130;
            const int ITEM_SPACING = 35;
            const int BUTTON_WIDTH = 100;
            const int BUTTON_HEIGHT = 35;

            // Labels và TextBoxes
            Label lblProductName = new Label { Text = "Tên sản phẩm:", Left = LABEL_LEFT, Top = 20, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtProductName = new TextBox { Left = INPUT_LEFT, Top = 20, Width = INPUT_WIDTH, Height = 25 };

            Label lblCategory = new Label { Text = "Danh mục:", Left = LABEL_LEFT, Top = 20 + ITEM_SPACING, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            cmbCategory = new ComboBox { Left = INPUT_LEFT, Top = 20 + ITEM_SPACING, Width = INPUT_WIDTH, Height = 25, DropDownStyle = ComboBoxStyle.DropDownList };
            LoadCategories();

            Label lblPrice = new Label { Text = "Giá (VNĐ):", Left = LABEL_LEFT, Top = 20 + ITEM_SPACING * 2, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtPrice = new TextBox { Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 2, Width = INPUT_WIDTH, Height = 25 };

            Label lblQuantity = new Label { Text = "Số lượng:", Left = LABEL_LEFT, Top = 20 + ITEM_SPACING * 3, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtQuantity = new TextBox { Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 3, Width = INPUT_WIDTH, Height = 25 };

            Label lblMinThreshold = new Label { Text = "Ngưỡng tối thiểu:", Left = LABEL_LEFT, Top = 20 + ITEM_SPACING * 4, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtMinThreshold = new TextBox { Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 4, Width = INPUT_WIDTH, Height = 25 };

            btnSave = new Button { Text = "💾 Lưu", Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 5 + 10, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };
            btnCancel = new Button { Text = "❌ Hủy", Left = INPUT_LEFT + BUTTON_WIDTH + 15, Top = 20 + ITEM_SPACING * 5 + 10, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT, CausesValidation = false };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

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

            Width = 520;
            Height = 420;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Padding = new Padding(10);

            Load += ProductForm_Load;
            ResumeLayout(true);
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            // Fields are always editable by default
            txtProductName.ReadOnly = false;
            cmbCategory.Enabled = true;
            txtPrice.ReadOnly = false;
            txtQuantity.ReadOnly = false;
            txtMinThreshold.ReadOnly = false;

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
                    txtPrice.Text = product.Price.ToString("N0");
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

        private void LoadCategories()
        {
            try
            {
                var categories = _categoryController.GetAllCategories();
                cmbCategory.Items.Clear();
                foreach (var category in categories)
                {
                    cmbCategory.Items.Add(category.CategoryName);
                }
                if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh mục: " + ex.Message);
                // Fallback to hardcoded categories
                cmbCategory.Items.AddRange(new[] { "Điện tử", "Gia dụng", "Công cụ" });
            }
        }

        /// <summary>
        /// Nút Lưu
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Frontend validation
            string productName = txtProductName.Text.Trim();
            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("❌ Vui lòng nhập tên sản phẩm");
                txtProductName.Focus();
                return;
            }

            if (productName.Length > 200)
            {
                MessageBox.Show("❌ Tên sản phẩm không được vượt quá 200 ký tự");
                txtProductName.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("❌ Giá phải là số");
                txtPrice.Focus();
                return;
            }

            if (price < 0)
            {
                MessageBox.Show("❌ Giá không được âm");
                txtPrice.Focus();
                return;
            }

            if (price > 999999999)
            {
                MessageBox.Show("❌ Giá quá lớn (tối đa: 999,999,999)");
                txtPrice.Focus();
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("❌ Số lượng phải là số nguyên");
                txtQuantity.Focus();
                return;
            }

            if (quantity < 0)
            {
                MessageBox.Show("❌ Số lượng không được âm");
                txtQuantity.Focus();
                return;
            }

            if (quantity > 999999)
            {
                MessageBox.Show("❌ Số lượng quá lớn (tối đa: 999,999)");
                txtQuantity.Focus();
                return;
            }

            if (!int.TryParse(txtMinThreshold.Text, out int minThreshold))
            {
                MessageBox.Show("❌ Ngưỡng tối thiểu phải là số nguyên");
                txtMinThreshold.Focus();
                return;
            }

            if (minThreshold < 0)
            {
                MessageBox.Show("❌ Ngưỡng tối thiểu không được âm");
                txtMinThreshold.Focus();
                return;
            }

            if (minThreshold > quantity)
            {
                MessageBox.Show("❌ Ngưỡng tối thiểu không được vượt quá số lượng hiện tại");
                txtMinThreshold.Focus();
                return;
            }

            if (cmbCategory.SelectedIndex < 0)
            {
                MessageBox.Show("❌ Vui lòng chọn danh mục");
                cmbCategory.Focus();
                return;
            }

            try
            {
                if (_productId.HasValue)
                {
                    _productController.UpdateProductFull(_productId.Value, txtProductName.Text, cmbCategory.SelectedIndex + 1, price, quantity, minThreshold);
                    MessageBox.Show("Cập nhật sản phẩm thành công!");
                }
                else
                {
                    _productController.CreateProduct(txtProductName.Text, cmbCategory.SelectedIndex + 1, price, quantity, minThreshold);
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
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}