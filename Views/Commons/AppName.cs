using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;

namespace WarehouseManagement.Views.Commons
{
    /// <summary>
    /// AppName component - Hiển thị tên ứng dụng
    /// </summary>
    public class AppName : CustomPanel
    {
        private Label lblAppName;

        public AppName()
        {
            InitializeComponent();
            ApplyPrimaryColor();
            
            // Subscribe to theme changes to reapply primary color
            ThemeManager.Instance.ThemeChanged += (s, e) => ApplyPrimaryColor();
        }
        
        private void ApplyPrimaryColor()
        {
            if (lblAppName != null)
            {
                lblAppName.ForeColor = UIConstants.PrimaryColor.Default;
            }
        }
        
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            // Reapply primary color after controls are added
            ApplyPrimaryColor();
        }

        private void InitializeComponent()
        {
            // Panel configuration
            BackColor = ThemeManager.Instance.BackgroundLight;
            ShowBorder = false;
            HasShadow = true;
            ShadowSize = 5;
            BorderRadius = UIConstants.Borders.RadiusMedium;
            Padding = new Padding(
                UIConstants.Spacing.Padding.Medium,
                UIConstants.Spacing.Padding.Large,
                UIConstants.Spacing.Padding.Medium,
                UIConstants.Spacing.Padding.Large
            );

            // App name label
            lblAppName = new Label
            {
                Text = $"WAREHOUSE",
                Font = ThemeManager.Instance.FontBold,
                ForeColor = UIConstants.PrimaryColor.Default,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(lblAppName);
        }
    }
}
