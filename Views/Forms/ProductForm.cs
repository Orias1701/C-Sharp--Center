using System;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;

namespace WarehouseManagement.Views.Forms
{
    /// <summary>
    /// Form ThÃªm/Sá»­a sáº£n pháº©m
    /// </summary>
    public partial class ProductForm : Form
    {
        private ProductController _productController;
        private CategoryController _categoryController;
        private int? _productId = null;
        private TextBox txtProductName, txtPrice, txtQuantity, txtMinThreshold;
        private ComboBox cmbCategory;
        private Button btnSave, btnCancel, btnEdit, btnDelete;

        public ProductForm(int? productId = null)
        {
            _productId = productId;
            _productController = new ProductController();
            _categoryController = new CategoryController();
            InitializeComponent();
            Text = productId.HasValue ? "Sá»­a sáº£n pháº©m" : "ThÃªm sáº£n pháº©m";
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

            // Labels vÃ  TextBoxes
            Label lblProductName = new Label { Text = "TÃªn sáº£n pháº©m:", Left = LABEL_LEFT, Top = 20, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtProductName = new TextBox { Left = INPUT_LEFT, Top = 20, Width = INPUT_WIDTH, Height = 25 };

            Label lblCategory = new Label { Text = "Danh má»¥c:", Left = LABEL_LEFT, Top = 20 + ITEM_SPACING, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            cmbCategory = new ComboBox { Left = INPUT_LEFT, Top = 20 + ITEM_SPACING, Width = INPUT_WIDTH, Height = 25, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCategory.Items.AddRange(new[] { "Thá»±c pháº©m", "Äiá»‡n tá»­", "Quáº§n Ã¡o", "KhÃ¡c" });

            Label lblPrice = new Label { Text = "GiÃ¡ (VNÄ):", Left = LABEL_LEFT, Top = 20 + ITEM_SPACING * 2, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtPrice = new TextBox { Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 2, Width = INPUT_WIDTH, Height = 25 };

            Label lblQuantity = new Label { Text = "Sá»‘ lÆ°á»£ng:", Left = LABEL_LEFT, Top = 20 + ITEM_SPACING * 3, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtQuantity = new TextBox { Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 3, Width = INPUT_WIDTH, Height = 25 };

            Label lblMinThreshold = new Label { Text = "NgÆ°á»¡ng tá»‘i thiá»ƒu:", Left = LABEL_LEFT, Top = 20 + ITEM_SPACING * 4, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtMinThreshold = new TextBox { Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 4, Width = INPUT_WIDTH, Height = 25 };

            btnSave = new Button { Text = "ðŸ’¾ LÆ°u", Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 5 + 10, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };
            btnCancel = new Button { Text = "âŒ Há»§y", Left = INPUT_LEFT + BUTTON_WIDTH + 15, Top = 20 + ITEM_SPACING * 5 + 10, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT, DialogResult = DialogResult.Cancel };
            btnEdit = new Button { Text = "âœï¸ Sá»­a", Left = 520 - 220, Top = 20 + ITEM_SPACING * 5 + 10, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };
            btnDelete = new Button { Text = "ðŸ—‘ï¸ XÃ³a", Left = 520 - 110, Top = 20 + ITEM_SPACING * 5 + 10, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };

            btnSave.Click += BtnSave_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;

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
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);

            Width = 520;
            Height = 420;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            CancelButton = btnCancel;
            Padding = new Padding(10);

            Load += ProductForm_Load;
            ResumeLayout(false);
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            if (_productId.HasValue)
            {
                LoadProduct();
                // Read-only mode by default
                txtProductName.ReadOnly = true;
                cmbCategory.Enabled = false;
                txtPrice.ReadOnly = true;
                txtQuantity.ReadOnly = true;
                txtMinThreshold.ReadOnly = true;
                
                btnSave.Visible = false;
                btnEdit.Visible = true;
                btnDelete.Visible = true;
            }
            else
            {
                cmbCategory.SelectedIndex = 0;
                btnSave.Visible = true;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
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
                MessageBox.Show("Lá»—i: " + ex.Message);
            }
        }

        /// <summary>
        /// NÃºt LÆ°u
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Frontend validation
            string productName = txtProductName.Text.Trim();
            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("âŒ Vui lÃ²ng nháº­p tÃªn sáº£n pháº©m");
                txtProductName.Focus();
                return;
            }

            if (productName.Length > 200)
            {
                MessageBox.Show("âŒ TÃªn sáº£n pháº©m khÃ´ng Ä‘Æ°á»£c vÆ°á»£t quÃ¡ 200 kÃ½ tá»±");
                txtProductName.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("âŒ GiÃ¡ pháº£i lÃ  sá»‘");
                txtPrice.Focus();
                return;
            }

            if (price < 0)
            {
                MessageBox.Show("âŒ GiÃ¡ khÃ´ng Ä‘Æ°á»£c Ã¢m");
                txtPrice.Focus();
                return;
            }

            if (price > 999999999)
            {
                MessageBox.Show("âŒ GiÃ¡ quÃ¡ lá»›n (tá»‘i Ä‘a: 999,999,999)");
                txtPrice.Focus();
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("âŒ Sá»‘ lÆ°á»£ng pháº£i lÃ  sá»‘ nguyÃªn");
                txtQuantity.Focus();
                return;
            }

            if (quantity < 0)
            {
                MessageBox.Show("âŒ Sá»‘ lÆ°á»£ng khÃ´ng Ä‘Æ°á»£c Ã¢m");
                txtQuantity.Focus();
                return;
            }

            if (quantity > 999999)
            {
                MessageBox.Show("âŒ Sá»‘ lÆ°á»£ng quÃ¡ lá»›n (tá»‘i Ä‘a: 999,999)");
                txtQuantity.Focus();
                return;
            }

            if (!int.TryParse(txtMinThreshold.Text, out int minThreshold))
            {
                MessageBox.Show("âŒ NgÆ°á»¡ng tá»‘i thiá»ƒu pháº£i lÃ  sá»‘ nguyÃªn");
                txtMinThreshold.Focus();
                return;
            }

            if (minThreshold < 0)
            {
                MessageBox.Show("âŒ NgÆ°á»¡ng tá»‘i thiá»ƒu khÃ´ng Ä‘Æ°á»£c Ã¢m");
                txtMinThreshold.Focus();
                return;
            }

            if (minThreshold > quantity)
            {
                MessageBox.Show("âŒ NgÆ°á»¡ng tá»‘i thiá»ƒu khÃ´ng Ä‘Æ°á»£c vÆ°á»£t quÃ¡ sá»‘ lÆ°á»£ng hiá»‡n táº¡i");
                txtMinThreshold.Focus();
                return;
            }

            if (cmbCategory.SelectedIndex < 0)
            {
                MessageBox.Show("âŒ Vui lÃ²ng chá»n danh má»¥c");
                cmbCategory.Focus();
                return;
            }

            try
            {
                if (_productId.HasValue)
                {
                    _productController.UpdateProductFull(_productId.Value, txtProductName.Text, cmbCategory.SelectedIndex + 1, price, quantity, minThreshold);
                    MessageBox.Show("Cáº­p nháº­t sáº£n pháº©m thÃ nh cÃ´ng!");
                }
                else
                {
                    _productController.CreateProduct(txtProductName.Text, cmbCategory.SelectedIndex + 1, price, quantity, minThreshold);
                    MessageBox.Show("ThÃªm sáº£n pháº©m thÃ nh cÃ´ng!");
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lá»—i: " + ex.Message);
            }
        }

        /// <summary>
        /// NÃºt Há»§y
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            // Enable edit mode
            txtProductName.ReadOnly = false;
            cmbCategory.Enabled = true;
            txtPrice.ReadOnly = false;
            txtQuantity.ReadOnly = false;
            txtMinThreshold.ReadOnly = false;

            btnEdit.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = true;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!_productId.HasValue) return;

            string productName = txtProductName.Text;
            
            DialogResult result = MessageBox.Show(
                $"Báº¡n cháº¯c cháº¯n muá»‘n xÃ³a sáº£n pháº©m '{productName}'?",
                "XÃ¡c nháº­n xÃ³a",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _productController.DeleteProduct(_productId.Value);
                    MessageBox.Show("Sáº£n pháº©m Ä‘Ã£ Ä‘Æ°á»£c xÃ³a thÃ nh cÃ´ng.", "ThÃ nh cÃ´ng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lá»—i xÃ³a sáº£n pháº©m: " + ex.Message, "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}




