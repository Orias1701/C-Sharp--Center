using System;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Models;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;
using WarehouseManagement.Controllers;

namespace WarehouseManagement.Views.Forms
{
    public partial class TransactionDetailForm : Form
    {
        private Transaction _transaction;
        private CustomTextBox txtTransactionID, txtType, txtDate, txtTotalValue, txtCreatedBy;
        private CustomTextArea txtNote;
        private DataGridView dgvDetails;
        private CustomButton btnClose;
        private SupplierController _supplierController;
        private CustomerController _customerController;
        private CustomTextBox txtPartner;

        public TransactionDetailForm(Transaction transaction)
        {
            InitializeComponent();
            _transaction = transaction;
            _supplierController = new SupplierController();
            _customerController = new CustomerController();
            Text = $"{UIConstants.Icons.FileText} Chi Tiết Giao Dịch #{transaction.TransactionID}";
            
            ThemeManager.Instance.ApplyThemeToForm(this);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            CustomPanel mainPanel = new CustomPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = UIConstants.Borders.RadiusLarge,
                ShowBorder = false,
                Padding = new Padding(0)
            };

            const int LEFT_MARGIN = 40;
            int currentY = 20;
            int spacing = UIConstants.Spacing.Margin.Medium;

            Label lblTransactionIDLabel = new Label 
            { 
                Text = "Mã Giao Dịch:",
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 100,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B)
            };

            Label lblTypeLabel = new Label 
            { 
                Text = "Loại Giao Dịch:",
                Left = LEFT_MARGIN + 250, 
                Top = currentY, 
                Width = 100,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B)
            };
            currentY += 20;

            txtTransactionID = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 220,
                ReadOnly = true,
                TabStop = false
            };

            txtType = new CustomTextBox 
            { 
                Left = LEFT_MARGIN + 250, 
                Top = currentY, 
                Width = 270,
                ReadOnly = true,
                TabStop = false
            };
            currentY += UIConstants.Sizes.InputHeight + spacing;

            Label lblPartnerLabel = new Label 
            { 
                Text = "Đối tác:",
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 100,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B)
            };
            
            txtPartner = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 520,
                ReadOnly = true,
                TabStop = false
            };
            currentY += UIConstants.Sizes.InputHeight + spacing;

            Label lblDateLabel = new Label 
            { 
                Text = "Ngày Tạo:",
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 100,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B)
            };

            Label lblCreatedByLabel = new Label 
            { 
                Text = "Người Tạo:",
                Left = LEFT_MARGIN + 250, 
                Top = currentY, 
                Width = 100,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B)
            };
            currentY += 20;

            txtDate = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 220,
                ReadOnly = true,
                TabStop = false
            };

            txtCreatedBy = new CustomTextBox 
            { 
                Left = LEFT_MARGIN + 250, 
                Top = currentY, 
                Width = 270,
                ReadOnly = true,
                TabStop = false
            };
            currentY += UIConstants.Sizes.InputHeight + spacing;

            Label lblTotalValueLabel = new Label 
            { 
                Text = "Tổng Giá Trị:",
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 100,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B)
            };
            currentY += 20;

            txtTotalValue = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 520,
                ReadOnly = true,
                TabStop = false
            };
            currentY += UIConstants.Sizes.InputHeight + spacing;

            Label lblNoteLabel = new Label 
            { 
                Text = "Ghi chú:",
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 100,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B)
            };
            currentY += 20;

            txtNote = new CustomTextArea 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 520, 
                Height = 60,
                ReadOnly = true,
                TabStop = false
            };
            currentY += 60 + spacing + 10;

            Label lblDetailsTitle = new Label
            {
                Text = $"{UIConstants.Icons.List} Chi Tiết Sản Phẩm:",
                Left = LEFT_MARGIN,
                Top = currentY,
                Width = 200,
                Font = ThemeManager.Instance.FontBold
            };
            currentY += 25;

            dgvDetails = new DataGridView
            {
                Left = LEFT_MARGIN,
                Top = currentY,
                Width = 520,
                Height = 180,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = ThemeManager.Instance.BackgroundDefault,
                BorderStyle = BorderStyle.FixedSingle
            };

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Sản phẩm", DataPropertyName = "ProductName", Width = 200 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số lượng", DataPropertyName = "Quantity", Width = 90 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đơn giá", DataPropertyName = "UnitPrice", Width = 110, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thành tiền", DataPropertyName = "SubTotal", Width = 110, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            // Note: DataPropertyName "SubTotal" matches TransactionDetail model. 
            // Was "TotalPrice" in UI but old model didn't have SubTotal, maybe it was calculated or property name? 
            // Checked TransactionDetail.cs, it has SubTotal. 
            // Old UI used TotalPrice. I need to make sure SubTotal is correct.
            
            currentY += 180 + spacing;

            btnClose = new CustomButton 
            { 
                Text = $"{UIConstants.Icons.Close} Đóng", 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = 120,
                ButtonStyleType = ButtonStyle.FilledNoOutline
            };
            btnClose.Click += (s, e) => {
                DialogResult = DialogResult.OK;
                Close();
            };

            mainPanel.Controls.Add(lblTransactionIDLabel);
            mainPanel.Controls.Add(txtTransactionID);
            mainPanel.Controls.Add(lblTypeLabel);
            mainPanel.Controls.Add(txtType);
            mainPanel.Controls.Add(lblPartnerLabel);
            mainPanel.Controls.Add(txtPartner);
            mainPanel.Controls.Add(lblDateLabel);
            mainPanel.Controls.Add(txtDate);
            mainPanel.Controls.Add(lblTotalValueLabel);
            mainPanel.Controls.Add(txtTotalValue);
            mainPanel.Controls.Add(lblCreatedByLabel);
            mainPanel.Controls.Add(txtCreatedBy);
            mainPanel.Controls.Add(lblNoteLabel);
            mainPanel.Controls.Add(txtNote);
            mainPanel.Controls.Add(lblDetailsTitle);
            mainPanel.Controls.Add(dgvDetails);
            mainPanel.Controls.Add(btnClose);

            Controls.Add(mainPanel);

            ClientSize = new Size(620, 635);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = ThemeManager.Instance.BackgroundLight;
            
            AcceptButton = btnClose;
            CancelButton = btnClose;

            Load += TransactionDetailForm_Load;
            ResumeLayout(false);
        }

        private void TransactionDetailForm_Load(object sender, EventArgs e)
        {
            try
            {
                txtTransactionID.Text = $"#{_transaction.TransactionID}";
                
                string typeIcon = _transaction.Type == "Import" ? UIConstants.Icons.Import : UIConstants.Icons.Export;
                string typeName = _transaction.Type == "Import" ? "Nhập" : "Xuất";
                txtType.Text = $"{typeIcon} {typeName}";
                
                txtDate.Text = _transaction.DateCreated.ToString("dd/MM/yyyy HH:mm");
                
                // Using TotalAmount or FinalAmount. Let's use FinalAmount (after discount) if available, 
                // or TotalAmount. The user requirement said FinalAmount is in Transaction.
                txtTotalValue.Text = $"{_transaction.FinalAmount:N0} ₫"; 
                
                txtCreatedBy.Text = $"User ID: {_transaction.CreatedByUserID}";
                txtCreatedBy.Text = $"User ID: {_transaction.CreatedByUserID}";
                
                // Load Partner Name
                string partnerName = "N/A";
                if (_transaction.Type == "Import" && _transaction.SupplierID.HasValue)
                {
                    var supplier = _supplierController.GetSupplierById(_transaction.SupplierID.Value);
                    partnerName = supplier != null ? $"Nhà cung cấp: {supplier.SupplierName}" : "N/A";
                }
                else if (_transaction.Type == "Export" && _transaction.CustomerID.HasValue)
                {
                    var customer = _customerController.GetCustomerById(_transaction.CustomerID.Value);
                    partnerName = customer != null ? $"Khách hàng: {customer.CustomerName}" : "N/A";
                }
                txtPartner.Text = partnerName;

                txtNote.Text = _transaction.Note ?? "(Không có ghi chú)";
                
                if (_transaction.Details != null && _transaction.Details.Count > 0)
                {
                    dgvDetails.DataSource = _transaction.Details;
                }
                
                ActiveControl = btnClose;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{UIConstants.Icons.Error} Lỗi tải chi tiết giao dịch: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
