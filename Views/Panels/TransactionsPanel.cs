using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Views.Forms;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;
using System.Linq;

namespace WarehouseManagement.Views.Panels
{
    public class TransactionsPanel : Panel, ISearchable
    {
        private DataGridView dgvTransactions;
        private InventoryController _inventoryController;
        private SupplierController _supplierController;
        private CustomerController _customerController;
        private List<Transaction> allTransactions;
        private Dictionary<int, string> _supplierNames = new Dictionary<int, string>();
        private Dictionary<int, string> _customerNames = new Dictionary<int, string>();

        public TransactionsPanel()
        {
            _inventoryController = new InventoryController();
            _supplierController = new SupplierController();
            _customerController = new CustomerController();
            InitializeComponent();
            SettingsForm.SettingsChanged += (s, e) => LoadData();
            
            ThemeManager.Instance.ThemeChanged += OnThemeChanged;
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = ThemeManager.Instance.BackgroundDefault;

            dgvTransactions = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = ThemeManager.Instance.BackgroundDefault,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle 
                { 
                    BackColor = UIConstants.PrimaryColor.Default,
                    ForeColor = Color.White,
                    Font = ThemeManager.Instance.FontBold
                },
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToResizeRows = false,
                Font = ThemeManager.Instance.FontRegular,
                RowTemplate = { Height = UIConstants.Sizes.TableRowHeight },
                ColumnHeadersHeight = UIConstants.Sizes.TableHeaderHeight
            };

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "ID", 
                DataPropertyName = "TransactionID", 
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(10, 5, 30, 5)
                } 
            });
            
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Loại", 
                DataPropertyName = "Type", 
                Width = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(5)
                } 
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Đối tác", 
                Name = "colPartner", // Used in CellFormatting
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 5, 30, 5)
                } 
            });
            
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Ngày", 
                DataPropertyName = "DateCreated", 
                Width = 140,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "dd/MM/yyyy HH:mm",
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(5)
                } 
            });
            
            // Total/Discount/Final Columns
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Tổng Tiền", 
                DataPropertyName = "TotalAmount", 
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "N0", 
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Padding = new Padding(5)
                } 
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Chiết Khấu", 
                DataPropertyName = "Discount", 
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "N0", 
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Padding = new Padding(5)
                } 
            });

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Thành Tiền", 
                DataPropertyName = "FinalAmount", 
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Format = "N0", 
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Padding = new Padding(5)
                } 
            });
             
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Ghi chú", 
                DataPropertyName = "Note", 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Padding = new Padding(5)
                }
            });

            // Status Column (Moved here)
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Trạng Thái", 
                DataPropertyName = "Status", 
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle 
                { 
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(5)
                } 
            });
            
            // ACTIONS COLUMN REMOVED
            
            foreach (DataGridViewColumn col in dgvTransactions.Columns)
            {
                if (col.DefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
                {
                    col.HeaderCell.Style.Alignment = col.DefaultCellStyle.Alignment;
                }
                col.HeaderCell.Style.Padding = new Padding(10, 5, 10, 5);
            }

            dgvTransactions.CellDoubleClick += DgvTransactions_CellDoubleClick;
            dgvTransactions.CellClick += DgvTransactions_CellClick;
            dgvTransactions.CellFormatting += DgvTransactions_CellFormatting;
            dgvTransactions.VisibleChanged += (s, e) =>
            {
                if (this.Visible)
                    LoadData();
            };
            
            // Hand cursor for Status column to indicate clickability
            dgvTransactions.CellMouseEnter += (s, e) => {
                if (e.RowIndex >= 0 && dgvTransactions.Columns[e.ColumnIndex].DataPropertyName == "Status")
                {
                    var trans = dgvTransactions.Rows[e.RowIndex].DataBoundItem as Transaction;
                    if (trans != null && trans.Status == "Pending")
                        dgvTransactions.Cursor = Cursors.Hand;
                }
            };
            dgvTransactions.CellMouseLeave += (s, e) => {
                dgvTransactions.Cursor = Cursors.Default;
            };

            CustomPanel tablePanel = new CustomPanel
            {
                Dock = DockStyle.Fill,
                HasShadow = true,
                ShowBorder = false,
                Padding = new Padding(10),
                BorderRadius = UIConstants.Borders.RadiusMedium,
                BackColor = ThemeManager.Instance.BackgroundDefault
            };
            tablePanel.Controls.Add(dgvTransactions);
            Controls.Add(tablePanel);
            
            // Apply Hover Effect
            Helpers.DataGridViewHelper.ApplyHoverEffect(dgvTransactions);
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            BackColor = ThemeManager.Instance.BackgroundDefault;
            dgvTransactions.BackgroundColor = ThemeManager.Instance.BackgroundDefault;
            dgvTransactions.DefaultCellStyle.BackColor = ThemeManager.Instance.BackgroundDefault;
            dgvTransactions.DefaultCellStyle.ForeColor = ThemeManager.Instance.TextPrimary;
            dgvTransactions.DefaultCellStyle.ForeColor = ThemeManager.Instance.TextPrimary;
            
            // Apply Selection Effect through Helper
            Helpers.DataGridViewHelper.ApplySelectionEffect(dgvTransactions);
            
            dgvTransactions.ColumnHeadersDefaultCellStyle.BackColor = UIConstants.PrimaryColor.Default;
            dgvTransactions.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }

        public void LoadData()
        {
            try
            {
                // Pre-load lookup data
                var suppliers = _supplierController.GetAllSuppliers();
                _supplierNames = suppliers.ToDictionary(s => s.SupplierID, s => s.SupplierName);

                var customers = _customerController.GetAllCustomers();
                _customerNames = customers.ToDictionary(c => c.CustomerID, c => c.CustomerName);

                allTransactions = _inventoryController.GetAllTransactions(SettingsForm.ShowHiddenItems);
                dgvTransactions.DataSource = allTransactions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{UIConstants.Icons.Error} Lỗi tải giao dịch: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Search(string searchText)
        {
            try
            {
                string search = searchText.ToLower();
                List<Transaction> filtered = allTransactions.FindAll(t => 
                    t.TransactionID.ToString().Contains(search) || 
                    t.Type.ToLower().Contains(search) || 
                    (t.Note != null && t.Note.ToLower().Contains(search)));
                dgvTransactions.DataSource = filtered;
            }
            catch { }
        }

        private void DgvTransactions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            // Always allow opening detail
            int transactionId = (int)dgvTransactions.Rows[e.RowIndex].Cells[0].Value;
            OpenTransactionDetail(transactionId);
        }

        private void DgvTransactions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var transaction = dgvTransactions.Rows[e.RowIndex].DataBoundItem as Transaction;
            if (transaction == null) return;

            // Handle Status Click
            if (dgvTransactions.Columns[e.ColumnIndex].DataPropertyName == "Status")
            {
                if (transaction.Status == "Pending")
                {
                    using (var dialog = new StatusActionDialog(
                        "Xử lý Giao dịch", 
                        $"Bạn muốn xử lý giao dịch #{transaction.TransactionID} như thế nào?"))
                    {
                        var result = dialog.ShowDialog();
                        if (result == DialogResult.Yes) // Approve
                        {
                            try
                            {
                                _inventoryController.ApproveTransaction(transaction.TransactionID);
                                LoadData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (result == DialogResult.No) // Cancel
                        {
                            try
                            {
                                _inventoryController.CancelTransaction(transaction.TransactionID);
                                LoadData();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        // Cancel/Exit does nothing
                    }
                }
                return; // Stop further processing
            }

            // Click elsewhere (except specific action columns if any) opens detail (except if click on check box etc is handled)
            // Currently simplified: any other click opens detail? 
            // It's safer to rely on DoubleClick for Details, but commonly user wants single click selection, double click detail.
            // But previous code opened detail on 'else'. Let's maintain that behavior for now unless it conflicts.
            // Just be careful not to trigger it when selecting rows.
            // e.ColumnIndex handles specific columns.
            // Let's rely on DoubleClick for Details to follow standard Windows app behavior, 
            // UNLESS user specifically wanted single click drilldown.
            // Given "DgvTransactions_CellDoubleClick" exists, maybe single click was just for Actions?
            // "else { OpenTransactionDetail }" was in previous code. Let's keep it but maybe restricted?
            // Actually, usually detail opening is on ID or distinct button. 
            // User feedback: "Why 3 clicks?" -> They expect single click or easier access. Restoring single click.
             OpenTransactionDetail(transaction.TransactionID); 
        }

        private void OpenTransactionDetail(int transactionId)
        {
             try
            {
                Transaction transaction = _inventoryController.GetTransactionById(transactionId);

                if (transaction != null)
                {
                    TransactionDetailForm form = new TransactionDetailForm(transaction);
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show($"{UIConstants.Icons.Error} Không tìm thấy giao dịch", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{UIConstants.Icons.Error} Lỗi tải giao dịch: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvTransactions.Rows.Count) return;

            var transaction = dgvTransactions.Rows[e.RowIndex].DataBoundItem as Transaction;
            if (transaction == null) return;

            string colName = dgvTransactions.Columns[e.ColumnIndex].Name;

            // Type Column
            if (dgvTransactions.Columns[e.ColumnIndex].DataPropertyName == "Type")
            {
                if (e.Value != null)
                {
                    string text = transaction.Type == "Import" ? "Nhập" : transaction.Type == "Export" ? "Xuất" : transaction.Type;
                    e.Value = text;
                    e.FormattingApplied = true;
                    
                    if (transaction.Type == "Import")
                        e.CellStyle.ForeColor = UIConstants.SemanticColors.Success;
                    else if (transaction.Type == "Export")
                        e.CellStyle.ForeColor = UIConstants.SemanticColors.Info;
                }
            }
            // Status Column
            else if (dgvTransactions.Columns[e.ColumnIndex].DataPropertyName == "Status")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    if (status == "Approved")
                    {
                        e.Value = "Đã duyệt";
                        e.CellStyle.ForeColor = UIConstants.SemanticColors.Success;
                    }
                    else if (status == "Cancelled")
                    {
                         e.Value = "Đã hủy";
                         e.CellStyle.ForeColor = UIConstants.SemanticColors.Error;
                    }
                    else
                    {
                        e.Value = "Đang chờ";
                        e.CellStyle.ForeColor = UIConstants.SemanticColors.Warning;
                    }
                    e.FormattingApplied = true;
                }
            }
            // Partner Column
            else if (colName == "colPartner")
            {
                if (transaction.Type == "Import" && transaction.SupplierID.HasValue)
                {
                     if (_supplierNames.TryGetValue(transaction.SupplierID.Value, out string name))
                     {
                         e.Value = name;
                         e.FormattingApplied = true;
                     }
                }
                else if (transaction.Type == "Export" && transaction.CustomerID.HasValue)
                {
                     if (_customerNames.TryGetValue(transaction.CustomerID.Value, out string name))
                     {
                         e.Value = name;
                         e.FormattingApplied = true;
                     }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeManager.Instance.ThemeChanged -= OnThemeChanged;
            }
            base.Dispose(disposing);
        }
    }
}
