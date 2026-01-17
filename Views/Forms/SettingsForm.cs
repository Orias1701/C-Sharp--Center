using System;
using System.Windows.Forms;

namespace WarehouseManagement.Views.Forms
{
    /// <summary>
    /// Form Cài Đặt - Quản lý tùy chọn hiển thị
    /// </summary>
    public class SettingsForm : Form
    {
        private CheckBox chkShowHidden;
        private Button btnSave, btnCancel;

        // Static property to share settings across the app
        public static bool ShowHiddenItems { get; set; } = false;
        
        // Event to notify when settings change
        public static event EventHandler SettingsChanged;

        public SettingsForm()
        {
            InitializeComponent();
            Text = "Cài Đặt";
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // Label
            Label lblShowHidden = new Label
            {
                Text = "Xem các mục đã ẩn:",
                Left = 20,
                Top = 20,
                Width = 200,
                Height = 25,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };

            // CheckBox
            chkShowHidden = new CheckBox
            {
                Text = "Hiển thị tất cả các bản ghi đã ẩn",
                Left = 20,
                Top = 50,
                Width = 300,
                Height = 25,
                Checked = ShowHiddenItems
            };

            // Buttons
            btnSave = new Button
            {
                Text = "💾 Lưu",
                Left = 100,
                Top = 100,
                Width = 100,
                Height = 35
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "❌ Hủy",
                Left = 210,
                Top = 100,
                Width = 100,
                Height = 35,
                DialogResult = DialogResult.Cancel
            };

            Controls.Add(lblShowHidden);
            Controls.Add(chkShowHidden);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);

            Width = 400;
            Height = 180;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            CancelButton = btnCancel;

            ResumeLayout(false);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Update static property
            ShowHiddenItems = chkShowHidden.Checked;

            // Notify all listeners
            SettingsChanged?.Invoke(this, EventArgs.Empty);

            MessageBox.Show("Cài đặt đã được lưu.");
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
