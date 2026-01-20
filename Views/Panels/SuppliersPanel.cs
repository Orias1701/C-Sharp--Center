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
    public class SuppliersPanel : Panel, ISearchable
    {
        private DataGridView dgvSuppliers;
        private SupplierController _controller;
        private List<Supplier> _allSuppliers;

        public SuppliersPanel()
        {
            _controller = new SupplierController();
            InitializeComponent();
            ThemeManager.Instance.ThemeChanged += OnThemeChanged;
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = ThemeManager.Instance.BackgroundDefault;
            Padding = new Padding(20);

            // Top Panel (Action Bar)
            Panel topPanel = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.Transparent };
            
            CustomButton btnAdd = new CustomButton
            {
                Text = $"{UIConstants.Icons.Add} Thêm Nhà Cung Cấp",
                Width = 220,
                Height = 35,
                Top = 5,
                Left = 0,
                ButtonStyleType = ButtonStyle.Filled
            };
            btnAdd.Click += BtnAdd_Click;
            topPanel.Controls.Add(btnAdd);
            Controls.Add(topPanel);

            // DataGridView
            dgvSuppliers = new DataGridView
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

            // Columns
            dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "ID", 
                DataPropertyName = "SupplierID", 
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Padding = new Padding(10, 5, 10, 5) }
            });
            
            dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Tên Nhà Cung Cấp", 
                DataPropertyName = "SupplierName", 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(10, 5, 10, 5) }
            });

            dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Số Điện Thoại", 
                DataPropertyName = "Phone", 
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(10, 5, 10, 5) }
            });

            dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Email", 
                DataPropertyName = "Email", 
                Width = 200,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(10, 5, 10, 5) }
            });

            dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                HeaderText = "Địa Chỉ", 
                DataPropertyName = "Address", 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { Padding = new Padding(10, 5, 10, 5) }
            });

            // Action Columns
            dgvSuppliers.Columns.Add(new DataGridViewLinkColumn 
            { 
                HeaderText = "", 
                Width = 60,
                UseColumnTextForLinkValue = true, 
                Text = UIConstants.Icons.Edit,
                LinkColor = ThemeManager.Instance.PrimaryDefault,
                ActiveLinkColor = ThemeManager.Instance.PrimaryDefault,
                VisitedLinkColor = ThemeManager.Instance.PrimaryDefault,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            
            dgvSuppliers.Columns.Add(new DataGridViewLinkColumn 
            { 
                HeaderText = "", 
                Width = 60,
                UseColumnTextForLinkValue = true, 
                Text = UIConstants.Icons.Delete,
                LinkColor = UIConstants.SemanticColors.Error,
                ActiveLinkColor = UIConstants.SemanticColors.Error,
                VisitedLinkColor = UIConstants.SemanticColors.Error,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Header Styling Sync
            foreach (DataGridViewColumn col in dgvSuppliers.Columns)
            {
                if (col.DefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
                    col.HeaderCell.Style.Alignment = col.DefaultCellStyle.Alignment;
                col.HeaderCell.Style.Padding = new Padding(10, 5, 10, 5);
            }

            dgvSuppliers.CellClick += DgvSuppliers_CellClick;

            // Container Panel
            CustomPanel tablePanel = new CustomPanel
            {
                Dock = DockStyle.Fill,
                HasShadow = true,
                ShowBorder = false,
                Padding = new Padding(10),
                BorderRadius = UIConstants.Borders.RadiusMedium,
                BackColor = ThemeManager.Instance.BackgroundDefault
            };
            tablePanel.Controls.Add(dgvSuppliers);
            Controls.Add(tablePanel);
            
            topPanel.SendToBack(); // Ensure top panel is docked top relative to fill

            LoadData();
        }

        public void LoadData()
        {
            try
            {
                _allSuppliers = _controller.GetAllSuppliers();
                dgvSuppliers.DataSource = _allSuppliers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        public void Search(string text)
        {
            if (_allSuppliers == null) return;
            string keyword = text.ToLower();
            var filtered = _allSuppliers.FindAll(s => 
                s.SupplierName.ToLower().Contains(keyword) || 
                (s.Phone != null && s.Phone.Contains(keyword)) ||
                (s.Email != null && s.Email.ToLower().Contains(keyword)));
            dgvSuppliers.DataSource = filtered;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            SupplierForm form = new SupplierForm(null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void DgvSuppliers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Edit
            if (e.ColumnIndex == 5) 
            {
                var supplier = dgvSuppliers.Rows[e.RowIndex].DataBoundItem as Supplier;
                if (supplier != null)
                {
                    SupplierForm form = new SupplierForm(supplier);
                    if (form.ShowDialog() == DialogResult.OK) LoadData();
                }
            }
            // Delete
            else if (e.ColumnIndex == 6)
            {
                var supplier = dgvSuppliers.Rows[e.RowIndex].DataBoundItem as Supplier;
                if (supplier != null)
                {
                    if (MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp '{supplier.SupplierName}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            if (_controller.DeleteSupplier(supplier.SupplierID))
                            {
                                MessageBox.Show("Đã xóa thành công!");
                                LoadData();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xóa: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void OnThemeChanged(object sender, EventArgs e) => ApplyTheme();

        private void ApplyTheme()
        {
            BackColor = ThemeManager.Instance.BackgroundDefault;
            dgvSuppliers.BackgroundColor = ThemeManager.Instance.BackgroundDefault;
            dgvSuppliers.DefaultCellStyle.BackColor = ThemeManager.Instance.BackgroundDefault;
            dgvSuppliers.DefaultCellStyle.ForeColor = ThemeManager.Instance.TextPrimary;
            dgvSuppliers.DefaultCellStyle.SelectionBackColor = UIConstants.PrimaryColor.Light;
            dgvSuppliers.DefaultCellStyle.SelectionForeColor = ThemeManager.Instance.TextPrimary;
            dgvSuppliers.ColumnHeadersDefaultCellStyle.BackColor = UIConstants.PrimaryColor.Default;
            dgvSuppliers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }
    }
}
