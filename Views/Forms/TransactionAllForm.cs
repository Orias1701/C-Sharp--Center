using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;

namespace WarehouseManagement.Views.Forms
{
    /// <summary>
    /// Form Tạo phiếu Nhập/Xuất kho
    /// </summary>
    public partial class TransactionAllForm : Form
    {
        private string _transactionType; // "Import" hoặc "Export"
        private InventoryController _inventoryController;
        private ProductController _productController;
        private ComboBox cmbProduct;
        private TextBox txtQuantity, txtUnitPrice, txtNote;
        private DataGridView dgvDetails;
        private Button btnAddDetail, btnRemoveDetail, btnSaveTransaction, btnCancel;
        private List<(int ProductID, int Quantity, decimal UnitPrice)> _details;

        public TransactionAllForm(string type)
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

            // Layout standard: Label 100px, Input 300px, spacing 20px
            const int LABEL_WIDTH = 100;
            const int INPUT_WIDTH = 300;
            const int LABEL_LEFT = 20;
            const int INPUT_LEFT = 130;
            const int ITEM_SPACING = 40;
            const int BUTTON_WIDTH = 100;
            const int BUTTON_HEIGHT = 35;

            // Labels và controls
            Label lblProduct = new Label { Text = "Sản phẩm:", Left = LABEL_LEFT, Top = 20, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            cmbProduct = new ComboBox { Left = INPUT_LEFT, Top = 20, Width = INPUT_WIDTH, Height = 25, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblQuantity = new Label { Text = "Số lượng:", Left = LABEL_LEFT, Top = 20 + ITEM_SPACING, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtQuantity = new TextBox { Left = INPUT_LEFT, Top = 20 + ITEM_SPACING, Width = 140, Height = 25 };

            Label lblPrice = new Label { Text = "Đơn giá:", Left = LABEL_LEFT + 160, Top = 20 + ITEM_SPACING, Width = 60, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            txtUnitPrice = new TextBox { Left = LABEL_LEFT + 230, Top = 20 + ITEM_SPACING, Width = 130, Height = 25 };

            Label lblNote = new Label { Text = "Ghi chú:", Left = LABEL_LEFT, Top = 20 + ITEM_SPACING * 2, Width = LABEL_WIDTH, AutoSize = false, TextAlign = System.Drawing.ContentAlignment.TopLeft };
            txtNote = new TextBox { Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 2, Width = INPUT_WIDTH, Height = 50, Multiline = true };

            btnAddDetail = new Button { Text = "➕ Thêm", Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 3 + 20, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };
            btnRemoveDetail = new Button { Text = "🗑️ Xóa", Left = INPUT_LEFT + BUTTON_WIDTH + 10, Top = 20 + ITEM_SPACING * 3 + 20, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };

            btnAddDetail.Click += BtnAddDetail_Click;
            btnRemoveDetail.Click += BtnRemoveDetail_Click;

            // DataGridView
            dgvDetails = new DataGridView
            {
                Left = LABEL_LEFT,
                Top = 20 + ITEM_SPACING * 4 + 30,
                Width = 520,
                Height = 180,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true
            };

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Sản phẩm", DataPropertyName = "ProductName", Width = 250 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số lượng", DataPropertyName = "Quantity", Width = 80 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đơn giá", DataPropertyName = "UnitPrice", Width = 140, DefaultCellStyle = new DataGridViewCellStyle { Format = "C" } });

            btnSaveTransaction = new Button { Text = "💾 Lưu Phiếu", Left = INPUT_LEFT, Top = 20 + ITEM_SPACING * 4 + 220, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };
            btnCancel = new Button { Text = "❌ Hủy", Left = INPUT_LEFT + BUTTON_WIDTH + 10, Top = 20 + ITEM_SPACING * 4 + 220, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT, DialogResult = DialogResult.Cancel };
            Button btnExportVoucher = new Button { Text = "📄 Xuất Phiếu", Left = INPUT_LEFT + (BUTTON_WIDTH + 10) * 2, Top = 20 + ITEM_SPACING * 4 + 220, Width = BUTTON_WIDTH, Height = BUTTON_HEIGHT };

            btnSaveTransaction.Click += BtnSaveTransaction_Click;
            btnExportVoucher.Click += BtnExportVoucher_Click;

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
            Controls.Add(btnExportVoucher);
            Controls.Add(btnCancel);
            Controls.Add(dgvDetails);

            Width = 600;
            Height = 580;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            CancelButton = btnCancel;

            Load += TransactionAllForm_Load;
            ResumeLayout(false);
        }

        private void BtnExportVoucher_Click(object sender, EventArgs e)
        {
            if (_details.Count == 0)
            {
                MessageBox.Show("❌ Vui lòng thêm ít nhất 1 sản phẩm trước khi xuất phiếu");
                return;
            }

            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv",
                    DefaultExt = "txt",
                    FileName = $"Phieu_{_transactionType}_{DateTime.Now:yyyyMMdd_HHmmss}"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportVoucherToFile(saveDialog.FileName);
                    MessageBox.Show("Xuất phiếu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất phiếu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportVoucherToFile(string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Header
                writer.WriteLine("╔═══════════════════════════════════════════════╗");
                writer.WriteLine($"║ PHIẾU {(_transactionType == "Import" ? "NHẬP KHO" : "XUẤT KHO"),-42} ║");
                writer.WriteLine("╚═══════════════════════════════════════════════╝");
                writer.WriteLine();

                // Thông tin phiếu
                writer.WriteLine($"Ngày tạo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                writer.WriteLine($"Loại phiếu: {(_transactionType == "Import" ? "Nhập kho" : "Xuất kho")}");
                if (!string.IsNullOrEmpty(txtNote.Text))
                {
                    writer.WriteLine($"Ghi chú: {txtNote.Text}");
                }
                writer.WriteLine();

                // Chi tiết
                writer.WriteLine("┌─────────────────────────────────────────────────┐");
                writer.WriteLine("│ CHI TIẾT PHIẾU                                  │");
                writer.WriteLine("├─────────────────────────────────────────────────┤");

                decimal totalAmount = 0;
                foreach (var detail in _details)
                {
                    var product = _productController.GetProductById(detail.ProductID);
                    decimal amount = detail.Quantity * detail.UnitPrice;
                    totalAmount += amount;

                    writer.WriteLine($"│ Sản phẩm: {product?.ProductName ?? "N/A",-35} │");
                    writer.WriteLine($"│   Số lượng: {detail.Quantity,-38} │");
                    writer.WriteLine($"│   Đơn giá: {detail.UnitPrice:N0} ₫{"",-31} │");
                    writer.WriteLine($"│   Thành tiền: {amount:N0} ₫{"",-25} │");
                    writer.WriteLine("├─────────────────────────────────────────────────┤");
                }

                writer.WriteLine($"│ TỔNG CỘNG: {totalAmount:N0} ₫{"",-21} │");
                writer.WriteLine("└─────────────────────────────────────────────────┘");
                writer.WriteLine();
                writer.WriteLine($"In lúc: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            }
        }

        private void TransactionAllForm_Load(object sender, EventArgs e)
        {
            try
            {
                List<Product> products = _productController.GetAllProducts();
            
            cmbProduct.DataSource = products;
            cmbProduct.DisplayMember = "ProductName";
            cmbProduct.ValueMember = "ProductID";

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
                MessageBox.Show("❌ Vui lòng chọn sản phẩm");
                cmbProduct.Focus();
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("❌ Số lượng phải là số nguyên");
                txtQuantity.Focus();
                return;
            }

            if (quantity <= 0)
            {
                MessageBox.Show("❌ Số lượng phải lớn hơn 0");
                txtQuantity.Focus();
                return;
            }

            if (quantity > 999999)
            {
                MessageBox.Show("❌ Số lượng quá lớn (tối đa: 999,999)");
                txtQuantity.Focus();
                return;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price))
            {
                MessageBox.Show("❌ Đơn giá phải là số");
                txtUnitPrice.Focus();
                return;
            }

            if (price < 0)
            {
                MessageBox.Show("❌ Đơn giá không được âm");
                txtUnitPrice.Focus();
                return;
            }

            if (price > 999999999)
            {
                MessageBox.Show("❌ Đơn giá quá lớn (tối đa: 999,999,999)");
                txtUnitPrice.Focus();
                return;
            }

            if (cmbProduct.SelectedValue == null)
            {
                MessageBox.Show("❌ Vui lòng chọn sản phẩm hợp lệ từ danh sách");
                cmbProduct.Focus();
                return;
            }

            int productId = (int)cmbProduct.SelectedValue;
            
            // Kiểm tra tồn kho nếu là Xuất
            if (_transactionType == "Export")
            {
                Product product = _productController.GetProductById(productId);
                if (product == null)
                {
                    MessageBox.Show("❌ Không tìm thấy thông tin sản phẩm");
                    return;
                }
                if (product.Quantity < quantity)
                {
                    MessageBox.Show($"❌ Tồn kho không đủ (hiện có: {product.Quantity})");
                    txtQuantity.Focus();
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
                MessageBox.Show("❌ Vui lòng chọn dòng để xóa");
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
                MessageBox.Show("❌ Vui lòng thêm ít nhất một sản phẩm");
                return;
            }

            try
            {
                if (_transactionType == "Import")
                {
                    _inventoryController.ImportBatch(_details, txtNote.Text);
                }
                else
                {
                    _inventoryController.ExportBatch(_details, txtNote.Text);
                }
                MessageBox.Show("✅ Lưu phiếu thành công!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi: " + ex.Message);
            }
        }
    }
}

