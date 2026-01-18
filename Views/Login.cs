using System;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Helpers;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;

namespace WarehouseManagement.Views
{
    public partial class Login : Form
    {
        private UserController _userController;

        public Login()
        {
            InitializeComponent();
            _userController = new UserController();
            
            // Apply theme
            ThemeManager.Instance.ApplyThemeToForm(this);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Text = "Đăng Nhập Hệ Thống";
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackColor = ThemeManager.Instance.BackgroundLight;
            
            // Set active control để focus đúng
            ActiveControl = txtUsername;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Vui lòng nhập tên đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show($"{UIConstants.Icons.Warning} Vui lòng nhập mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                // Try login - pass raw password, UserController will hash it
                User user = _userController.Login(txtUsername.Text, txtPassword.Text);
                
                if (user != null && user.IsActive)
                {
                    GlobalUser.CurrentUser = user;
                    MessageBox.Show($"{UIConstants.Icons.Success} Đăng nhập thành công!\nChào mừng {user.FullName}!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (user != null && !user.IsActive)
                {
                    MessageBox.Show($"{UIConstants.Icons.Error} Tài khoản của bạn đã bị vô hiệu hóa.\nVui lòng liên hệ quản trị viên.", "Tài khoản bị khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Text = "";
                    txtPassword.Focus();
                }
                else
                {
                    MessageBox.Show($"{UIConstants.Icons.Error} Tên đăng nhập hoặc mật khẩu không chính xác.\nVui lòng thử lại.", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Text = "";
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                // Log error cho debug (nếu cần)
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                
                // Hiển thị thông báo thân thiện thay vì error stack
                MessageBox.Show($"{UIConstants.Icons.Error} Không thể đăng nhập.\n\nVui lòng kiểm tra lại thông tin đăng nhập.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = "";
                txtPassword.Focus();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                
                // Kiểm tra xem cả 2 ô đã được fill chưa
                bool usernameValid = !string.IsNullOrWhiteSpace(txtUsername.Text);
                bool passwordValid = !string.IsNullOrWhiteSpace(txtPassword.Text);
                
                if (usernameValid && passwordValid)
                {
                    // Cả 2 ô đều đã fill → Đăng nhập
                    BtnLogin_Click(null, null);
                }
                else if (!usernameValid)
                {
                    // Username chưa fill → Chuyển về username
                    txtUsername.Focus();
                }
                // Nếu chỉ password chưa fill thì giữ nguyên focus tại password
            }
        }

        private void TxtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                
                // Kiểm tra xem cả 2 ô đã được fill chưa
                bool usernameValid = !string.IsNullOrWhiteSpace(txtUsername.Text);
                bool passwordValid = !string.IsNullOrWhiteSpace(txtPassword.Text);
                
                if (usernameValid && passwordValid)
                {
                    // Cả 2 ô đều đã fill → Đăng nhập
                    BtnLogin_Click(null, null);
                }
                else if (!passwordValid)
                {
                    // Password chưa fill → Chuyển sang password
                    txtPassword.Focus();
                }
                // Nếu chỉ username chưa fill thì giữ nguyên focus tại username
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // Main container panel
            CustomPanel mainPanel = new CustomPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = UIConstants.Borders.RadiusLarge,
                ShowBorder = false,
                Padding = new Padding(UIConstants.Spacing.Padding.XXLarge)
            };

            // Title Label - Tối giản, không icon
            Label lblTitle = new Label
            {
                Text = "Đăng nhập",
                Font = ThemeManager.Instance.FontBold,
                ForeColor = Color.FromArgb(200, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B), // Primary nhạt 20%
                AutoSize = true,
                Location = new Point(50, 40),
                TabStop = false
            };

            // Username Label - Không icon, màu primary, nhỏ hơn
            lblUsername = new Label
            {
                Text = "Tên đăng nhập",
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B), // Primary nhạt 30%
                AutoSize = true,
                Location = new Point(50, 100),
                TabStop = false
            };

            // Username TextBox
            txtUsername = new CustomTextBox
            {
                Location = new Point(50, 125),
                Width = 380,
                Placeholder = "Nhập tên đăng nhập...",
                TabIndex = 0,
                TabStop = true
            };
            txtUsername.KeyDown += TxtUsername_KeyDown;

            // Password Label - Không icon, màu primary, nhỏ hơn
            lblPassword = new Label
            {
                Text = "Mật khẩu",
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                          UIConstants.PrimaryColor.Default.G, 
                                          UIConstants.PrimaryColor.Default.B), // Primary nhạt 30%
                AutoSize = true,
                Location = new Point(50, 185),
                TabStop = false
            };

            // Password TextBox
            txtPassword = new CustomTextBox
            {
                Location = new Point(50, 210),
                Width = 380,
                Placeholder = "Nhập mật khẩu...",
                IsPassword = true,
                TabIndex = 1,
                TabStop = true
            };
            txtPassword.KeyDown += TxtPassword_KeyDown;

            // Login Button
            btnLogin = new CustomButton
            {
                Text = "Đăng nhập",
                Location = new Point(150, 280),
                Width = 140,
                Height = UIConstants.Sizes.ButtonHeight,
                ButtonStyleType = ButtonStyle.FilledNoOutline,
                TabIndex = 2,
                TabStop = true
            };
            btnLogin.Click += BtnLogin_Click;

            // Cancel Button
            btnCancel = new CustomButton
            {
                Text = "Thoát",
                Location = new Point(300, 280),
                Width = 130,
                Height = UIConstants.Sizes.ButtonHeight,
                ButtonStyleType = ButtonStyle.Outlined,
                TabIndex = 3,
                TabStop = true
            };
            btnCancel.Click += BtnCancel_Click;

            // Add controls to main panel
            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(lblUsername);
            mainPanel.Controls.Add(txtUsername);
            mainPanel.Controls.Add(lblPassword);
            mainPanel.Controls.Add(txtPassword);
            mainPanel.Controls.Add(btnLogin);
            mainPanel.Controls.Add(btnCancel);

            // Form settings
            ClientSize = new Size(500, 380);
            Controls.Add(mainPanel);
            Name = "Login";
            Text = "Login";
            
            // Set default buttons
            AcceptButton = btnLogin;
            CancelButton = btnCancel;
            
            Load += Login_Load;
            
            ResumeLayout(false);
        }

        private Label lblUsername;
        private Label lblPassword;
        private CustomTextBox txtUsername;
        private CustomTextBox txtPassword;
        private CustomButton btnLogin;
        private CustomButton btnCancel;
    }
}