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
        private Button btnExit;

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
            chkShowHidden.CheckedChanged += ChkShowHidden_CheckedChanged;
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

            // === NÚT THOÁT ===
            btnExit = new Button
            {
                Text = "Thoát",
                Left = LEFT_MARGIN + (INPUT_WIDTH - 100) / 2,
                Top = currentY,
                Width = 100,
                Height = 35,
                CausesValidation = false
            };
            btnExit.Click += BtnExit_Click;

            mainPanel.Controls.Add(btnExit);

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
            ThemeManager.Instance.ApplyThemeToForm(this);
            if (chkDarkMode != null)
            {
                chkDarkMode.Text = ThemeManager.Instance.IsDarkMode
                    ? $"{UIConstants.Icons.Sun} Chế độ sáng (Light Mode)"
                    : $"{UIConstants.Icons.Moon} Chế độ tối (Dark Mode)";
                chkDarkMode.ForeColor = ThemeManager.Instance.TextPrimary;
            }
            if (chkShowHidden != null)
                chkShowHidden.ForeColor = ThemeManager.Instance.TextPrimary;
            if (btnViewComponents != null)
            {
                btnViewComponents.BackColor = ThemeManager.Instance.BackgroundDefault;
                btnViewComponents.ForeColor = ThemeManager.Instance.TextPrimary;
            }
            if (btnExit != null)
            {
                btnExit.BackColor = ThemeManager.Instance.BackgroundDefault;
                btnExit.ForeColor = ThemeManager.Instance.TextPrimary;
            }
        }

        private void ChkDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            ThemeManager.Instance.IsDarkMode = chkDarkMode.Checked;
        }

        private void ChkShowHidden_CheckedChanged(object sender, EventArgs e)
        {
            ShowHiddenItems = chkShowHidden.Checked;
            SettingsChanged?.Invoke(this, EventArgs.Empty);
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
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
