using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Views.Forms;

namespace WarehouseManagement.Views.Panels
{
    public class CategoriesPanel : Panel, ISearchable
    {
        private DataGridView dgvCategories;
        private CategoryController _categoryController;
        private List<Category> allCategories;

        public CategoriesPanel()
        {
            _categoryController = new CategoryController();
            InitializeComponent();
            SettingsForm.SettingsChanged += (s, e) => LoadData();
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
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên Danh Mục", DataPropertyName = "CategoryName", Width = 200 });
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mô Tả", DataPropertyName = "Description", Width = 300 });
            dgvCategories.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Ẩn", Width = 50, UseColumnTextForButtonValue = true, Text = "👁️" });
            dgvCategories.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Xóa", Width = 50, UseColumnTextForButtonValue = true, Text = "🗑️" });

            dgvCategories.CellClick += DgvCategories_CellClick;
            dgvCategories.VisibleChanged += (s, e) =>
            {
                if (this.Visible)
                    LoadData();
            };
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Controls.Add(dgvCategories);
        }

        public void LoadData()
        {
            try
            {
                // Load all categories including hidden ones if setting is enabled
                allCategories = _categoryController.GetAllCategories(SettingsForm.ShowHiddenItems);
                dgvCategories.DataSource = allCategories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message);
            }
        }

        public void Search(string searchText)
        {
            try
            {
                string search = searchText.ToLower();
                List<Category> filtered = allCategories.FindAll(c => c.CategoryName.ToLower().Contains(search));
                dgvCategories.DataSource = filtered;
            }
            catch { }
        }

        private void DgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int categoryId = (int)dgvCategories.Rows[e.RowIndex].Cells[0].Value;
            string categoryName = dgvCategories.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";

            // Column 3 is the hide button
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show(
                    $"Bạn chắc chắn muốn đảo trạng thái danh mục '{categoryName}'?",
                    "Xác nhận đảo trạng thái",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _categoryController.HideCategory(categoryId);
                        MessageBox.Show("Trạng thái danh mục đã được thay đổi.");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ẩn danh mục: " + ex.Message);
                    }
                }
                return;
            }

            // Column 4 is the delete button
            if (e.ColumnIndex == 4)
            {
                DialogResult result = MessageBox.Show(
                    $"Bạn chắc chắn muốn xóa danh mục '{categoryName}'?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _categoryController.DeleteCategory(categoryId);
                        MessageBox.Show("Danh mục đã được xóa thành công.");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa danh mục: " + ex.Message);
                    }
                }
                return;
            }

            // Other columns open the edit form
            CategoryForm form = new CategoryForm(categoryId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
            dgvCategories.Rows[e.RowIndex].Selected = true;
        }
    }
}