using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;

namespace WarehouseManagement.Views.Forms
{
    public class InventoryCheckForm : Form
    {
        private InventoryCheck _check;
        private bool _isNew;
        private InventoryCheckController _controller;
        private ProductController _productController;

        private CustomComboBox cmbProduct;
        private CustomTextArea txtNote;
        private DataGridView dgvDetails;
        private CustomButton btnAddProduct, btnSave, btnComplete, btnClose;
        
        private List<InventoryCheckDetail> _detailsList;

        public InventoryCheckForm(InventoryCheck check = null)
        {
            _check = check;
            _isNew = (check == null);
            _controller = new InventoryCheckController();
            _productController = new ProductController();
            _detailsList = new List<InventoryCheckDetail>();

            if (!_isNew)
            {
                _detailsList = check.Details ?? new List<InventoryCheckDetail>();
            }

            InitializeComponent();
            ThemeManager.Instance.ApplyThemeToForm(this);
        }

        private void InitializeComponent()
        {
            Text = _isNew ? $"{UIConstants.Icons.Check} Tạo Phiếu Kiểm Kê Mới" : $"{UIConstants.Icons.Check} Chi Tiết Phiếu Kiểm Kê #{_check.CheckID}";
            Size = new Size(800, 700);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = ThemeManager.Instance.BackgroundLight;

            int margin = 20;
            int y = 20;

            // Info Header
            Label lblUser = CreateLabel($"Người tạo: {(_isNew ? GlobalUser.CurrentUser?.Username : _check.CreatedByUserID.ToString())}", margin, y, 300);
            Label lblDate = CreateLabel($"Ngày tạo: {(_isNew ? DateTime.Now.ToString("dd/MM/yyyy HH:mm") : _check.CheckDate.ToString("dd/MM/yyyy HH:mm"))}", margin + 350, y, 300);
            
            Controls.Add(lblUser);
            Controls.Add(lblDate);
            y += 30;

            if (!_isNew)
            {
                Label lblStatus = CreateLabel($"Trạng thái: {_check.Status}", margin, y, 300);
                if (_check.Status == "Completed") lblStatus.ForeColor = UIConstants.SemanticColors.Success;
                else lblStatus.ForeColor = UIConstants.SemanticColors.Warning;
                Controls.Add(lblStatus);
                y += 30;
            }

            // Note
            Controls.Add(CreateLabel("Ghi chú:", margin, y, 100));
            y += 25;
            txtNote = new CustomTextArea
            {
                Left = margin,
                Top = y,
                Width = 740,
                Height = 60,
                Placeholder = "Ghi chú kiểm kê...",
                ReadOnly = (!_isNew)
            };
            if (!_isNew) txtNote.Text = _check.Note;
            Controls.Add(txtNote);
            y += 70;

            // Product Selection (Only for New)
            if (_isNew)
            {
                Controls.Add(CreateLabel("Thêm sản phẩm:", margin, y, 150));
                y += 25;

                cmbProduct = new CustomComboBox
                {
                    Left = margin,
                    Top = y,
                    Width = 600
                };
                LoadProducts();

                btnAddProduct = new CustomButton
                {
                    Text = "Thêm",
                    Left = margin + 610,
                    Top = y,
                    Width = 130,
                    ButtonStyleType = ButtonStyle.Filled
                };
                btnAddProduct.Click += BtnAddProduct_Click;

                Controls.Add(cmbProduct);
                Controls.Add(btnAddProduct);
                y += 45;
            }

            // Grid
            Controls.Add(CreateLabel("Chi tiết kiểm kê:", margin, y, 200));
            y += 25;

            dgvDetails = new DataGridView
            {
                Left = margin,
                Top = y,
                Width = 740,
                Height = 350,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                BackgroundColor = ThemeManager.Instance.BackgroundDefault,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Setup Columns
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "ProductID", Width = 60, ReadOnly = true });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên sản phẩm", Name = "ProductName", Width = 200, ReadOnly = true }); // Need to map name manually or via property
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tồn Máy", DataPropertyName = "SystemQuantity", Width = 90, ReadOnly = true });
            
            var colActual = new DataGridViewTextBoxColumn { HeaderText = "Tồn Thực", DataPropertyName = "ActualQuantity", Width = 90 };
            colActual.DefaultCellStyle.BackColor = _isNew ? Color.White : Color.WhiteSmoke;
            colActual.ReadOnly = !_isNew; 
            dgvDetails.Columns.Add(colActual);

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Chênh lệch", DataPropertyName = "Difference", Width = 90, ReadOnly = true });
            
            var colReason = new DataGridViewTextBoxColumn { HeaderText = "Lý do", DataPropertyName = "Reason", Width = 150 };
            colReason.ReadOnly = !_isNew;
            dgvDetails.Columns.Add(colReason);

            dgvDetails.CellEndEdit += DgvDetails_CellEndEdit;
            dgvDetails.CellFormatting += DgvDetails_CellFormatting;

            Controls.Add(dgvDetails);
            y += 360;

            // Buttons
            btnClose = new CustomButton
            {
                Text = "Đóng",
                Left = margin + 640,
                Top = y,
                Width = 100,
                ButtonStyleType = ButtonStyle.Text
            };
            btnClose.Click += (s, e) => Close();
            Controls.Add(btnClose);

            if (_isNew)
            {
                btnSave = new CustomButton
                {
                    Text = "Lưu Nháp (Pending)",
                    Left = margin,
                    Top = y,
                    Width = 180,
                    ButtonStyleType = ButtonStyle.Outlined
                };
                btnSave.Click += BtnSave_Click;
                Controls.Add(btnSave);

                btnComplete = new CustomButton
                {
                    Text = "Hoàn Tất & Cân Bằng",
                    Left = margin + 190,
                    Top = y,
                    Width = 200,
                    ButtonStyleType = ButtonStyle.Filled
                };
                btnComplete.Click += BtnComplete_Click;
                Controls.Add(btnComplete);
            }
            else if (_check.Status == "Pending")
            {
                btnComplete = new CustomButton
                {
                    Text = "Hoàn Tất & Cân Bằng",
                    Left = margin,
                    Top = y,
                    Width = 200,
                    ButtonStyleType = ButtonStyle.Filled
                };
                btnComplete.Click += BtnComplete_Click;
                Controls.Add(btnComplete);
            }

            RefreshGrid();
        }

        private Label CreateLabel(string text, int x, int y, int width)
        {
            return new Label
            {
                Text = text,
                Left = x, 
                Top = y,
                Width = width,
                Font = ThemeManager.Instance.FontRegular,
                AutoSize = false
            };
        }

        private void LoadProducts()
        {
            try
            {
                var products = _productController.GetAllProducts();
                cmbProduct.DataSource = products;
                cmbProduct.DisplayMember = "ProductName";
                cmbProduct.ValueMember = "ProductID";
                cmbProduct.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sản phẩm: " + ex.Message);
            }
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex < 0) return;
            
            int productId = (int)cmbProduct.SelectedValue;
            if (_detailsList.Any(d => d.ProductID == productId))
            {
                MessageBox.Show("Sản phẩm đã có trong danh sách");
                return;
            }

            var product = _productController.GetProductById(productId);
            if (product == null) return;

            var detail = new InventoryCheckDetail
            {
                ProductID = productId,
                SystemQuantity = product.Quantity,
                ActualQuantity = product.Quantity, // Default to same
                Difference = 0,
                Reason = ""
            };
            _detailsList.Add(detail);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dgvDetails.DataSource = null;
            dgvDetails.DataSource = _detailsList;
        }

        private void DgvDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _detailsList.Count)
            {
                var detail = _detailsList[e.RowIndex];
                if (dgvDetails.Columns[e.ColumnIndex].Name == "ProductName")
                {
                    // Look up name? We passed IDs. 
                    // Better to load names into Dictionary or fetch on fly.
                    // For now, let's fetch on fly (might be slow) or assume we populated it.
                    // Actually, let's use a workaround:
                    // If we are in New Mode, valid products are in ComboBox or we fetched them.
                    
                    // Optimization: Pre-fetch all names or use ProductController cache if it had one.
                    // Simple way: Fetch
                    var product = _productController.GetProductById(detail.ProductID);
                    e.Value = product?.ProductName ?? "Unknown";
                    e.FormattingApplied = true;
                }
                
                if (dgvDetails.Columns[e.ColumnIndex].DataPropertyName == "Difference")
                {
                   int diff = detail.Difference;
                   if (diff > 0) e.CellStyle.ForeColor = UIConstants.SemanticColors.Success; // Found more
                   else if (diff < 0) e.CellStyle.ForeColor = UIConstants.SemanticColors.Error; // Missing
                }
            }
        }

        private void DgvDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Recalc difference if ActualQuantity changed
            if (dgvDetails.Columns[e.ColumnIndex].DataPropertyName == "ActualQuantity")
            {
                var detail = _detailsList[e.RowIndex];
                detail.Difference = detail.ActualQuantity - detail.SystemQuantity;
                RefreshGrid(); // Refresh to update Difference column
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveCheck("Pending");
        }

        private void BtnComplete_Click(object sender, EventArgs e)
        {
             if (MessageBox.Show("Bạn có chắc chắn muốn hoàn tất phiếu kiểm kê này?\nHệ thống sẽ tự động tạo các phiếu Nhập/Xuất để cân bằng tồn kho.", 
                 "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
             {
                 if (_isNew)
                 {
                     SaveCheck("Completed");
                 }
                 else
                 {
                     // Complete existing Pending check
                     try
                     {
                        _controller.CompleteCheck(_check.CheckID, GlobalUser.CurrentUser?.UserID ?? 0);
                        MessageBox.Show("Hoàn tất kiểm kê thành công!");
                        DialogResult = DialogResult.OK;
                        Close();
                     }
                     catch(Exception ex)
                     {
                         MessageBox.Show("Lỗi: " + ex.Message);
                     }
                 }
             }
        }

        private void SaveCheck(string status)
        {
            if (_detailsList.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm");
                return;
            }

            try
            {
                string note = txtNote.Text;
                int userId = GlobalUser.CurrentUser?.UserID ?? 0;
                
                int checkId = _controller.CreateCheck(userId, note, _detailsList, status);
                
                MessageBox.Show($"Đã lưu phiếu kiểm kê #{checkId} ({status})");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu phiếu: " + ex.Message);
            }
        }
    }
}
