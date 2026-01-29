using System;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.UI;

namespace WarehouseManagement.Views.Forms
{
    public class StatusActionDialog : Form
    {
        public StatusActionDialog(string title, string message)
        {
            InitializeComponent(title, message);
            ApplyTheme();
        }

        private void InitializeComponent(string title, string message)
        {
            this.Text = title;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(400, 200);
            this.Padding = new Padding(20);

            var lblMessage = new Label
            {
                Text = message,
                Dock = DockStyle.Top,
                Height = 80,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = ThemeManager.Instance.FontRegular
            };

            var btnApprove = new Button
            {
                Text = "Duyệt (Approve)",
                DialogResult = DialogResult.Yes,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = ThemeManager.Instance.FontBold
            };
            btnApprove.FlatAppearance.BorderSize = 0;
            btnApprove.BackColor = UIConstants.SemanticColors.Success; // Green
            btnApprove.ForeColor = UIConstants.TextOnColor.Default;

            var btnCancelTrans = new Button
            {
                Text = "Hủy Phiếu (Cancel)",
                DialogResult = DialogResult.No,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = ThemeManager.Instance.FontBold
            };
            btnCancelTrans.FlatAppearance.BorderSize = 0;
            btnCancelTrans.BackColor = UIConstants.SemanticColors.Error; // Red
            btnCancelTrans.ForeColor = UIConstants.TextOnColor.Default;

            var btnExit = new Button
            {
                Text = "Thoát (Exit)",
                DialogResult = DialogResult.Cancel,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = ThemeManager.Instance.FontRegular
            };
            btnExit.FlatAppearance.BorderColor = UIConstants.NeutralColors.Border;
            btnExit.BackColor = UIConstants.ChartColors.Background;
            btnExit.ForeColor = UIConstants.TextLight.Primary;

            // Layout using TableLayoutPanel for buttons
            var tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                ColumnCount = 3,
                RowCount = 1
            };
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            
            tableLayout.Controls.Add(btnApprove, 0, 0);
            tableLayout.Controls.Add(btnCancelTrans, 1, 0);
            tableLayout.Controls.Add(btnExit, 2, 0);

            // Make buttons fill their cells with some margin
            btnApprove.Dock = DockStyle.Fill;
            btnCancelTrans.Dock = DockStyle.Fill;
            btnExit.Dock = DockStyle.Fill;
            
            // Add margins manually via a wrapper or just simple spacing? 
            // Letting them touch is cleaner for a modern look, or use margin property.
            btnApprove.Margin = new Padding(5);
            btnCancelTrans.Margin = new Padding(5);
            btnExit.Margin = new Padding(5);

            this.Controls.Add(tableLayout);
            this.Controls.Add(lblMessage);
        }

        private void ApplyTheme()
        {
            this.BackColor = ThemeManager.Instance.BackgroundDefault;
            this.ForeColor = ThemeManager.Instance.TextPrimary;
        }
    }
}
