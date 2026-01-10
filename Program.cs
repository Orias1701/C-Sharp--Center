using System;
using System.Windows.Forms;
using WarehouseManagement.Helpers;
using WarehouseManagement.Views;
using WarehouseManagement.Models;
using System.Diagnostics;
using System.IO;
using System.Text;

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
            try
            {
                // Initialize application visual styles FIRST before any UI operations
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Set UTF-8 encoding cho console và file output
                // Console.OutputEncoding = Encoding.UTF8;
                
                // Thêm listener để log ra file với UTF-8 encoding
                string logPath = Path.Combine(
                    Path.GetDirectoryName(Application.ExecutablePath),
                    "debug.log");
                var fileStream = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
                var listener = new TextWriterTraceListener(streamWriter);
                Debug.Listeners.Add(listener);
                Debug.WriteLine($"[Program] Ứng dụng khởi động lúc: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                Debug.Flush();

                // Tự động chạy schema.sql lần đầu
                try
                {
                    DatabaseHelper.ExecuteSchema();
                }
                catch (Exception schemaEx)
                {
                    // Schema có thể đã tồn tại, không cần lỗi
                    Debug.WriteLine($"[Program] Schema error (expected if exists): {schemaEx.Message}");
                }

                // Kiểm tra kết nối database
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
                    Debug.WriteLine("[Program] Database connection failed, exiting");
                    return;
                }

                Debug.WriteLine("[Program] Database connection successful");

                // Hiển thị LoginForm
                LoginForm loginForm = new LoginForm();
                if (loginForm.ShowDialog() == DialogResult.OK && GlobalUser.CurrentUser != null)
                {
                    Debug.WriteLine($"[Program] User logged in: {GlobalUser.CurrentUser.Username}");
                    Application.Run(new MainForm());
                }
                else
                {
                    Debug.WriteLine("[Program] Login cancelled or failed");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Program] Fatal error: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show("Lỗi khởi động ứng dụng: " + ex.Message + "\n\n" + ex.StackTrace, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
