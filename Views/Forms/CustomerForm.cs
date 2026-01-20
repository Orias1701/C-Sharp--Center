using System;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Models;
using WarehouseManagement.Controllers;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;

namespace WarehouseManagement.Views.Forms
{
    public class CustomerForm : Form
    {
        private readonly CustomerController _controller;
        private Customer _customer;
        private bool _isEditMode;

        private CustomTextBox txtName;
        private CustomTextBox txtPhone;
        private CustomTextBox txtEmail;
        private CustomTextArea txtAddress;
        private CustomButton btnSave;
        private CustomButton btnCancel;

        public CustomerForm(Customer customer = null)
        {
            _controller = new CustomerController();
            _customer = customer;
            _isEditMode = customer != null;

            InitializeComponent();
            ThemeManager.Instance.ApplyThemeToForm(this);
        }

        private void InitializeComponent()
        {
            Text = _isEditMode ? "Sửa Khách Hàng" : "Thêm Khách Hàng";
            
            // Main container
            CustomPanel mainPanel = new CustomPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = UIConstants.Borders.RadiusLarge,
                ShowBorder = false,
                Padding = new Padding(0)
            };

            // Layout constants
            const int INPUT_WIDTH = 400;
            const int LEFT_MARGIN = 40;
            int currentY = 30;
            int inputSpacing = 20;

            // Helper to create styled labels
            Label CreateStyledLabel(string text, int y)
            {
                return new Label
                {
                    Text = text,
                    Left = LEFT_MARGIN,
                    Top = y,
                    AutoSize = true,
                    Font = ThemeManager.Instance.FontSmall,
                    ForeColor = Color.FromArgb(180, UIConstants.PrimaryColor.Default.R, 
                                              UIConstants.PrimaryColor.Default.G, 
                                              UIConstants.PrimaryColor.Default.B),
                    TabStop = false
                };
            }

            // Name
            mainPanel.Controls.Add(CreateStyledLabel("Tên KH (*)", currentY));
            currentY += 20;

            txtName = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = INPUT_WIDTH,
                Placeholder = "Nhập tên khách hàng..."
            };
            mainPanel.Controls.Add(txtName);
            currentY += UIConstants.Sizes.InputHeight + inputSpacing;

            // Phone
            mainPanel.Controls.Add(CreateStyledLabel("Số điện thoại", currentY));
            currentY += 20;

            txtPhone = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = INPUT_WIDTH,
                Placeholder = "Nhập số điện thoại..."
            };
            mainPanel.Controls.Add(txtPhone);
            currentY += UIConstants.Sizes.InputHeight + inputSpacing;

            // Email
            mainPanel.Controls.Add(CreateStyledLabel("Email", currentY));
            currentY += 20;

            txtEmail = new CustomTextBox 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = INPUT_WIDTH,
                Placeholder = "Nhập email..."
            };
            mainPanel.Controls.Add(txtEmail);
            currentY += UIConstants.Sizes.InputHeight + inputSpacing;

            // Address
            mainPanel.Controls.Add(CreateStyledLabel("Địa chỉ", currentY));
            currentY += 20;

            txtAddress = new CustomTextArea 
            { 
                Left = LEFT_MARGIN, 
                Top = currentY, 
                Width = INPUT_WIDTH, 
                Height = 100,
                Placeholder = "Nhập địa chỉ..."
            };
            mainPanel.Controls.Add(txtAddress);
            currentY += 100 + 30;

            // Buttons - Centered
            int totalButtonWidth = 120 + 10 + 120;
            int buttonStartX = LEFT_MARGIN + (INPUT_WIDTH - totalButtonWidth) / 2;

            btnSave = new CustomButton
            {
                Text = "Lưu",
                Left = buttonStartX,
                Top = currentY,
                Width = 120,
                Height = 35,
                ButtonStyleType = ButtonStyle.FilledNoOutline
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new CustomButton
            {
                Text = "Hủy",
                Left = buttonStartX + 120 + 10,
                Top = currentY,
                Width = 120,
                Height = 35,
                ButtonStyleType = ButtonStyle.Outlined,
                CausesValidation = false
            };
            btnCancel.Click += (s, e) => Close();

            mainPanel.Controls.Add(btnSave);
            mainPanel.Controls.Add(btnCancel);

            Controls.Add(mainPanel);

            // Calculate height
            int contentHeight = currentY + 35; // Bottom of buttons
            int paddingBottom = 40;
            int calculatedHeight = contentHeight + paddingBottom;

            // Use ClientSize to ensure inner content area size
            ClientSize = new Size(480, calculatedHeight);
            
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = ThemeManager.Instance.BackgroundLight;
            AcceptButton = btnSave;
            CancelButton = btnCancel;

            if (_isEditMode)
            {
                txtName.Text = _customer.CustomerName;
                txtPhone.Text = _customer.Phone;
                txtEmail.Text = _customer.Email;
                txtAddress.Text = _customer.Address;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_isEditMode)
                {
                    _customer.CustomerName = txtName.Text.Trim();
                    _customer.Phone = txtPhone.Text.Trim();
                    _customer.Email = txtEmail.Text.Trim();
                    _customer.Address = txtAddress.Text.Trim();

                    if (_controller.UpdateCustomer(_customer))
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var newCustomer = new Customer
                    {
                        CustomerName = txtName.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Address = txtAddress.Text.Trim(),
                        Visible = true
                    };

                    if (_controller.AddCustomer(newCustomer))
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
