using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WarehouseManagement.Helpers
{
    /// <summary>
    /// Helper quản lý kết nối và thực thi lệnh SQL chung
    /// </summary>
    public static class DatabaseHelper
    {
        // Lấy chuỗi kết nối từ App.config
        private static string connectionString = ConfigurationManager.ConnectionStrings["WarehouseDB"].ConnectionString;

        /// <summary>
        /// Lấy đối tượng kết nối MySqlConnection
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Thực thi câu lệnh SQL trả về DataTable (SELECT)
        /// </summary>
        public static DataTable ExecuteQuery(string query, params MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thực thi truy vấn: " + ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// Thực thi câu lệnh SQL không trả về dữ liệu (INSERT, UPDATE, DELETE)
        /// Trả về số dòng bị ảnh hưởng
        /// </summary>
        public static int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thực thi lệnh: " + ex.Message);
            }
        }

        /// <summary>
        /// Thực thi câu lệnh SQL trả về giá trị đơn (Scalar - COUNT, MAX, SUM...)
        /// </summary>
        public static object ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thực thi Scalar: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}