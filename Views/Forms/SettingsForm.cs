using System;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;

namespace WarehouseManagement.Views.Forms
{
    /// <summary>
    /// Form Cài Đặt - Quản lý tùy chọn hiển thị và theme
    /// </summary>
    public class SettingsForm : Form
    {
        private CheckBox chkShowHidden;
        private CheckBox chkDarkMode;
        private Button btnViewComponents;
        private Button btnSave, btnCancel;

        // Static property to share settings across the app
        public static bool ShowHiddenItems { get; set; } = false;
        
        // Event to notify when settings change
        public static event EventHandler SettingsChanged;

        public SettingsForm()
        {
            InitializeComponent();
            Text = "Cài Đặt";
            
            // Subscribe to theme changes
            ThemeManager.Instance.ThemeChanged += OnThemeChanged;
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            
            // Main container
            CustomPanel mainPanel = new CustomPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = UIConstants.Borders.RadiusLarge,
                ShowBorder = false,
                Padding = new Padding(0)
            };

            const int LEFT_MARGIN = 40;
            const int INPUT_WIDTH = 320;
            int currentY = 30;

            // === DISPLAY SETTINGS ===
            Label lblDisplaySettings = new Label
            {
                Text = "HIỂN THỊ",
                Left = LEFT_MARGIN,
                Top = currentY,
                Width = INPUT_WIDTH,
                Height = 25,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = ThemeManager.Instance.TextPrimary
            };
            mainPanel.Controls.Add(lblDisplaySettings);
            currentY += 30;

            // CheckBox Show Hidden
            chkShowHidden = new CheckBox
            {
                Text = "Hiển thị tất cả các bản ghi đã ẩn",
                Left = LEFT_MARGIN + 10,
                Top = currentY,
                Width = INPUT_WIDTH,
                Height = 25,
                Checked = ShowHiddenItems,
                Font = ThemeManager.Instance.FontRegular,
                ForeColor = ThemeManager.Instance.TextPrimary
            };
            mainPanel.Controls.Add(chkShowHidden);
            currentY += 40;

            // === THEME SETTINGS ===
            Label lblThemeSettings = new Label
            {
                Text = "GIAO DIỆN",
                Left = LEFT_MARGIN,
                Top = currentY,
                Width = INPUT_WIDTH,
                Height = 25,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = ThemeManager.Instance.TextPrimary
            };
            mainPanel.Controls.Add(lblThemeSettings);
            currentY += 30;

            // CheckBox Dark Mode
            chkDarkMode = new CheckBox
            {
                Text = $"{UIConstants.Icons.Moon} Chế độ tối (Dark Mode)",
                Left = LEFT_MARGIN + 10,
                Top = currentY,
                Width = INPUT_WIDTH,
                Height = 25,
                Checked = ThemeManager.Instance.IsDarkMode,
                Font = ThemeManager.Instance.FontRegular,
                ForeColor = ThemeManager.Instance.TextPrimary
            };
            chkDarkMode.CheckedChanged += ChkDarkMode_CheckedChanged;
            mainPanel.Controls.Add(chkDarkMode);
            currentY += 40;

            // Button View Components
            btnViewComponents = new Button
            {
                Text = $"{UIConstants.Icons.Eye} Xem Components",
                Left = LEFT_MARGIN,
                Top = currentY,
                Width = 200,
                Height = 35,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            btnViewComponents.Click += BtnViewComponents_Click;
            mainPanel.Controls.Add(btnViewComponents);
            currentY += 60;

            // === ACTION BUTTONS - Centered ===
            int totalBtnW = 100 + 10 + 100;
            int startX = LEFT_MARGIN + (INPUT_WIDTH - totalBtnW) / 2;

            btnSave = new Button
            {
                Text = "💾 Lưu",
                Left = startX,
                Top = currentY,
                Width = 100,
                Height = 35
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "❌ Hủy",
                Left = startX + 110,
                Top = currentY,
                Width = 100,
                Height = 35,
                CausesValidation = false
            };
            btnCancel.Click += BtnCancel_Click;

            mainPanel.Controls.Add(btnSave);
            mainPanel.Controls.Add(btnCancel);

            Controls.Add(mainPanel);

            Width = 420;
            Height = currentY + 80;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            ResumeLayout(false);
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            BackColor = ThemeManager.Instance.BackgroundDefault;
            ForeColor = ThemeManager.Instance.TextPrimary;
            
            // Update icon in dark mode checkbox
            chkDarkMode.Text = ThemeManager.Instance.IsDarkMode 
                ? $"{UIConstants.Icons.Sun} Chế độ sáng (Light Mode)" 
                : $"{UIConstants.Icons.Moon} Chế độ tối (Dark Mode)";
        }

        private void ChkDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            ThemeManager.Instance.IsDarkMode = chkDarkMode.Checked;
        }

        private void BtnViewComponents_Click(object sender, EventArgs e)
        {
            // Tạo form mới để hiển thị ComponentsTestPanel
            Form componentsForm = new Form
            {
                Text = "Components Preview",
                Width = 1400,
                Height = 800,
                StartPosition = FormStartPosition.CenterParent,
                WindowState = FormWindowState.Maximized
            };

            ComponentsTestPanel testPanel = new ComponentsTestPanel();
            componentsForm.Controls.Add(testPanel);

            // Apply theme to form
            ThemeManager.Instance.ApplyThemeToForm(componentsForm);

            componentsForm.ShowDialog();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Update static property
            ShowHiddenItems = chkShowHidden.Checked;

            // Notify all listeners
            SettingsChanged?.Invoke(this, EventArgs.Empty);

            MessageBox.Show("Cài đặt đã được lưu.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // Revert dark mode if changed
            chkDarkMode.Checked = ThemeManager.Instance.IsDarkMode;
            
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeManager.Instance.ThemeChanged -= OnThemeChanged;
            }
            base.Dispose(disposing);
        }
    }
}
