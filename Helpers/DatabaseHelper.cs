using System;
using System.IO;
using WarehouseManagement.Repositories;

namespace WarehouseManagement.Helpers
{
    /// <summary>
    /// Helper kiểm tra kết nối database và backup
    /// </summary>
    public class DatabaseHelper
    {
        public DatabaseHelper()
        {
            // BaseRepository là abstract, không cần khởi tạo
        }

        /// <summary>
        /// Kiểm tra kết nối đến database
        /// </summary>
        public static bool TestDatabaseConnection()
        {
            try
            {
                var repo = new ProductRepository();
                return repo.TestConnection();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy thông báo lỗi kết nối
        /// </summary>
        public static string GetConnectionError()
        {
            try
            {
                if (!TestDatabaseConnection())
                    return "Không thể kết nối tới database. Vui lòng kiểm tra App.config";
                return "";
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }

        /// <summary>
        /// Tạo file backup đơn giản (chỉ lưu thông tin)
        /// </summary>
        public static bool CreateBackup(string backupPath)
        {
            try
            {
                if (!Directory.Exists(backupPath))
                    Directory.CreateDirectory(backupPath);

                string filename = Path.Combine(backupPath, $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine($"Backup created at: {DateTime.Now}");
                    writer.WriteLine("Database: QL_KhoHang");
                    writer.WriteLine("Status: OK");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo backup: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy kích thước database (mô phỏng)
        /// </summary>
        public static string GetDatabaseSize()
        {
            try
            {
                return "Chưa cập nhật - cần triển khai MySQL INFORMATION_SCHEMA";
            }
            catch (Exception ex)
            {
                return "Lỗi: " + ex.Message;
            }
        }
    }
}
