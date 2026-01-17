using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;

namespace WarehouseManagement.Repositories
{
    /// <summary>
    /// Repository Ä‘á»ƒ quáº£n lÃ½ phiáº¿u Nháº­p/Xuáº¥t kho
    /// </summary>
    public class StockTransactionRepository : BaseRepository
    {
        private readonly TransactionDetailRepository _detailRepo;

        public StockTransactionRepository()
        {
            _detailRepo = new TransactionDetailRepository();
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ phiáº¿u
        /// </summary>
        public List<StockTransaction> GetAllTransactions()
        {
            var transactions = new List<StockTransaction>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM StockTransactions ORDER BY DateCreated DESC", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new StockTransaction
                                {
                                    TransactionID = reader.GetInt32("TransactionID"),
                                    Type = reader.GetString("Type"),
                                    DateCreated = reader.GetDateTime("DateCreated"),
                                    CreatedByUserID = reader.IsDBNull(reader.GetOrdinal("CreatedByUserID")) ? 0 : reader.GetInt32("CreatedByUserID"),
                                    Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString("Note")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh sÃ¡ch phiáº¿u: " + ex.Message);
            }
            return transactions;
        }

        /// <summary>
        /// Láº¥y phiáº¿u theo ID (bao gá»“m chi tiáº¿t)
        /// </summary>
        public StockTransaction GetTransactionById(int transactionId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    
                    var transaction = new StockTransaction { TransactionID = transactionId };
                    
                    // Láº¥y thÃ´ng tin giao dá»‹ch
                    using (var cmd = new MySqlCommand("SELECT * FROM StockTransactions WHERE TransactionID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", transactionId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                transaction.TransactionID = reader.GetInt32("TransactionID");
                                transaction.Type = reader.GetString("Type");
                                transaction.DateCreated = reader.GetDateTime("DateCreated");
                                transaction.CreatedByUserID = reader.IsDBNull(reader.GetOrdinal("CreatedByUserID")) ? 0 : reader.GetInt32("CreatedByUserID");
                                transaction.Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString("Note");
                            }
                        }
                    }

                    // Láº¥y chi tiáº¿t giao dá»‹ch
                    transaction.Details = _detailRepo.GetDetailsByTransactionId(transactionId);
                    
                    return transaction;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lá»—i khi láº¥y phiáº¿u ID {transactionId}: " + ex.Message);
            }
        }

        /// <summary>
        /// Táº¡o phiáº¿u nháº­p/xuáº¥t má»›i
        /// </summary>
        public int CreateTransaction(StockTransaction transaction)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) " +
                        "VALUES (@type, @date, @userId, @note); SELECT LAST_INSERT_ID();", conn))
                    {
                        cmd.Parameters.AddWithValue("@type", transaction.Type);
                        cmd.Parameters.AddWithValue("@date", transaction.DateCreated);
                        cmd.Parameters.AddWithValue("@userId", transaction.CreatedByUserID);
                        cmd.Parameters.AddWithValue("@note", transaction.Note ?? "");
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi táº¡o phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t thÃ´ng tin phiáº¿u (khÃ´ng cáº­p nháº­t chi tiáº¿t)
        /// </summary>
        public bool UpdateTransaction(StockTransaction transaction)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE StockTransactions SET Type=@type, Note=@note WHERE TransactionID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@type", transaction.Type);
                        cmd.Parameters.AddWithValue("@note", transaction.Note ?? "");
                        cmd.Parameters.AddWithValue("@id", transaction.TransactionID);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi cáº­p nháº­t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a phiáº¿u (CASCADE xÃ³a chi tiáº¿t tá»± Ä‘á»™ng)
        /// </summary>
        public bool DeleteTransaction(int transactionId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("DELETE FROM StockTransactions WHERE TransactionID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", transactionId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch phiáº¿u theo loáº¡i (Import/Export)
        /// </summary>
        public List<StockTransaction> GetTransactionsByType(string type)
        {
            var transactions = new List<StockTransaction>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT * FROM StockTransactions WHERE Type=@type ORDER BY DateCreated DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@type", type);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new StockTransaction
                                {
                                    TransactionID = reader.GetInt32("TransactionID"),
                                    Type = reader.GetString("Type"),
                                    DateCreated = reader.GetDateTime("DateCreated"),
                                    CreatedByUserID = reader.IsDBNull(reader.GetOrdinal("CreatedByUserID")) ? 0 : reader.GetInt32("CreatedByUserID"),
                                    Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString("Note")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y phiáº¿u theo loáº¡i: " + ex.Message);
            }
            return transactions;
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch phiáº¿u trong má»™t khoáº£ng thá»i gian
        /// </summary>
        public List<StockTransaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            var transactions = new List<StockTransaction>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT * FROM StockTransactions WHERE DateCreated BETWEEN @start AND @end ORDER BY DateCreated DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@start", startDate);
                        cmd.Parameters.AddWithValue("@end", endDate);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new StockTransaction
                                {
                                    TransactionID = reader.GetInt32("TransactionID"),
                                    Type = reader.GetString("Type"),
                                    DateCreated = reader.GetDateTime("DateCreated"),
                                    CreatedByUserID = reader.IsDBNull(reader.GetOrdinal("CreatedByUserID")) ? 0 : reader.GetInt32("CreatedByUserID"),
                                    Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString("Note")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y phiáº¿u theo ngÃ y: " + ex.Message);
            }
            return transactions;
        }
    }
}




