using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Services;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Form chính - Giao diện chính ứng dụng với TabControl
    /// </summary>
    public partial class MainForm : Form
    {
        private ProductController _productController;
        private InventoryController _inventoryController;
        private SaveManager _saveManager;
        private TabControl tabControl;
        private DataGridView dgvProducts;
        private DataGridView dgvCategories;
        private DataGridView dgvTransactions;
        private TextBox txtSearch;
        private Button btnAddProduct;
        private Button btnImport, btnExport, btnUndo, btnReport, btnSave;
        private Label lblTotalValue;
        private Label lblChangeStatus;

        public MainForm()
        {
            InitializeComponent();
            Text = "Quản Lý Kho Hàng";
            WindowState = FormWindowState.Maximized;
            _productController = new ProductController();
            _inventoryController = new InventoryController();
            _saveManager = SaveManager.Instance;
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // TabControl
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Location = new Point(0, 60)
            };
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // Tab 1: Sản phẩm
            TabPage tabProducts = new TabPage("Sản Phẩm");
            tabProducts.Controls.Add(CreateProductsTab());
            tabControl.TabPages.Add(tabProducts);

            // Tab 1.5: Danh mục
            TabPage tabCategories = new TabPage("Danh Mục");
            tabCategories.Controls.Add(CreateCategoriesTab());
            tabControl.TabPages.Add(tabCategories);

            // Tab 2: Giao dịch
            TabPage tabTransactions = new TabPage("Giao Dịch");
            tabTransactions.Controls.Add(CreateTransactionsTab());
            tabControl.TabPages.Add(tabTransactions);

            // Tab 3: Báo cáo
            TabPage tabReport = new TabPage("Báo Cáo");
            tabReport.Controls.Add(CreateReportTab());
            tabControl.TabPages.Add(tabReport);

            // Toolbar
            Panel toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.LightGray,
                BorderStyle = BorderStyle.FixedSingle
            };

            btnAddProduct = new Button { Text = "➕ Thêm", Left = 10, Top = 15, Width = 80, Height = 30 };
            btnImport = new Button { Text = "📥 Nhập", Left = 100, Top = 15, Width = 80, Height = 30 };
            btnExport = new Button { Text = "📤 Xuất", Left = 190, Top = 15, Width = 80, Height = 30 };
            btnSave = new Button { Text = "💾 Lưu", Left = 280, Top = 15, Width = 80, Height = 30, BackColor = Color.LightGreen };
            btnUndo = new Button { Text = "↶ Hoàn tác", Left = 370, Top = 15, Width = 90, Height = 30 };
            btnReport = new Button { Text = "📊 Báo cáo", Left = 470, Top = 15, Width = 90, Height = 30 };
            lblChangeStatus = new Label { Text = "", Left = 570, Top = 20, Width = 200, Height = 20, ForeColor = Color.Red, Font = new Font("Arial", 10, FontStyle.Bold) };

            btnAddProduct.Click += BtnAddProduct_Click;
            btnImport.Click += BtnImport_Click;
            btnExport.Click += BtnExport_Click;
            btnSave.Click += BtnSave_Click;
            btnUndo.Click += BtnUndo_Click;
            btnReport.Click += BtnReport_Click;

            toolbar.Controls.Add(btnAddProduct);
            toolbar.Controls.Add(btnImport);
            toolbar.Controls.Add(btnExport);
            toolbar.Controls.Add(btnSave);
            toolbar.Controls.Add(btnUndo);
            toolbar.Controls.Add(btnReport);
            toolbar.Controls.Add(lblChangeStatus);

            Controls.Add(tabControl);
            Controls.Add(toolbar);

            Load += MainForm_Load;
            FormClosing += MainForm_FormClosing;
            ResumeLayout(false);
        }

        private Control CreateProductsTab()
        {
            Panel panel = new Panel { Dock = DockStyle.Fill };

            // Search box
            txtSearch = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 30,
                Margin = new Padding(5),
                Text = ""
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            panel.Controls.Add(txtSearch);

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
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên Sản Phẩm", DataPropertyName = "ProductName", Width = 220 });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Danh Mục", DataPropertyName = "CategoryID", Width = 100 });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Giá", DataPropertyName = "Price", Width = 110, DefaultCellStyle = new DataGridViewCellStyle { Format = "C", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tồn Kho", DataPropertyName = "Quantity", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ngưỡng Min", DataPropertyName = "MinThreshold", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            
            // Add action buttons
            DataGridViewButtonColumn editBtn = new DataGridViewButtonColumn { HeaderText = "✏️", Text = "Sửa", Width = 50, UseColumnTextForButtonValue = true };
            DataGridViewButtonColumn deleteBtn = new DataGridViewButtonColumn { HeaderText = "🗑️", Text = "Xóa", Width = 50, UseColumnTextForButtonValue = true };
            dgvProducts.Columns.Add(editBtn);
            dgvProducts.Columns.Add(deleteBtn);

            dgvProducts.CellFormatting += DgvProducts_CellFormatting;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            panel.Controls.Add(dgvProducts);
            return panel;
        }

        private Control CreateCategoriesTab()
        {
            Panel panel = new Panel { Dock = DockStyle.Fill };

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
            
            // Add action buttons
            DataGridViewButtonColumn catEditBtn = new DataGridViewButtonColumn { HeaderText = "✏️", Text = "Sửa", Width = 50, UseColumnTextForButtonValue = true };
            DataGridViewButtonColumn catDeleteBtn = new DataGridViewButtonColumn { HeaderText = "🗑️", Text = "Xóa", Width = 50, UseColumnTextForButtonValue = true };
            dgvCategories.Columns.Add(catEditBtn);
            dgvCategories.Columns.Add(catDeleteBtn);

            dgvCategories.CellClick += DgvCategories_CellClick;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            panel.Controls.Add(dgvCategories);
            return panel;
        }

        private Control CreateTransactionsTab()
        {
            Panel panel = new Panel { Dock = DockStyle.Fill };

            dgvTransactions = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White
            };

            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID Phiếu", DataPropertyName = "TransactionID", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Loại", DataPropertyName = "Type", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ngày", DataPropertyName = "DateCreated", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ghi chú", DataPropertyName = "Note", Width = 400 });
            
            // Add view button
            DataGridViewButtonColumn viewBtn = new DataGridViewButtonColumn { HeaderText = "👁️", Text = "Xem", Width = 50, UseColumnTextForButtonValue = true };
            dgvTransactions.Columns.Add(viewBtn);

            // Double-click để xem chi tiết
            dgvTransactions.CellDoubleClick += DgvTransactions_CellDoubleClick;
            dgvTransactions.CellClick += DgvTransactions_CellClick;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            panel.Controls.Add(dgvTransactions);
            return panel;
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

        private Control CreateReportTab()
        {
            Panel panel = new Panel { Dock = DockStyle.Fill };

            lblTotalValue = new Label
            {
                Dock = DockStyle.Top,
                Text = "Tổng giá trị tồn kho: 0 VNĐ",
                Height = 40,
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10)
            };

            panel.Controls.Add(lblTotalValue);
            return panel;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền user
            if (GlobalUser.CurrentUser != null && !GlobalUser.CurrentUser.IsAdmin)
            {
                // Staff không thấy nút Báo cáo
                btnReport.Visible = false;
            }

            LoadProducts();
            LoadCategories();
            LoadTransactions();
            UpdateTotalValue();
        }

        private void LoadProducts()
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

        private void LoadCategories()
        {
            try
            {
                List<Category> categories = _productController.GetAllCategories();
                dgvCategories.DataSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message);
            }
        }

        private void LoadTransactions()
        {
            try
            {
                List<StockTransaction> transactions = _inventoryController.GetAllTransactions();
                dgvTransactions.DataSource = transactions;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải giao dịch: " + ex.Message);
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

        private void UpdateTotalValue()
        {
            try
            {
                decimal total = _inventoryController.GetTotalInventoryValue();
                lblTotalValue.Text = $"Tổng giá trị tồn kho: {total:C}";
            }
            catch { }
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0) // Sản Phẩm
            {
                ProductForm form = new ProductForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                    UpdateTotalValue();
                }
            }
            else if (tabControl.SelectedIndex == 1) // Danh Mục
            {
                CategoryForm form = new CategoryForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadCategories();
                    LoadProducts();
                }
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            TransactionForm form = new TransactionForm("Import");
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
                LoadTransactions();
                UpdateTotalValue();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            TransactionForm form = new TransactionForm("Export");
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
                LoadTransactions();
                UpdateTotalValue();
            }
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_inventoryController.UndoLastAction())
                {
                    MessageBox.Show("Hoàn tác thành công!");
                    LoadProducts();
                    LoadTransactions();
                    UpdateTotalValue();
                }
                else
                {
                    MessageBox.Show("Không có hành động để hoàn tác");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hoàn tác: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReport_Click(object sender, EventArgs e)
        {
            ReportForm form = new ReportForm();
            form.ShowDialog();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Row selection và button click handler cho Products
        /// </summary>
        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  // Header row
            
            // Check if button columns were clicked
            if (e.ColumnIndex == 6) // Edit button
            {
                int productId = (int)dgvProducts.Rows[e.RowIndex].Cells[0].Value;
                ProductForm form = new ProductForm(productId);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                    UpdateTotalValue();
                }
                return;
            }
            else if (e.ColumnIndex == 7) // Delete button
            {
                int productId = (int)dgvProducts.Rows[e.RowIndex].Cells[0].Value;
                string productName = dgvProducts.Rows[e.RowIndex].Cells[1].Value.ToString();
                
                try
                {
                    // Kiểm tra phụ thuộc khóa ngoài
                    if (_productController.ProductHasDependencies(productId))
                    {
                        DialogResult result = MessageBox.Show(
                            $"Sản phẩm '{productName}' đang được sử dụng trong các phiếu giao dịch.\n\n" +
                            "Bạn có muốn ẩn sản phẩm này khỏi danh sách không?\n" +
                            "(Dữ liệu sẽ được giữ lại để hỗ trợ undo)",
                            "Xóa sản phẩm",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        
                        if (result == DialogResult.Yes)
                        {
                            _productController.DeleteProduct(productId);
                            MessageBox.Show("Sản phẩm đã được ẩn thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProducts();
                            UpdateTotalValue();
                        }
                    }
                    else
                    {
                        if (MessageBox.Show($"Bạn chắc chắn muốn xóa sản phẩm '{productName}'?", "Xác nhận xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            _productController.DeleteProduct(productId);
                            MessageBox.Show("Sản phẩm đã được xóa thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProducts();
                            UpdateTotalValue();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
            
            // Normal row selection for other columns
            dgvProducts.ClearSelection();
            dgvProducts.Rows[e.RowIndex].Selected = true;
        }

        /// <summary>
        /// Row selection cho Transactions - click any cell để select entire row
        /// </summary>
        private void DgvTransactions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  // Header row
            
            // Check if view button was clicked
            if (e.ColumnIndex == 4) // View button
            {
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
                return;
            }
            
            // Normal row selection for other columns
            dgvTransactions.ClearSelection();
            dgvTransactions.Rows[e.RowIndex].Selected = true;
        }

        /// <summary>
        /// Row selection và button click handler cho Categories
        /// </summary>
        private void DgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;  // Header row
            
            // Check if button columns were clicked
            if (e.ColumnIndex == 2) // Edit button
            {
                int categoryId = (int)dgvCategories.Rows[e.RowIndex].Cells[0].Value;
                CategoryForm form = new CategoryForm(categoryId);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadCategories();
                    LoadProducts();
                }
                return;
            }
            else if (e.ColumnIndex == 3) // Delete button
            {
                int categoryId = (int)dgvCategories.Rows[e.RowIndex].Cells[0].Value;
                string categoryName = dgvCategories.Rows[e.RowIndex].Cells[1].Value.ToString();
                
                try
                {
                    // Kiểm tra danh mục có sản phẩm hay không
                    if (_productController.CategoryHasProducts(categoryId))
                    {
                        DialogResult result = MessageBox.Show(
                            $"Danh mục '{categoryName}' đang có sản phẩm.\n\n" +
                            "Bạn có muốn ẩn danh mục này khỏi danh sách không?\n" +
                            "(Dữ liệu sẽ được giữ lại để hỗ trợ undo)",
                            "Xóa danh mục",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        
                        if (result == DialogResult.Yes)
                        {
                            _productController.DeleteCategory(categoryId);
                            MessageBox.Show("Danh mục đã được ẩn thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCategories();
                            LoadProducts();
                        }
                    }
                    else
                    {
                        if (MessageBox.Show($"Bạn chắc chắn muốn xóa danh mục '{categoryName}'?", "Xác nhận xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            _productController.DeleteCategory(categoryId);
                            MessageBox.Show("Danh mục đã được xóa thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCategories();
                            LoadProducts();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
            
            // Normal row selection for other columns
            dgvCategories.ClearSelection();
            dgvCategories.Rows[e.RowIndex].Selected = true;
        }

        /// <summary>
        /// Nút Save - Lưu tất cả thay đổi vào database
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_saveManager.HasUnsavedChanges)
                {
                    MessageBox.Show("Không có thay đổi nào để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show($"Bạn muốn lưu {_saveManager.ChangeCount} thay đổi vào database?", "Xác nhận lưu", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _saveManager.CommitChanges();
                    UpdateChangeStatus();
                    MessageBox.Show("Đã lưu thay đổi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái thay đổi trên UI
        /// </summary>
        private void UpdateChangeStatus()
        {
            if (_saveManager.HasUnsavedChanges)
            {
                lblChangeStatus.Text = $"⚠️ Chưa lưu: {_saveManager.ChangeCount} thay đổi";
                lblChangeStatus.ForeColor = Color.Red;
                btnSave.Enabled = true;
            }
            else
            {
                lblChangeStatus.Text = "✓ Tất cả thay đổi đã được lưu";
                lblChangeStatus.ForeColor = Color.Green;
                btnSave.Enabled = false;
            }
        }

        /// <summary>
        /// Xử lý sự kiện đóng form
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_saveManager.HasUnsavedChanges)
                {
                    DialogResult result = MessageBox.Show(
                        $"Có {_saveManager.ChangeCount} thay đổi chưa được lưu.\n\nBạn muốn lưu trước khi thoát?",
                        "Xác nhận thoát",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (result == DialogResult.Yes)
                    {
                        // Lưu thay đổi
                        _saveManager.CommitChanges();
                        MessageBox.Show("Đã lưu thay đổi.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (result == DialogResult.No)
                    {
                        // Khôi phục về lần save cuối
                        _saveManager.RollbackChanges();
                        MessageBox.Show("Đã hủy bỏ tất cả thay đổi từ lần lưu cuối.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Xóa toàn bộ undo stack
                _saveManager.ClearUndoStack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thoát: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

