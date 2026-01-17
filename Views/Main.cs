using System;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Services;
using WarehouseManagement.Views.Panels;
using WarehouseManagement.Views.Forms;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Main - Khung giao diện chính
    /// 
    /// TRÁCH NHIỆM:
    /// - Quản lý cấu trúc: Toolbar, TabControl, các tab
    /// - Điều phối các tab panel
    /// - Xử lý các nút chính: Thêm, Nhập, Xuất, Báo cáo
    /// - Xử lý sự kiện form: Load, Closing
    /// 
    /// CHUYÊN MÔN:
    /// - UI chi tiết từng tab → Tạo bởi ProductsPanel, CategoriesPanel, TransactionsPanel
    /// - Event handler của từng tab → Xử lý bởi các panel class
    /// - Format hiển thị → Xử lý bởi các panel class
    /// - Xử lý Save/Undo → Xử lý bởi Actions class
    /// </summary>
    public partial class Main : Form
    {
        private ProductController _productController;
        private CategoryController _categoryController;
        private InventoryController _inventoryController;
        private ActionsController _logController;
        private ActionsService _actionsService;
        private Actions _actions;

        private TabControl tabControl;
        private ProductsPanel productsPanel;
        private CategoriesPanel categoriesPanel;
        private TransactionsPanel transactionsPanel;
        private System.Windows.Forms.Timer statusUpdateTimer;

        private Button btnAddProduct, btnImport, btnExport, btnUndo, btnSave, btnReport;
        private Label lblChangeStatus;

        public Main()
        {
            // Initialize controllers FIRST, before UI
            _productController = new ProductController();
            _categoryController = new CategoryController();
            _inventoryController = new InventoryController();
            _logController = new ActionsController();
            _actionsService = ActionsService.Instance;
            
            // Then initialize UI components
            InitializeComponent();
            Text = "Quản Lý Kho Hàng";
            WindowState = FormWindowState.Maximized;
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

            // Tab 1: Sản Phẩm
            productsPanel = new ProductsPanel();
            TabPage tabProducts = new TabPage("Sản Phẩm");
            tabProducts.Controls.Add(productsPanel);
            tabControl.TabPages.Add(tabProducts);

            // Tab 2: Danh Mục
            categoriesPanel = new CategoriesPanel();
            TabPage tabCategories = new TabPage("Danh Mục");
            tabCategories.Controls.Add(categoriesPanel);
            tabControl.TabPages.Add(tabCategories);

            // Tab 3: Giao Dịch
            transactionsPanel = new TransactionsPanel();
            TabPage tabTransactions = new TabPage("Giao Dịch");
            tabTransactions.Controls.Add(transactionsPanel);
            tabControl.TabPages.Add(tabTransactions);

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

            // Khởi tạo Actions handler
            _actions = new Actions(_actionsService, _inventoryController, lblChangeStatus, btnSave, RefreshAllData);

            // Khởi tạo timer để cập nhật trạng thái thay đổi định kỳ (500ms)
            statusUpdateTimer = new System.Windows.Forms.Timer();
            statusUpdateTimer.Interval = 500;
            statusUpdateTimer.Tick += (s, e) => _actions?.UpdateChangeStatus();

            Load += Main_Load;
            FormClosing += Main_FormClosing;
            ResumeLayout(false);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_actions != null)
            {
                _actions.Save();
            }
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            if (_actions != null)
            {
                _actions.Undo();
            }
        }

        private void RefreshAllData()
        {
            productsPanel.LoadData();
            categoriesPanel.LoadData();
            transactionsPanel.LoadData();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (GlobalUser.CurrentUser != null && !GlobalUser.CurrentUser.IsAdmin)
            {
                // Staff restrictions here if needed
            }

            // Start the status update timer
            statusUpdateTimer?.Start();

            productsPanel.LoadData();
            categoriesPanel.LoadData();
            transactionsPanel.LoadData();
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0) // Sản Phẩm
            {
                ProductForm form = new ProductForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    productsPanel.LoadData();
                    _actions?.UpdateChangeStatus();
                }
            }
            else if (tabControl.SelectedIndex == 1) // Danh Mục
            {
                CategoryForm form = new CategoryForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    categoriesPanel.LoadData();
                    productsPanel.LoadData();
                    _actions?.UpdateChangeStatus();
                }
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            TransactionAllForm form = new TransactionAllForm("Import");
            if (form.ShowDialog() == DialogResult.OK)
            {
                productsPanel.LoadData();
                transactionsPanel.LoadData();
                _actions?.UpdateChangeStatus();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            TransactionAllForm form = new TransactionAllForm("Export");
            if (form.ShowDialog() == DialogResult.OK)
            {
                productsPanel.LoadData();
                transactionsPanel.LoadData();
                _actions?.UpdateChangeStatus();
            }
        }

        private void BtnReport_Click(object sender, EventArgs e)
        {
            TransactionReportForm form = new TransactionReportForm();
            form.ShowDialog();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Stop the status update timer
                statusUpdateTimer?.Stop();
                statusUpdateTimer?.Dispose();

                if (_actionsService.HasUnsavedChanges)
                {
                    DialogResult result = MessageBox.Show(
                        $"Có {_actionsService.ChangeCount} thay đổi chưa được lưu.\n\nBạn muốn lưu trước khi thoát?",
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
                        _actionsService.CommitChanges();
                        MessageBox.Show("Đã lưu thay đổi.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (result == DialogResult.No)
                    {
                        _actionsService.RollbackChanges();
                        MessageBox.Show("Đã hủy bỏ tất cả thay đổi từ lần lưu cuối.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                try
                {
                    if (_logController != null)
                    {
                        _logController.ClearAllLogs();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi xóa logs: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thoát: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}