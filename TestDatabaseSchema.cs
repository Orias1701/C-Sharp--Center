using System;
using System.Configuration;
using MySql.Data.MySqlClient;

class TestSchema
{
    static void Main()
    {
        try
        {
            string connStr = "Server=localhost;Uid=root;Pwd=LongK@170105;Database=QL_KhoHang;CharSet=utf8mb4;";
            
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                
                // Check if Products table has Visible column
                var cmd = new MySqlCommand(
                    "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Products' AND COLUMN_NAME='Visible'", 
                    conn);
                
                var result = cmd.ExecuteScalar();
                
                if (result != null)
                {
                    Console.WriteLine("âœ“ Database schema is correct - 'Visible' column found in Products table!");
                }
                else
                {
                    Console.WriteLine("âœ— 'Visible' column NOT found in Products table");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}




