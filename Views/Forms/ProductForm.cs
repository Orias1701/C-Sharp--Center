using System;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;

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
        private CustomTextBox txtProductName, txtPrice, txtQuantity, txtMinThreshold;
        private CustomComboBox cmbCategory;
        private CustomButton btnSave, btnCancel;

        public ProductForm(int? productId = null)
        {
            _productId = productId;
            _productController = new ProductController();
            _categoryController = new CategoryController();
            InitializeComponent();
            Text = productId.HasValue 
                ? $"{UIConstants.Icons.Edit} Sửa sản phẩm" 
                : $"{UIConstants.Icons.Add} Thêm sản phẩm";
            
            // Apply theme
            ThemeManager.Instance.ApplyThemeToForm(this);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // Main container
            CustomPanel mainPanel = new CustomPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = UIConstants.Borders.RadiusLarge,
                ShowBorder = false,
                Padding = new Padding(0)  // Không dùng padding của panel
            };

            // Layout constants
            const int INPUT_WIDTH = 400;
            const int LEFT_MARGIN = 40;  // Margin từ bên trái
            int currentY = 30;
            int inputSpacing = 20;

            // Product Name Label
            Label lblProductName = new Label 
            { 
                Text = "Tên sản phẩm",
                Left = LEFT_MARGIN, 
                Top = currentY, 
                AutoSize = true,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B),
                TabStop = false
            };
            currentY += 20;

            txtProductName = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = INPUT_WIDTH,
                Placeholder = "Nhập tên sản phẩm...",
                TabIndex = 0
            };
            currentY += UIConstants.Sizes.InputHeight + inputSpacing;

            // Category Label
            Label lblCategory = new Label 
            { 
                Text = "Danh mục",
                Left = LEFT_MARGIN, 
                Top = currentY, 
                AutoSize = true,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B),
                TabStop = false
            };
            currentY += 20;

            cmbCategory = new CustomComboBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = INPUT_WIDTH,
                TabIndex = 1
            };
            LoadCategories();
            currentY += UIConstants.Sizes.InputHeight + inputSpacing;

            // Price Label
            Label lblPrice = new Label 
            { 
                Text = "Giá (VNĐ)",
                Left = LEFT_MARGIN, 
                Top = currentY, 
                AutoSize = true,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B),
                TabStop = false
            };
            currentY += 20;

            txtPrice = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = INPUT_WIDTH,
                Placeholder = "Nhập giá...",
                TabIndex = 2
            };
            currentY += UIConstants.Sizes.InputHeight + inputSpacing;

            // Quantity Label
            Label lblQuantity = new Label 
            { 
                Text = "Số lượng",
                Left = LEFT_MARGIN, 
                Top = currentY, 
                AutoSize = true,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B),
                TabStop = false
            };
            currentY += 20;

            txtQuantity = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = INPUT_WIDTH,
                Placeholder = "Nhập số lượng...",
                TabIndex = 3
            };
            currentY += UIConstants.Sizes.InputHeight + inputSpacing;

            // Min Threshold Label
            Label lblMinThreshold = new Label 
            { 
                Text = "Ngưỡng tối thiểu",
                Left = LEFT_MARGIN, 
                Top = currentY, 
                AutoSize = true,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B),
                TabStop = false
            };
            currentY += 20;

            txtMinThreshold = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = INPUT_WIDTH,
                Placeholder = "Nhập ngưỡng tối thiểu...",
                TabIndex = 4
            };
            currentY += UIConstants.Sizes.InputHeight + 30;

            // Buttons - Centered
            int totalButtonWidth = 120 + 10 + 120; // Lưu + spacing + Hủy
            int buttonStartX = LEFT_MARGIN + (INPUT_WIDTH - totalButtonWidth) / 2;

            btnSave = new CustomButton 
            { 
                Text = "Lưu",
                Left = buttonStartX, 
                Top = currentY, 
                Width = 120,
                ButtonStyleType = ButtonStyle.FilledNoOutline,
                TabIndex = 5
            };

            btnCancel = new CustomButton 
            { 
                Text = "Hủy",
                Left = buttonStartX + 120 + 10, 
                Top = currentY, 
                Width = 120,
                ButtonStyleType = ButtonStyle.Outlined,
                CausesValidation = false,
                TabIndex = 6
            };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            mainPanel.Controls.Add(lblProductName);
            mainPanel.Controls.Add(txtProductName);
            mainPanel.Controls.Add(lblCategory);
            mainPanel.Controls.Add(cmbCategory);
            mainPanel.Controls.Add(lblPrice);
            mainPanel.Controls.Add(txtPrice);
            mainPanel.Controls.Add(lblQuantity);
            mainPanel.Controls.Add(txtQuantity);
            mainPanel.Controls.Add(lblMinThreshold);
            mainPanel.Controls.Add(txtMinThreshold);
            mainPanel.Controls.Add(btnSave);
            mainPanel.Controls.Add(btnCancel);

            Controls.Add(mainPanel);

            ClientSize = new Size(480, 560);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = ThemeManager.Instance.BackgroundLight;
            AcceptButton = btnSave;
            CancelButton = btnCancel;

            Load += ProductForm_Load;
            ResumeLayout(false);
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
                if (cmbCategory.Items.Count > 0)
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
                MessageBox.Show($"{UIConstants.Icons.Error} Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"{UIConstants.Icons.Warning} Lỗi khi tải danh mục: {ex.Message}", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Fallback to hardcoded categories
                cmbCategory.Items.AddRange(new[] { "Điện tử", "Gia dụng", "Công cụ" });
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Frontend validation
            string productName = txtProductName.Text.Trim();
            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Vui lòng nhập tên sản phẩm", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return;
            }

            if (productName.Length > 200)
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Tên sản phẩm không được vượt quá 200 ký tự", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Giá phải là số", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            if (price < 0)
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Giá không được âm", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            if (price > 999999999)
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Giá quá lớn (tối đa: 999,999,999)", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Số lượng phải là số nguyên", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            if (quantity < 0)
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Số lượng không được âm", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            if (quantity > 999999)
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Số lượng quá lớn (tối đa: 999,999)", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            if (!int.TryParse(txtMinThreshold.Text, out int minThreshold))
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Ngưỡng tối thiểu phải là số nguyên", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMinThreshold.Focus();
                return;
            }

            if (minThreshold < 0)
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Ngưỡng tối thiểu không được âm", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMinThreshold.Focus();
                return;
            }

            if (minThreshold > quantity)
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Ngưỡng tối thiểu không được vượt quá số lượng hiện tại", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMinThreshold.Focus();
                return;
            }

            if (cmbCategory.SelectedIndex < 0)
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Vui lòng chọn danh mục", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return;
            }

            try
            {
                if (_productId.HasValue)
                {
                    _productController.UpdateProductFull(_productId.Value, txtProductName.Text, cmbCategory.SelectedIndex + 1, price, quantity, minThreshold);
                    MessageBox.Show($"{UIConstants.Icons.Success} Cập nhật sản phẩm thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _productController.CreateProduct(txtProductName.Text, cmbCategory.SelectedIndex + 1, price, quantity, minThreshold);
                    MessageBox.Show($"{UIConstants.Icons.Success} Thêm sản phẩm thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{UIConstants.Icons.Error} Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
