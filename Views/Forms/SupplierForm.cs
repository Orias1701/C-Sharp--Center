using System;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Models;
using WarehouseManagement.Controllers;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;

namespace WarehouseManagement.Views.Forms
{
    public class SupplierForm : Form
    {
        private readonly SupplierController _controller;
        private Supplier _supplier;
        private bool _isEditMode;

        private CustomTextBox txtName;
        private CustomTextBox txtPhone;
        private CustomTextBox txtEmail;
        private CustomTextArea txtAddress;
        private CustomButton btnSave;
        private CustomButton btnCancel;

        public SupplierForm(Supplier supplier = null)
        {
            _controller = new SupplierController();
            _supplier = supplier;
            _isEditMode = supplier != null;

            InitializeComponent();
            ThemeManager.Instance.ApplyThemeToForm(this);
        }

        private void InitializeComponent()
        {
            Text = _isEditMode ? "Sửa Nhà Cung Cấp" : "Thêm Nhà Cung Cấp";
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
            var lblName = CreateLabel("Tên NCC (*):", margin, y);
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
                txtName.Text = _supplier.SupplierName;
                txtPhone.Text = _supplier.Phone;
                txtEmail.Text = _supplier.Email;
                txtAddress.Text = _supplier.Address;
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
                    MessageBox.Show("Vui lòng nhập tên nhà cung cấp", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_isEditMode)
                {
                    _supplier.SupplierName = txtName.Text.Trim();
                    _supplier.Phone = txtPhone.Text.Trim();
                    _supplier.Email = txtEmail.Text.Trim();
                    _supplier.Address = txtAddress.Text.Trim();

                    if (_controller.UpdateSupplier(_supplier))
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
                    var newSupplier = new Supplier
                    {
                        SupplierName = txtName.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Address = txtAddress.Text.Trim(),
                        Visible = true
                    };

                    if (_controller.AddSupplier(newSupplier))
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
