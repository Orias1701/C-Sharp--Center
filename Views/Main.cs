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
    /// Main - Khung giao diá»‡n chÃ­nh
    /// 
    /// TRÃCH NHIá»†M:
    /// - Quáº£n lÃ½ cáº¥u trÃºc: Toolbar, TabControl, cÃ¡c tab
    /// - Äiá»u phá»‘i cÃ¡c tab panel
    /// - Xá»­ lÃ½ cÃ¡c nÃºt chÃ­nh: ThÃªm, Nháº­p, Xuáº¥t, BÃ¡o cÃ¡o
    /// - Xá»­ lÃ½ sá»± kiá»‡n form: Load, Closing
    /// 
    /// CHá»ˆ NÃ“ICHUYÃŠN MÃ”N:
    /// - UI chi tiáº¿t tá»«ng tab â†’ Táº¡o bá»Ÿi ProductsPanel, CategoriesPanel, TransactionsPanel
    /// - Event handler cá»§a tá»«ng tab â†’ Xá»­ lÃ½ bá»Ÿi cÃ¡c panel class
    /// - Format hiá»ƒn thá»‹ â†’ Xá»­ lÃ½ bá»Ÿi cÃ¡c panel class
    /// - Xá»­ lÃ½ Save/Undo â†’ Xá»­ lÃ½ bá»Ÿi Actions class
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
            Text = "Quáº£n LÃ½ Kho HÃ ng";
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

            // Tab 1: Sáº£n Pháº©m
            productsPanel = new ProductsPanel();
            TabPage tabProducts = new TabPage("Sáº£n Pháº©m");
            tabProducts.Controls.Add(productsPanel);
            tabControl.TabPages.Add(tabProducts);

            // Tab 2: Danh Má»¥c
            categoriesPanel = new CategoriesPanel();
            TabPage tabCategories = new TabPage("Danh Má»¥c");
            tabCategories.Controls.Add(categoriesPanel);
            tabControl.TabPages.Add(tabCategories);

            // Tab 3: Giao Dá»‹ch
            transactionsPanel = new TransactionsPanel();
            TabPage tabTransactions = new TabPage("Giao Dá»‹ch");
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

            btnAddProduct = new Button { Text = "âž• ThÃªm", Left = 10, Top = 15, Width = 80, Height = 30 };
            btnImport = new Button { Text = "ðŸ“¥ Nháº­p", Left = 100, Top = 15, Width = 80, Height = 30 };
            btnExport = new Button { Text = "ðŸ“¤ Xuáº¥t", Left = 190, Top = 15, Width = 80, Height = 30 };
            btnSave = new Button { Text = "ðŸ’¾ LÆ°u", Left = 280, Top = 15, Width = 80, Height = 30, BackColor = Color.LightGreen };
            btnUndo = new Button { Text = "â†¶ HoÃ n tÃ¡c", Left = 370, Top = 15, Width = 90, Height = 30 };
            btnReport = new Button { Text = "ðŸ“Š BÃ¡o cÃ¡o", Left = 470, Top = 15, Width = 90, Height = 30 };
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
            if (tabControl.SelectedIndex == 0) // Sáº£n Pháº©m
            {
                ProductForm form = new ProductForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    productsPanel.LoadData();
                    _actions?.UpdateChangeStatus();
                }
            }
            else if (tabControl.SelectedIndex == 1) // Danh Má»¥c
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
                        $"CÃ³ {_actionsService.ChangeCount} thay Ä‘á»•i chÆ°a Ä‘Æ°á»£c lÆ°u.\n\nBáº¡n muá»‘n lÆ°u trÆ°á»›c khi thoÃ¡t?",
                        "XÃ¡c nháº­n thoÃ¡t",
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
                        MessageBox.Show("ÄÃ£ lÆ°u thay Ä‘á»•i.", "ThÃ nh cÃ´ng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (result == DialogResult.No)
                    {
                        _actionsService.RollbackChanges();
                        MessageBox.Show("ÄÃ£ há»§y bá» táº¥t cáº£ thay Ä‘á»•i tá»« láº§n lÆ°u cuá»‘i.", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    System.Diagnostics.Debug.WriteLine($"Lá»—i xÃ³a logs: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i khi thoÃ¡t: {ex.Message}", "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}





