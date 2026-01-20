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
            Size = new Size(500, 480);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            int y = 20;
            int labelWidth = 100;
            int inputWidth = 340;
            int margin = 20;

            // Name
            var lblName = CreateLabel("Tên KH (*):", margin, y);
            txtName = new CustomTextBox { Left = margin + labelWidth, Top = y, Width = inputWidth };
            Controls.Add(lblName);
            Controls.Add(txtName);
            y += 50;

            // Phone
            var lblPhone = CreateLabel("Số điện thoại:", margin, y);
            txtPhone = new CustomTextBox { Left = margin + labelWidth, Top = y, Width = inputWidth };
            Controls.Add(lblPhone);
            Controls.Add(txtPhone);
            y += 50;

            // Email
            var lblEmail = CreateLabel("Email:", margin, y);
            txtEmail = new CustomTextBox { Left = margin + labelWidth, Top = y, Width = inputWidth };
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            y += 50;

            // Address
            var lblAddress = CreateLabel("Địa chỉ:", margin, y);
            txtAddress = new CustomTextArea { Left = margin + labelWidth, Top = y, Width = inputWidth, Height = 100 };
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            y += 120;

            // Buttons
            btnSave = new CustomButton
            {
                Text = "Lưu",
                Left = 250,
                Top = y,
                Width = 100,
                Height = 35,
                ButtonStyleType = ButtonStyle.Filled
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new CustomButton
            {
                Text = "Hủy",
                Left = 360,
                Top = y,
                Width = 100,
                Height = 35,
                ButtonStyleType = ButtonStyle.Outlined
            };
            btnCancel.Click += (s, e) => Close();

            Controls.Add(btnSave);
            Controls.Add(btnCancel);

            if (_isEditMode)
            {
                txtName.Text = _customer.CustomerName;
                txtPhone.Text = _customer.Phone;
                txtEmail.Text = _customer.Email;
                txtAddress.Text = _customer.Address;
            }
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Left = x,
                Top = y + 5,
                Width = 100,
                AutoSize = false,
                Font = ThemeManager.Instance.FontRegular
            };
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
