using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Views.Forms;

namespace WarehouseManagement.Views.Panels
{
    public class CategoriesPanel : Panel
    {
        private DataGridView dgvCategories;
        private CategoryController _categoryController;

        public CategoriesPanel()
        {
            _categoryController = new CategoryController();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;

            // DataGridView
            dgvCategories = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White
            };

            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "CategoryID", Width = 50 });
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên Danh Mục", DataPropertyName = "CategoryName", Width = 400 });

            dgvCategories.CellClick += DgvCategories_CellClick;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Controls.Add(dgvCategories);
        }

        public void LoadData()
        {
            try
            {
                List<Category> categories = _categoryController.GetAllCategories();
                dgvCategories.DataSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message);
            }
        }

        private void DgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int categoryId = (int)dgvCategories.Rows[e.RowIndex].Cells[0].Value;
            CategoryForm form = new CategoryForm(categoryId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
            dgvCategories.Rows[e.RowIndex].Selected = true;
        }
    }
}