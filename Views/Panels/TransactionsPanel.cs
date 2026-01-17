using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Views.Forms;

namespace WarehouseManagement.Views.Panels
{
    public class TransactionsPanel : Panel, ISearchable
    {
        private DataGridView dgvTransactions;
        private InventoryController _inventoryController;
        private List<StockTransaction> allTransactions;

        public TransactionsPanel()
        {
            _inventoryController = new InventoryController();
            InitializeComponent();
            SettingsForm.SettingsChanged += (s, e) => LoadData();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;

            dgvTransactions = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White
            };

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID Phiếu", DataPropertyName = "TransactionID", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Loại", DataPropertyName = "Type", Width = 60, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ngày", DataPropertyName = "DateCreated", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tổng Giá Trị", DataPropertyName = "TotalValue", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "C", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ghi chú", DataPropertyName = "Note", Width = 250 });
            dgvTransactions.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Ẩn", Width = 50, UseColumnTextForButtonValue = true, Text = "👁️" });

            dgvTransactions.CellDoubleClick += DgvTransactions_CellDoubleClick;
            dgvTransactions.CellClick += DgvTransactions_CellClick;
            dgvTransactions.CellFormatting += DgvTransactions_CellFormatting;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.VisibleChanged += (s, e) =>
            {
                if (this.Visible)
                    LoadData();
            };

            Controls.Add(dgvTransactions);
        }

        public void LoadData()
        {
            try
            {
                allTransactions = _inventoryController.GetAllTransactions(SettingsForm.ShowHiddenItems);
                dgvTransactions.DataSource = allTransactions;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải giao dịch: " + ex.Message);
            }
        }

        public void Search(string searchText)
        {
            try
            {
                string search = searchText.ToLower();
                List<StockTransaction> filtered = allTransactions.FindAll(t => 
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
            int transactionId = (int)dgvTransactions.Rows[e.RowIndex].Cells[0].Value;

            try
            {
                StockTransaction transaction = _inventoryController.GetTransactionById(transactionId);

                if (transaction != null)
                {
                    TransactionDetailForm form = new TransactionDetailForm(transaction);
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy giao dịch");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải giao dịch: " + ex.Message);
            }
        }

        private void DgvTransactions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Column 5 is the hide button
            if (e.ColumnIndex == 5)
            {
                int transactionId = (int)dgvTransactions.Rows[e.RowIndex].Cells[0].Value;
                
                DialogResult result = MessageBox.Show(
                    "Bạn chắc chắn muốn đảo trạng thái giao dịch này?",
                    "Xác nhận đảo trạng thái",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _inventoryController.HideTransaction(transactionId);
                        MessageBox.Show("Trạng thái giao dịch đã được thay đổi.");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ẩn giao dịch: " + ex.Message);
                    }
                }
                return;
            }

            // Other columns show details
            int id = (int)dgvTransactions.Rows[e.RowIndex].Cells[0].Value;

            try
            {
                StockTransaction transaction = _inventoryController.GetTransactionById(id);
                if (transaction != null)
                {
                    TransactionDetailForm form = new TransactionDetailForm(transaction);
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy giao dịch");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải giao dịch: " + ex.Message);
            }
        }

        private void DgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Format Type column to show Vietnamese names
            if (e.ColumnIndex == 1 && dgvTransactions.Rows[e.RowIndex].DataBoundItem is StockTransaction transaction)
            {
                if (e.Value != null)
                {
                    e.Value = transaction.Type == "Import" ? "Nhập" : transaction.Type == "Export" ? "Xuất" : transaction.Type;
                    e.FormattingApplied = true;
                }
            }
        }
    }
}