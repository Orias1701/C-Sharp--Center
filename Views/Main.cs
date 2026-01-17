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
    /// Layout: Toolbar (top) | Menu (left) | Content (center) | Footer (bottom)
    /// </summary>
    public partial class Main : Form
    {
        private ProductController _productController;
        private CategoryController _categoryController;
        private InventoryController _inventoryController;
        private ActionsController _logController;
        private ActionsService _actionsService;
        private Actions _actions;

        // UI Components
        private Panel toolbarPanel, menuPanel, contentPanel, footerPanel;
        private ProductsPanel productsPanel;
        private CategoriesPanel categoriesPanel;
        private TransactionsPanel transactionsPanel;
        
        private TextBox txtSearch;
        private Button btnSearch, btnAddRecord, btnImport, btnExport, btnUndo, btnSave, btnReport;
        private Button btnCategories, btnProducts, btnTransactions, btnSettings, btnAccount;
        private Label lblChangeStatus, lblFooterTime;
        
        private System.Windows.Forms.Timer statusUpdateTimer, timeUpdateTimer;

        public Main()
        {
            _productController = new ProductController();
            _categoryController = new CategoryController();
            _inventoryController = new InventoryController();
            _logController = new ActionsController();
            _actionsService = ActionsService.Instance;
            
            InitializeComponent();
            Text = "Quản Lý Kho Hàng";
            WindowState = FormWindowState.Maximized;
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // 1. TOOLBAR (Top)
            toolbarPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle
            };
            CreateToolbar(toolbarPanel);

            // 2. FOOTER (Bottom)
            footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle
            };
            lblFooterTime = new Label
            {
                Dock = DockStyle.Right,
                Width = 200,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 10, 0),
                Font = new Font("Arial", 10, FontStyle.Regular)
            };
            footerPanel.Controls.Add(lblFooterTime);

            // 3. MENU (Left) - 200px width
            menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };
            CreateMenu(menuPanel);

            // 4. CONTENT (Center) - Panel Stack (không dùng TabControl)
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            
            // Create panels
            CreatePanels();

            contentPanel.Controls.Add(transactionsPanel);
            contentPanel.Controls.Add(productsPanel);
            contentPanel.Controls.Add(categoriesPanel);

            // 5. Add all to form in correct order
            Controls.Add(contentPanel);
            Controls.Add(menuPanel);
            Controls.Add(footerPanel);
            Controls.Add(toolbarPanel);

            // Initialize handlers
            _actions = new Actions(_actionsService, _inventoryController, lblChangeStatus, btnSave, RefreshAllData);

            // Timers
            statusUpdateTimer = new System.Windows.Forms.Timer();
            statusUpdateTimer.Interval = 500;
            statusUpdateTimer.Tick += (s, e) => _actions?.UpdateChangeStatus();

            timeUpdateTimer = new System.Windows.Forms.Timer();
            timeUpdateTimer.Interval = 1000;
            timeUpdateTimer.Tick += (s, e) => lblFooterTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            Load += Main_Load;
            FormClosing += Main_FormClosing;
            ResumeLayout(false);
        }

        private void CreateToolbar(Panel toolbar)
        {
            // Search section (Left)
            txtSearch = new TextBox
            {
                Text = "Tìm kiếm...",
                Left = 10,
                Top = 15,
                Width = 200,
                Height = 30,
                ForeColor = Color.Gray
            };
            txtSearch.GotFocus += (s, e) =>
            {
                if (txtSearch.Text == "Tìm kiếm...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };
            txtSearch.LostFocus += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    txtSearch.Text = "Tìm kiếm...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            btnSearch = new Button
            {
                Text = "🔍",
                Left = 215,
                Top = 15,
                Width = 40,
                Height = 30
            };
            btnSearch.Click += (s, e) => TxtSearch_TextChanged(null, null);

            // Middle section buttons
            btnAddRecord = new Button { Text = "➕ Thêm", Left = 270, Top = 15, Width = 90, Height = 30 };
            btnImport = new Button { Text = "📥 Nhập", Left = 365, Top = 15, Width = 80, Height = 30 };
            btnExport = new Button { Text = "📤 Xuất", Left = 450, Top = 15, Width = 80, Height = 30 };
            btnUndo = new Button { Text = "↶ Hoàn tác", Left = 535, Top = 15, Width = 90, Height = 30 };
            btnSave = new Button { Text = "💾 Lưu", Left = 630, Top = 15, Width = 80, Height = 30, BackColor = Color.LightGreen };
            btnReport = new Button { Text = "📊 Báo cáo", Left = 715, Top = 15, Width = 90, Height = 30 };

            // Status label (Right)
            lblChangeStatus = new Label
            {
                Text = "",
                Left = 810,
                Top = 20,
                Width = 150,
                Height = 20,
                ForeColor = Color.Red,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            // Event handlers
            btnAddRecord.Click += BtnAddRecord_Click;
            btnImport.Click += BtnImport_Click;
            btnExport.Click += BtnExport_Click;
            btnUndo.Click += BtnUndo_Click;
            btnSave.Click += BtnSave_Click;
            btnReport.Click += BtnReport_Click;

            toolbar.Controls.AddRange(new Control[] { 
                txtSearch, btnSearch, btnAddRecord, btnImport, btnExport, 
                btnUndo, btnSave, btnReport, lblChangeStatus 
            });
        }

        private void CreateMenu(Panel menu)
        {
            // Navigation buttons (Catalog)
            btnCategories = new Button
            {
                Text = "📁 Danh Mục",
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(100, 150, 200),
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold)
            };
            btnCategories.Click += (s, e) => ShowPanel(0);

            btnProducts = new Button
            {
                Text = "📦 Sản Phẩm",
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(100, 150, 200),
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold)
            };
            btnProducts.Click += (s, e) => ShowPanel(1);

            btnTransactions = new Button
            {
                Text = "📊 Giao Dịch",
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(100, 150, 200),
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold)
            };
            btnTransactions.Click += (s, e) => ShowPanel(2);

            // Account button (Bottom)
            btnSettings = new Button
            {
                Text = "⚙️ Cài Đặt",
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(150, 150, 150),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnSettings.Click += BtnSettings_Click;

            btnAccount = new Button
            {
                Text = "👤 " + (GlobalUser.CurrentUser?.FullName ?? "Account"),
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(200, 100, 100),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnAccount.Click += BtnAccount_Click;

            menu.Controls.Add(btnAccount);
            menu.Controls.Add(btnSettings);
            menu.Controls.Add(btnTransactions);
            menu.Controls.Add(btnProducts);
            menu.Controls.Add(btnCategories);
        }

        private void CreatePanels()
        {
            // Panel 0: Categories
            categoriesPanel = new CategoriesPanel();
            categoriesPanel.Dock = DockStyle.Fill;
            categoriesPanel.Visible = true;

            // Panel 1: Products
            productsPanel = new ProductsPanel();
            productsPanel.Dock = DockStyle.Fill;
            productsPanel.Visible = false;

            // Panel 2: Transactions
            transactionsPanel = new TransactionsPanel();
            transactionsPanel.Dock = DockStyle.Fill;
            transactionsPanel.Visible = false;
        }

        private void ShowPanel(int index)
        {
            ResetSearch();
            
            // Hide all panels
            categoriesPanel.Visible = false;
            productsPanel.Visible = false;
            transactionsPanel.Visible = false;

            // Show selected panel
            switch (index)
            {
                case 0:
                    categoriesPanel.Visible = true;
                    categoriesPanel.BringToFront();
                    break;
                case 1:
                    productsPanel.Visible = true;
                    productsPanel.BringToFront();
                    break;
                case 2:
                    transactionsPanel.Visible = true;
                    transactionsPanel.BringToFront();
                    break;
            }
        }

        private void BtnAddRecord_Click(object sender, EventArgs e)
        {
            // Determine which form to open based on currently visible panel
            if (categoriesPanel.Visible)
            {
                CategoryForm form = new CategoryForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    categoriesPanel.LoadData();
                    productsPanel.LoadData();
                    _actions?.UpdateChangeStatus();
                }
            }
            else if (productsPanel.Visible)
            {
                ProductForm form = new ProductForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    productsPanel.LoadData();
                    _actions?.UpdateChangeStatus();
                }
            }
            else if (transactionsPanel.Visible)
            {
                ProductForm form = new ProductForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    productsPanel.LoadData();
                    transactionsPanel.LoadData();
                    _actions?.UpdateChangeStatus();
                }
            }
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }

        private void BtnAccount_Click(object sender, EventArgs e)
        {
            // Show popup with Switch Account and Quit options
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("🔄 Chuyển Account", null, (s, e) =>
            {
                this.Hide();
                Login loginForm = new Login();
                loginForm.ShowDialog();
                if (GlobalUser.CurrentUser != null)
                {
                    btnAccount.Text = "👤 " + GlobalUser.CurrentUser.FullName;
                    this.Show();
                }
                else
                {
                    Application.Exit();
                }
            });
            menu.Items.Add("❌ Thoát", null, (s, e) => Application.Exit());
            menu.Show(btnAccount, 0, btnAccount.Height);
        }

        private void ResetSearch()
        {
            txtSearch.Text = "Tìm kiếm...";
            txtSearch.ForeColor = Color.Gray;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Tìm kiếm...") return;

            // Determine which panel is visible and search
            Control panel = null;
            
            if (categoriesPanel.Visible) panel = categoriesPanel;
            else if (productsPanel.Visible) panel = productsPanel;
            else if (transactionsPanel.Visible) panel = transactionsPanel;

            if (panel is ISearchable searchable)
            {
                searchable.Search(txtSearch.Text);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            TransactionAllForm form = new TransactionAllForm("Import");
            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshAllData();
                _actions?.UpdateChangeStatus();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            TransactionAllForm form = new TransactionAllForm("Export");
            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshAllData();
                _actions?.UpdateChangeStatus();
            }
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            if (_actions != null)
            {
                _actions.Undo();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_actions != null)
            {
                _actions.Save();
            }
        }

        private void BtnReport_Click(object sender, EventArgs e)
        {
            TransactionReportForm form = new TransactionReportForm();
            form.ShowDialog();
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

            statusUpdateTimer?.Start();
            timeUpdateTimer?.Start();

            productsPanel.LoadData();
            categoriesPanel.LoadData();
            transactionsPanel.LoadData();

            lblFooterTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                statusUpdateTimer?.Stop();
                statusUpdateTimer?.Dispose();
                timeUpdateTimer?.Stop();
                timeUpdateTimer?.Dispose();

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