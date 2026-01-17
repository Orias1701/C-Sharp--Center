using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Views.Forms;

namespace WarehouseManagement.Views.Panels
{
    public class ProductsPanel : Panel, ISearchable
    {
        private DataGridView dgvProducts;
        private ProductController _productController;
        private List<Product> allProducts;

        public ProductsPanel()
        {
            _productController = new ProductController();
            InitializeComponent();
            SettingsForm.SettingsChanged += (s, e) => LoadData();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;

            // DataGridView
            dgvProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White
            };

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "ProductID", Width = 50 });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên Sản Phẩm", DataPropertyName = "ProductName", Width = 180 });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Danh Mục", DataPropertyName = "CategoryID", Width = 80 });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Giá", DataPropertyName = "Price", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Format = "C", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tồn Kho", DataPropertyName = "Quantity", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ngưỡng Min", DataPropertyName = "MinThreshold", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tổng Giá Trị", DataPropertyName = "InventoryValue", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "C", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvProducts.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Ẩn", Width = 50, UseColumnTextForButtonValue = true, Text = "👁️" });
            dgvProducts.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Xóa", Width = 50, UseColumnTextForButtonValue = true, Text = "🗑️" });

            dgvProducts.CellFormatting += DgvProducts_CellFormatting;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.VisibleChanged += (s, e) =>
            {
                if (this.Visible)
                    LoadData();
            };
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Controls.Add(dgvProducts);
        }

        public void LoadData()
        {
            try
            {
                allProducts = _productController.GetAllProducts(SettingsForm.ShowHiddenItems);
                dgvProducts.DataSource = allProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        public void Search(string searchText)
        {
            try
            {
                string search = searchText.ToLower();
                List<Product> filtered = allProducts.FindAll(p => p.ProductName.ToLower().Contains(search));
                dgvProducts.DataSource = filtered;
            }
            catch { }
        }

        private void DgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProducts.Rows[e.RowIndex].DataBoundItem is Product product)
            {
                if (product.IsLowStock)
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                    e.CellStyle.ForeColor = Color.DarkRed;
                }
                else
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Column 7 is the hide button
            if (e.ColumnIndex == 7)
            {
                int productId = (int)dgvProducts.Rows[e.RowIndex].Cells[0].Value;
                string productName = dgvProducts.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                
                DialogResult result = MessageBox.Show(
                    $"Bạn chắc chắn muốn đảo trạng thái sản phẩm '{productName}'?",
                    "Xác nhận đảo trạng thái",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _productController.HideProduct(productId);
                        MessageBox.Show("Trạng thái sản phẩm đã được thay đổi.");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ẩn sản phẩm: " + ex.Message);
                    }
                }
                return;
            }

            // Column 8 is the delete button
            if (e.ColumnIndex == 8)
            {
                int productId = (int)dgvProducts.Rows[e.RowIndex].Cells[0].Value;
                string productName = dgvProducts.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                
                DialogResult result = MessageBox.Show(
                    $"Bạn chắc chắn muốn xóa sản phẩm '{productName}'?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _productController.DeleteProduct(productId);
                        MessageBox.Show("Sản phẩm đã được xóa thành công.");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa sản phẩm: " + ex.Message);
                    }
                }
                return;
            }

            // Other columns open the edit form
            int id = (int)dgvProducts.Rows[e.RowIndex].Cells[0].Value;
            ProductForm form = new ProductForm(id);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
    }
}