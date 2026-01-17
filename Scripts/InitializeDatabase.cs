using System;
using System.IO;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WarehouseManagement.Scripts
{
    /// <summary>
    /// Script khởi tạo cơ sở dữ liệu từ file SQL (schema và seed data)
    /// </summary>
    public class InitializeDatabase
    {
        private readonly string _connectionString;

        public InitializeDatabase()
        {
            // Lấy connection string từ file App.config
            _connectionString = ConfigurationManager.ConnectionStrings["WarehouseDB"].ConnectionString;
        }

        /// <summary>
        /// Thực thi script SQL từ file
        /// </summary>
        /// <param name="filePath">Đường dẫn tương đối đến file .sql</param>
        public void ExecuteSqlScript(string filePath)
        {
            string scriptContent;
            try
            {
                // Đọc nội dung file SQL
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                // Điều chỉnh đường dẫn để tìm đúng file trong thư mục Assets/SQL
                // Khi chạy debug, file exe thường nằm trong bin/Debug/, cần đi ngược ra
                string fullPath = Path.Combine(baseDir, "..", "..", filePath); 
                
                if (!File.Exists(fullPath))
                {
                    // Thử tìm trực tiếp nếu file được copy to output directory
                    fullPath = Path.Combine(baseDir, filePath);
                    if (!File.Exists(fullPath))
                    {
                        Console.WriteLine($"Không tìm thấy file SQL: {filePath}");
                        return;
                    }
                }

                scriptContent = File.ReadAllText(fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đọc file SQL: {ex.Message}");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    
                    // MySqlScript hỗ trợ chạy nhiều lệnh SQL phân cách bởi dấu ;
                    MySqlScript script = new MySqlScript(conn, scriptContent);
                    script.Delimiter = ";";
                    int count = script.Execute();
                    
                    Console.WriteLine($"Đã thực thi script {filePath} thành công. {count} lệnh được thực hiện.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi thực thi SQL: {ex.Message}");
            }
        }

        /// <summary>
        /// Chạy toàn bộ quá trình khởi tạo (Schema + Seed)
        /// </summary>
        public void RunInitialization()
        {
            Console.WriteLine("Bắt đầu khởi tạo cơ sở dữ liệu...");
            
            // 1. Tạo bảng (Schema)
            ExecuteSqlScript("Assets/SQL/schema.sql");
            
            // 2. Chèn dữ liệu mẫu (Seed)
            ExecuteSqlScript("Assets/SQL/seed.sql");
            
            Console.WriteLine("Hoàn tất khởi tạo cơ sở dữ liệu!");
        }
    }
}