using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;

namespace WarehouseManagement.Views.Forms
{
    /// <summary>
    /// Form ThÃªm/Sá»­a danh má»¥c sáº£n pháº©m
    /// </summary>
    public partial class CategoryForm : Form
    {
        private CategoryController _categoryController;
        private int? _categoryId = null;
        private TextBox txtCategoryName;
        private Button btnSave, btnCancel, btnEdit, btnDelete;

        public CategoryForm(int? categoryId = null)
        {
            _categoryId = categoryId;
            _categoryController = new CategoryController();
            InitializeComponent();
            Text = categoryId.HasValue ? "Sá»­a danh má»¥c" : "ThÃªm danh má»¥c";
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // Layout standard: Label 100px, Input 300px, spacing 35px
            const int LABEL_WIDTH = 100;
            const int INPUT_WIDTH = 300;
            const int LABEL_LEFT = 20;
            const int INPUT_LEFT = 130;
            const int ITEM_SPACING = 35;
            const int BUTTON_WIDTH = 100;
            const int BUTTON_HEIGHT = 35;

            // Labels vÃ  TextBoxes
            Label lblCategoryName = new Label { Text = "TÃªn danh má»¥c:", Left = LABEL_LEFT, Top = 20, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtCategoryName = new TextBox { Left = INPUT_LEFT, Top = 20, Width = INPUT_WIDTH, Height = 25 };

            btnSave = new Button { Text = "ðŸ’¾ LÆ°u", Left = INPUT_LEFT, Top = 20 + ITEM_SPACING + 10, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };
            btnCancel = new Button { Text = "âŒ Há»§y", Left = INPUT_LEFT + BUTTON_WIDTH + 15, Top = 20 + ITEM_SPACING + 10, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT, DialogResult = DialogResult.Cancel };
            btnEdit = new Button { Text = "âœï¸ Sá»­a", Left = 420 - 220, Top = 20 + ITEM_SPACING + 10, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };
            btnDelete = new Button { Text = "ðŸ—‘ï¸ XÃ³a", Left = 420 - 110, Top = 20 + ITEM_SPACING + 10, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };

            btnSave.Click += BtnSave_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;

            Controls.Add(lblCategoryName);
            Controls.Add(txtCategoryName);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);

            Width = 480;
            Height = 180;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            CancelButton = btnCancel;
            Padding = new Padding(10);

            Load += CategoryForm_Load;
            ResumeLayout(false);
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            if (_categoryId.HasValue)
            {
                LoadCategory();
                // Read-only mode by default
                txtCategoryName.ReadOnly = true;
                
                btnSave.Visible = false;
                btnEdit.Visible = true;
                btnDelete.Visible = true;
            }
            else
            {
                btnSave.Visible = true;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
            }
        }

        private void LoadCategory()
        {
            try
            {
                Category category = _categoryController.GetCategoryById(_categoryId.Value);
                if (category != null)
                {
                    txtCategoryName.Text = category.CategoryName;
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
            string categoryName = txtCategoryName.Text.Trim();
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("âŒ Vui lÃ²ng nháº­p tÃªn danh má»¥c");
                txtCategoryName.Focus();
                return;
            }

            if (categoryName.Length > 100)
            {
                MessageBox.Show("âŒ TÃªn danh má»¥c khÃ´ng Ä‘Æ°á»£c vÆ°á»£t quÃ¡ 100 kÃ½ tá»±");
                txtCategoryName.Focus();
                return;
            }

            try
            {
                if (_categoryId.HasValue)
                {
                    _categoryController.UpdateCategory(_categoryId.Value, categoryName);
                    MessageBox.Show("Cáº­p nháº­t danh má»¥c thÃ nh cÃ´ng!");
                }
                else
                {
                    _categoryController.CreateCategory(categoryName);
                    MessageBox.Show("ThÃªm danh má»¥c thÃ nh cÃ´ng!");
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
            txtCategoryName.ReadOnly = false;

            btnEdit.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = true;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!_categoryId.HasValue) return;

            string categoryName = txtCategoryName.Text;
            
            DialogResult result = MessageBox.Show(
                $"Báº¡n cháº¯c cháº¯n muá»‘n xÃ³a danh má»¥c '{categoryName}'?",
                "XÃ¡c nháº­n xÃ³a",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _categoryController.DeleteCategory(_categoryId.Value);
                    MessageBox.Show("Danh má»¥c Ä‘Ã£ Ä‘Æ°á»£c xÃ³a thÃ nh cÃ´ng.", "ThÃ nh cÃ´ng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lá»—i xÃ³a danh má»¥c: " + ex.Message, "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}



