using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;

namespace WarehouseManagement.Repositories
{
    /// <summary>
    /// Repository Ä‘á»ƒ quáº£n lÃ½ chi tiáº¿t phiáº¿u Nháº­p/Xuáº¥t kho
    /// </summary>
    public class TransactionDetailRepository : BaseRepository
    {
        /// <summary>
        /// Láº¥y danh sÃ¡ch chi tiáº¿t theo Transaction ID
        /// </summary>
        public List<TransactionDetail> GetDetailsByTransactionId(int transactionId)
        {
            var details = new List<TransactionDetail>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT * FROM TransactionDetails WHERE TransactionID=@transId", conn))
                    {
                        cmd.Parameters.AddWithValue("@transId", transactionId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                details.Add(new TransactionDetail
                                {
                                    DetailID = reader.GetInt32("DetailID"),
                                    TransactionID = reader.GetInt32("TransactionID"),
                                    ProductID = reader.GetInt32("ProductID"),
                                    ProductName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? "" : reader.GetString("ProductName"),
                                    Quantity = reader.GetInt32("Quantity"),
                                    UnitPrice = reader.GetDecimal("UnitPrice")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y chi tiáº¿t phiáº¿u: " + ex.Message);
            }
            return details;
        }

        /// <summary>
        /// Láº¥y chi tiáº¿t theo ID
        /// </summary>
        public TransactionDetail GetDetailById(int detailId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT * FROM TransactionDetails WHERE DetailID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", detailId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TransactionDetail
                                {
                                    DetailID = reader.GetInt32("DetailID"),
                                    TransactionID = reader.GetInt32("TransactionID"),
                                    ProductID = reader.GetInt32("ProductID"),
                                    ProductName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? "" : reader.GetString("ProductName"),
                                    Quantity = reader.GetInt32("Quantity"),
                                    UnitPrice = reader.GetDecimal("UnitPrice")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lá»—i khi láº¥y chi tiáº¿t ID {detailId}: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// ThÃªm chi tiáº¿t vÃ o phiáº¿u
        /// </summary>
        public int AddTransactionDetail(TransactionDetail detail)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) " +
                        "VALUES (@transId, @prodId, @prodName, @qty, @price); SELECT LAST_INSERT_ID();", conn))
                    {
                        cmd.Parameters.AddWithValue("@transId", detail.TransactionID);
                        cmd.Parameters.AddWithValue("@prodId", detail.ProductID);
                        cmd.Parameters.AddWithValue("@prodName", detail.ProductName ?? "");
                        cmd.Parameters.AddWithValue("@qty", detail.Quantity);
                        cmd.Parameters.AddWithValue("@price", detail.UnitPrice);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi thÃªm chi tiáº¿t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t chi tiáº¿t phiáº¿u
        /// </summary>
        public bool UpdateTransactionDetail(TransactionDetail detail)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE TransactionDetails SET ProductID=@prodId, ProductName=@prodName, Quantity=@qty, UnitPrice=@price " +
                        "WHERE DetailID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@prodId", detail.ProductID);
                        cmd.Parameters.AddWithValue("@prodName", detail.ProductName ?? "");
                        cmd.Parameters.AddWithValue("@qty", detail.Quantity);
                        cmd.Parameters.AddWithValue("@price", detail.UnitPrice);
                        cmd.Parameters.AddWithValue("@id", detail.DetailID);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi cáº­p nháº­t chi tiáº¿t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a chi tiáº¿t phiáº¿u
        /// </summary>
        public bool DeleteTransactionDetail(int detailId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "DELETE FROM TransactionDetails WHERE DetailID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", detailId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a chi tiáº¿t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a táº¥t cáº£ chi tiáº¿t cá»§a má»™t phiáº¿u
        /// </summary>
        public bool DeleteAllDetails(int transactionId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "DELETE FROM TransactionDetails WHERE TransactionID=@transId", conn))
                    {
                        cmd.Parameters.AddWithValue("@transId", transactionId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a chi tiáº¿t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y tá»•ng sá»‘ sáº£n pháº©m trong chi tiáº¿t phiáº¿u
        /// </summary>
        public int GetTotalQuantity(int transactionId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT SUM(Quantity) FROM TransactionDetails WHERE TransactionID=@transId", conn))
                    {
                        cmd.Parameters.AddWithValue("@transId", transactionId);
                        var result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi tÃ­nh tá»•ng sá»‘ lÆ°á»£ng: " + ex.Message);
            }
        }
    }
}




