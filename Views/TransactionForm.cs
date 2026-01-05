using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Form Tạo phiếu Nhập/Xuất kho
    /// </summary>
    public partial class TransactionForm : Form
    {
        private string _transactionType; // "Import" hoặc "Export"
        private InventoryController _inventoryController;
        private ProductController _productController;
        private ComboBox cmbProduct;
        private TextBox txtQuantity, txtUnitPrice, txtNote;
        private DataGridView dgvDetails;
        private Button btnAddDetail, btnRemoveDetail, btnSaveTransaction, btnCancel;
        private List<(int ProductID, int Quantity, decimal UnitPrice)> _details;

        public TransactionForm(string type)
        {
            InitializeComponent();
            _transactionType = type;
            _details = new List<(int, int, decimal)>();
            _inventoryController = new InventoryController();
            _productController = new ProductController();
            Text = type == "Import" ? "Phiếu Nhập Kho" : "Phiếu Xuất Kho";
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // Labels và controls
            Label lblProduct = new Label { Text = "Sản phẩm:", Left = 20, Top = 20, Width = 100 };
            cmbProduct = new ComboBox { Left = 130, Top = 20, Width = 250, Height = 25, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblQuantity = new Label { Text = "Số lượng:", Left = 20, Top = 60, Width = 100 };
            txtQuantity = new TextBox { Left = 130, Top = 60, Width = 100, Height = 25 };

            Label lblPrice = new Label { Text = "Đơn giá:", Left = 250, Top = 60, Width = 100 };
            txtUnitPrice = new TextBox { Left = 360, Top = 60, Width = 120, Height = 25 };

            Label lblNote = new Label { Text = "Ghi chú:", Left = 20, Top = 100, Width = 100 };
            txtNote = new TextBox { Left = 130, Top = 100, Width = 350, Height = 50, Multiline = true };

            btnAddDetail = new Button { Text = "➕ Thêm", Left = 130, Top = 160, Width = 80, Height = 30 };
            btnRemoveDetail = new Button { Text = "🗑️ Xóa", Left = 220, Top = 160, Width = 80, Height = 30 };

            btnAddDetail.Click += BtnAddDetail_Click;
            btnRemoveDetail.Click += BtnRemoveDetail_Click;

            // DataGridView
            dgvDetails = new DataGridView
            {
                Dock = DockStyle.Bottom,
                Height = 200,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                Location = new System.Drawing.Point(0, 250)
            };

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Sản phẩm", DataPropertyName = "ProductName", Width = 200 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số lượng", DataPropertyName = "Quantity", Width = 80 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đơn giá", DataPropertyName = "UnitPrice", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "C" } });

            btnSaveTransaction = new Button { Text = "💾 Lưu Phiếu", Left = 130, Top = 200, Width = 100, Height = 35 };
            btnCancel = new Button { Text = "❌ Hủy", Left = 240, Top = 200, Width = 100, Height = 35, DialogResult = DialogResult.Cancel };

            btnSaveTransaction.Click += BtnSaveTransaction_Click;

            Controls.Add(lblProduct);
            Controls.Add(cmbProduct);
            Controls.Add(lblQuantity);
            Controls.Add(txtQuantity);
            Controls.Add(lblPrice);
            Controls.Add(txtUnitPrice);
            Controls.Add(lblNote);
            Controls.Add(txtNote);
            Controls.Add(btnAddDetail);
            Controls.Add(btnRemoveDetail);
            Controls.Add(btnSaveTransaction);
            Controls.Add(btnCancel);
            Controls.Add(dgvDetails);

            Width = 600;
            Height = 500;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            CancelButton = btnCancel;

            Load += TransactionForm_Load;
            ResumeLayout(false);
        }

        private void TransactionForm_Load(object sender, EventArgs e)
        {
            try
            {
                List<Product> products = _productController.GetAllProducts();
                foreach (var product in products)
                {
                    cmbProduct.Items.Add(new { Text = product.ProductName, Value = product.ProductID });
                }
                cmbProduct.DisplayMember = "Text";
                cmbProduct.ValueMember = "Value";
                if (cmbProduct.Items.Count > 0) cmbProduct.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sản phẩm: " + ex.Message);
            }
        }

        private void BtnAddDetail_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ");
                return;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ");
                return;
            }

            int productId = (int)cmbProduct.SelectedValue;
            
            // Kiểm tra tồn kho nếu là Xuất
            if (_transactionType == "Export")
            {
                Product product = _productController.GetProductById(productId);
                if (product.Quantity < quantity)
                {
                    MessageBox.Show($"Tồn kho không đủ (hiện có: {product.Quantity})");
                    return;
                }
            }

            _details.Add((productId, quantity, price));
            RefreshDetails();
            txtQuantity.Clear();
            txtUnitPrice.Clear();
        }

        private void RefreshDetails()
        {
            dgvDetails.DataSource = null;
            var displayList = new List<dynamic>();
            foreach (var (productId, qty, price) in _details)
            {
                var product = _productController.GetProductById(productId);
                displayList.Add(new { ProductName = product.ProductName, Quantity = qty, UnitPrice = price });
            }
            dgvDetails.DataSource = displayList;
        }

        private void BtnRemoveDetail_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng để xóa");
                return;
            }

            int index = dgvDetails.SelectedRows[0].Index;
            if (index >= 0 && index < _details.Count)
            {
                _details.RemoveAt(index);
                RefreshDetails();
            }
        }

        private void BtnSaveTransaction_Click(object sender, EventArgs e)
        {
            if (_details.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm");
                return;
            }

            try
            {
                foreach (var (productId, quantity, unitPrice) in _details)
                {
                    if (_transactionType == "Import")
                    {
                        _inventoryController.Import(productId, quantity, unitPrice, txtNote.Text);
                    }
                    else
                    {
                        _inventoryController.Export(productId, quantity, unitPrice, txtNote.Text);
                    }
                }

                MessageBox.Show("Lưu phiếu thành công!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }        }
    }
}