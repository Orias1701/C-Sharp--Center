using System;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
using WarehouseManagement.Repositories;

namespace WarehouseManagement.Helpers
{
    /// <summary>
    /// Helper kiá»ƒm tra káº¿t ná»‘i database vÃ  backup
    /// </summary>
    public class DatabaseHelper
    {
        public DatabaseHelper()
        {
            // BaseRepository lÃ  abstract, khÃ´ng cáº§n khá»Ÿi táº¡o
        }

        /// <summary>
        /// Kiá»ƒm tra káº¿t ná»‘i Ä‘áº¿n database
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
        /// Láº¥y thÃ´ng bÃ¡o lá»—i káº¿t ná»‘i
        /// </summary>
        public static string GetConnectionError()
        {
            try
            {
                if (!TestDatabaseConnection())
                    return "KhÃ´ng thá»ƒ káº¿t ná»‘i tá»›i database. Vui lÃ²ng kiá»ƒm tra App.config";
                return "";
            }
            catch (Exception ex)
            {
                return "Lá»—i: " + ex.Message;
            }
        }

        /// <summary>
        /// Tá»± Ä‘á»™ng cháº¡y schema.sql Ä‘á»ƒ táº¡o/reset database
        /// </summary>
        public static bool ExecuteSchema()
        {
            try
            {
                string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "SQL", "schema.sql");
                
                if (!File.Exists(schemaPath))
                    throw new FileNotFoundException("KhÃ´ng tÃ¬m tháº¥y file schema.sql");

                string sqlScript = File.ReadAllText(schemaPath, Encoding.UTF8);
                
                // Get connection string without specifying database
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["WarehouseDB"].ConnectionString;
                string connWithoutDb = connectionString.Replace("Database=QL_KhoHang;", "").Replace("QL_KhoHang;", "");
                
                using (var conn = new MySqlConnection(connWithoutDb))
                {
                    conn.Open();
                    
                    // Split script by semicolon and execute each command
                    string[] commands = sqlScript.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach (string command in commands)
                    {
                        if (string.IsNullOrWhiteSpace(command))
                            continue;
                            
                        using (var cmd = new MySqlCommand(command.Trim(), conn))
                        {
                            cmd.CommandTimeout = 30;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi cháº¡y schema: " + ex.Message);
            }
        }

        /// <summary>
        /// Táº¡o file backup Ä‘Æ¡n giáº£n (chá»‰ lÆ°u thÃ´ng tin)
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
                throw new Exception("Lá»—i khi táº¡o backup: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y kÃ­ch thÆ°á»›c database (mÃ´ phá»ng)
        /// </summary>
        public static string GetDatabaseSize()
        {
            try
            {
                return "ChÆ°a cáº­p nháº­t - cáº§n triá»ƒn khai MySQL INFORMATION_SCHEMA";
            }
            catch (Exception ex)
            {
                return "Lá»—i: " + ex.Message;
            }
        }
    }
}




