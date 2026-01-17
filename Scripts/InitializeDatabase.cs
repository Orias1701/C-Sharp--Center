using System;
using System.IO;
using MySql.Data.MySqlClient;

class DatabaseInitializer
{
    static void Main()
    {
        try
        {
            string connectionString = "Server=localhost;Uid=root;Pwd=LongK@170105;CharSet=utf8mb4;";
            string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "SQL", "schema.sql");
            
            if (!File.Exists(schemaPath))
            {
                Console.WriteLine($"Schema file not found at: {schemaPath}");
                return;
            }

            string sqlContent = File.ReadAllText(schemaPath);
            
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                Console.WriteLine("Connected to MySQL server.");
                
                // Split by GO statements or semicolon
                string[] statements = sqlContent.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (string stmt in statements)
                {
                    string trimmed = stmt.Trim();
                    if (string.IsNullOrWhiteSpace(trimmed)) continue;
                    
                    using (var cmd = new MySqlCommand(trimmed, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                
                conn.Close();
                Console.WriteLine("Schema script executed successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}




