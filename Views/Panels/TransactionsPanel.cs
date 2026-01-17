using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Views.Forms;

namespace WarehouseManagement.Views.Panels
{
    public class TransactionsPanel : Panel
    {
        private DataGridView dgvTransactions;
        private InventoryController _inventoryController;

        public TransactionsPanel()
        {
            _inventoryController = new InventoryController();
            InitializeComponent();
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

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID Phiáº¿u", DataPropertyName = "TransactionID", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Loáº¡i", DataPropertyName = "Type", Width = 60, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "NgÃ y", DataPropertyName = "DateCreated", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tá»•ng GiÃ¡ Trá»‹", DataPropertyName = "TotalValue", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "C", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ghi chÃº", DataPropertyName = "Note", Width = 320 });

            dgvTransactions.CellDoubleClick += DgvTransactions_CellDoubleClick;
            dgvTransactions.CellClick += DgvTransactions_CellClick;
            dgvTransactions.CellFormatting += DgvTransactions_CellFormatting;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Controls.Add(dgvTransactions);
        }

        public void LoadData()
        {
            try
            {
                List<StockTransaction> transactions = _inventoryController.GetAllTransactions();
                dgvTransactions.DataSource = transactions;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lá»—i táº£i giao dá»‹ch: " + ex.Message);
            }
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
                    MessageBox.Show("KhÃ´ng tÃ¬m tháº¥y giao dá»‹ch");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lá»—i táº£i giao dá»‹ch: " + ex.Message);
            }
        }

        private void DgvTransactions_CellClick(object sender, DataGridViewCellEventArgs e)
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
                    MessageBox.Show("KhÃ´ng tÃ¬m tháº¥y giao dá»‹ch");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lá»—i táº£i giao dá»‹ch: " + ex.Message);
            }
        }

        private void DgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Format Type column to show Vietnamese names
            if (e.ColumnIndex == 1 && dgvTransactions.Rows[e.RowIndex].DataBoundItem is StockTransaction transaction)
            {
                if (e.Value != null)
                {
                    e.Value = transaction.Type == "Import" ? "Nháº­p" : transaction.Type == "Export" ? "Xuáº¥t" : transaction.Type;
                    e.FormattingApplied = true;
                }
            }
        }
    }
}




