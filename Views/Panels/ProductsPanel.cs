using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Views.Forms;

namespace WarehouseManagement.Views.Panels
{
    public class ProductsPanel : Panel
    {
        private DataGridView dgvProducts;
        private TextBox txtSearch;
        private ProductController _productController;

        public ProductsPanel()
        {
            _productController = new ProductController();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;

            // Search box
            txtSearch = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 30,
                Margin = new Padding(5),
                Text = ""
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            Controls.Add(txtSearch);

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

            dgvProducts.CellFormatting += DgvProducts_CellFormatting;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Controls.Add(dgvProducts);
        }

        public void LoadData()
        {
            try
            {
                List<Product> products = _productController.GetAllProducts();
                dgvProducts.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            try
            {
                List<Product> allProducts = _productController.GetAllProducts();
                List<Product> filtered = allProducts.FindAll(p => p.ProductName.ToLower().Contains(searchText));
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

            int productId = (int)dgvProducts.Rows[e.RowIndex].Cells[0].Value;
            ProductForm form = new ProductForm(productId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
    }
}
