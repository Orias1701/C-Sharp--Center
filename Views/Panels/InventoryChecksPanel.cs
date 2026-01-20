using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Views.Forms;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;

namespace WarehouseManagement.Views.Panels
{
    public class InventoryChecksPanel : Panel, ISearchable
    {
        private DataGridView dgvChecks;
        private InventoryCheckController _controller;
        private List<InventoryCheck> _allChecks;

        public InventoryChecksPanel()
        {
            _controller = new InventoryCheckController();
            InitializeComponent();
            ThemeManager.Instance.ThemeChanged += OnThemeChanged;
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            Padding = new Padding(20);

            // Top Panel (for future "Create Check" button)
            Panel topPanel = new Panel { Dock = DockStyle.Top, Height = 50 };
            
            // For now only View mode is requested as fully functional, but let's add placeholder Add button
            CustomButton btnAdd = new CustomButton
            {
                Text = $"{UIConstants.Icons.Add} Tạo Phiếu Kiểm Kê",
                Width = 200,
                Height = 35,
                Top = 5,
                Left = 0,
                ButtonStyleType = ButtonStyle.Filled
            };
            btnAdd.Click += (s,e) => {
                 InventoryCheckForm form = new InventoryCheckForm(null);
                 if (form.ShowDialog() == DialogResult.OK) {
                     LoadData();
                 }
            };
            topPanel.Controls.Add(btnAdd);
            Controls.Add(topPanel);

            // Grid
            dgvChecks = new DataGridView
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
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowTemplate = { Height = UIConstants.Sizes.TableRowHeight },
                ColumnHeadersHeight = UIConstants.Sizes.TableHeaderHeight
            };

            dgvChecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã Phiếu", DataPropertyName = "CheckID", Width = 100 });
            dgvChecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ngày Kiểm", DataPropertyName = "CheckDate", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dgvChecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Người Kiểm (ID)", DataPropertyName = "CreatedByUserID", Width = 120 });
            dgvChecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng Thái", DataPropertyName = "Status", Width = 120 });
            dgvChecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ghi Chú", DataPropertyName = "Note", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dgvChecks.CellDoubleClick += DgvChecks_CellDoubleClick;

            CustomPanel gridPanel = new CustomPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = UIConstants.Borders.RadiusMedium,
                ShowBorder = false,
                Padding = new Padding(10),
                BackColor = ThemeManager.Instance.BackgroundDefault
            };
            gridPanel.Controls.Add(dgvChecks);
            Controls.Add(gridPanel);
            
            topPanel.BringToFront();

            LoadData();
        }

        public void LoadData()
        {
            try
            {
                _allChecks = _controller.GetAllChecks();
                dgvChecks.DataSource = _allChecks;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu kiểm kê: " + ex.Message);
            }
        }

        public void Search(string text)
        {
            if (_allChecks == null) return;
            string keyword = text.ToLower();
            var filtered = _allChecks.FindAll(c => 
                c.CheckID.ToString().Contains(keyword) || 
                (c.Note != null && c.Note.ToLower().Contains(keyword)) ||
                c.Status.ToLower().Contains(keyword));
            dgvChecks.DataSource = filtered;
        }

        private void DgvChecks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var check = dgvChecks.Rows[e.RowIndex].DataBoundItem as InventoryCheck;
            if (check == null) return;

            // Load full check with details if needed (Repository might already fetch them or we check null)
            InventoryCheck fullCheck = _controller.GetCheckById(check.CheckID);
            
            InventoryCheckForm form = new InventoryCheckForm(fullCheck);
            form.ShowDialog();
        }

        private void OnThemeChanged(object sender, EventArgs e) => ApplyTheme();

        private void ApplyTheme()
        {
            BackColor = ThemeManager.Instance.BackgroundDefault;
            dgvChecks.BackgroundColor = ThemeManager.Instance.BackgroundDefault;
            dgvChecks.DefaultCellStyle.BackColor = ThemeManager.Instance.BackgroundDefault;
            dgvChecks.DefaultCellStyle.ForeColor = ThemeManager.Instance.TextPrimary;
            dgvChecks.ColumnHeadersDefaultCellStyle.BackColor = UIConstants.PrimaryColor.Default;
            dgvChecks.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }
    }
}
