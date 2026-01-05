using System;
using System.Windows.Forms;
using WarehouseManagement.Helpers;
using WarehouseManagement.Views;

namespace WarehouseManagement
{
    static class Program
    {
        /// <summary>
        /// Điểm vào chính của ứng dụng
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Kiểm tra kết nối database trước khi khởi chạy
            if (!DatabaseHelper.TestDatabaseConnection())
            {
                MessageBox.Show(
                    "Không thể kết nối tới database!\n\nVui lòng kiểm tra:\n" +
                    "1. MySQL Server đang chạy\n" +
                    "2. Connection String trong App.config đúng\n\n" +
                    DatabaseHelper.GetConnectionError(),
                    "Lỗi Kết Nối Database",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
