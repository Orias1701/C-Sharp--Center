using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace WarehouseManagement.Repositories
{
    /// <summary>
    /// Lá»›p cÆ¡ sá»Ÿ cho táº¥t cáº£ repositories - khá»Ÿi táº¡o káº¿t ná»‘i MySQL
    /// </summary>
    public abstract class BaseRepository
    {
        protected string _connectionString;

        public BaseRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["WarehouseDB"]?.ConnectionString
                ?? throw new ConfigurationErrorsException("Connection string 'WarehouseDB' not found in App.config");
        }

        /// <summary>
        /// Láº¥y káº¿t ná»‘i má»›i Ä‘áº¿n database
        /// </summary>
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        /// <summary>
        /// Kiá»ƒm tra káº¿t ná»‘i tá»›i database
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
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




