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
    public class TransactionRepository : BaseRepository
    {
        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ phiáº¿u (bao gá»“m chi tiáº¿t)
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
                    
                    // Load chi tiáº¿t cho má»—i phiáº¿u
                    foreach (var trans in transactions)
                    {
                        using (var detailCmd = new MySqlCommand(
                            "SELECT * FROM TransactionDetails WHERE TransactionID=@transId", conn))
                        {
                            detailCmd.Parameters.AddWithValue("@transId", trans.TransactionID);
                            using (var detailReader = detailCmd.ExecuteReader())
                            {
                                while (detailReader.Read())
                                {
                                    trans.Details.Add(new TransactionDetail
                                    {
                                        DetailID = detailReader.GetInt32("DetailID"),
                                        TransactionID = detailReader.GetInt32("TransactionID"),
                                        ProductID = detailReader.GetInt32("ProductID"),
                                        ProductName = detailReader.IsDBNull(detailReader.GetOrdinal("ProductName")) ? "" : detailReader.GetString("ProductName"),
                                        Quantity = detailReader.GetInt32("Quantity"),
                                        UnitPrice = detailReader.GetDecimal("UnitPrice")
                                    });
                                }
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

                    // Láº¥y chi tiáº¿t giao dá»‹ch - reader cÅ© Ä‘Ã£ Ä‘Ã³ng
                    using (var detailCmd = new MySqlCommand(
                        "SELECT * FROM TransactionDetails WHERE TransactionID=@transId", conn))
                    {
                        detailCmd.Parameters.AddWithValue("@transId", transactionId);
                        using (var detailReader = detailCmd.ExecuteReader())
                        {
                            int detailCount = 0;
                            while (detailReader.Read())
                            {
                                transaction.Details.Add(new TransactionDetail
                                {
                                    DetailID = detailReader.GetInt32("DetailID"),
                                    TransactionID = detailReader.GetInt32("TransactionID"),
                                    ProductID = detailReader.GetInt32("ProductID"),
                                    ProductName = detailReader.IsDBNull(detailReader.GetOrdinal("ProductName")) ? "" : detailReader.GetString("ProductName"),
                                    Quantity = detailReader.GetInt32("Quantity"),
                                    UnitPrice = detailReader.GetDecimal("UnitPrice")
                                });
                                detailCount++;
                            }
                        }
                    }
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
                        "INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note, TotalValue) " +
                        "VALUES (@type, @date, @userId, @note, @totalValue); SELECT LAST_INSERT_ID();", conn))
                    {
                        cmd.Parameters.AddWithValue("@type", transaction.Type);
                        cmd.Parameters.AddWithValue("@date", transaction.DateCreated);
                        cmd.Parameters.AddWithValue("@userId", transaction.CreatedByUserID);
                        cmd.Parameters.AddWithValue("@note", transaction.Note ?? "");
                        cmd.Parameters.AddWithValue("@totalValue", transaction.TotalValue);
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
        /// ThÃªm chi tiáº¿t vÃ o phiáº¿u
        /// </summary>
        public bool AddTransactionDetail(TransactionDetail detail)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) " +
                        "VALUES (@transId, @prodId, @prodName, @qty, @price)", conn))
                    {
                        cmd.Parameters.AddWithValue("@transId", detail.TransactionID);
                        cmd.Parameters.AddWithValue("@prodId", detail.ProductID);
                        cmd.Parameters.AddWithValue("@prodName", detail.ProductName ?? "");
                        cmd.Parameters.AddWithValue("@qty", detail.Quantity);
                        cmd.Parameters.AddWithValue("@price", detail.UnitPrice);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi thÃªm chi tiáº¿t phiáº¿u: " + ex.Message);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t tá»•ng giÃ¡ trá»‹ cá»§a phiáº¿u (sau khi thÃªm táº¥t cáº£ chi tiáº¿t)
        /// </summary>
        public bool UpdateTransactionTotalValue(int transactionId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE StockTransactions SET TotalValue = (SELECT COALESCE(SUM(Quantity * UnitPrice), 0) FROM TransactionDetails WHERE TransactionID = @transId) WHERE TransactionID = @transId", conn))
                    {
                        cmd.Parameters.AddWithValue("@transId", transactionId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi cáº­p nháº­t tá»•ng giÃ¡ trá»‹ phiáº¿u: " + ex.Message);
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
    }
}





