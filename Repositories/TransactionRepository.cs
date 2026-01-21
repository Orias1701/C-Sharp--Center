using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;

namespace WarehouseManagement.Repositories
{
    /// <summary>
    /// Repository để quản lý phiếu Nhập/Xuất kho (Bảng Transactions)
    /// </summary>
    public class TransactionRepository : BaseRepository
    {
        /// <summary>
        /// Lấy danh sách tất cả phiếu (bao gồm chi tiết, chỉ visible records trừ khi includeHidden=true)
        /// </summary>
        public List<Transaction> GetAllTransactions(bool includeHidden = false)
        {
            var transactions = new List<Transaction>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = includeHidden
                        ? "SELECT * FROM Transactions ORDER BY DateCreated DESC"
                        : "SELECT * FROM Transactions WHERE Visible=TRUE ORDER BY DateCreated DESC";
                    
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new Transaction
                                {
                                    TransactionID = reader.GetInt32("TransactionID"),
                                    Type = reader.GetString("Type"),
                                    DateCreated = reader.GetDateTime("DateCreated"),
                                    CreatedByUserID = reader.IsDBNull(reader.GetOrdinal("CreatedByUserID")) ? 0 : reader.GetInt32("CreatedByUserID"),
                                    SupplierID = reader.IsDBNull(reader.GetOrdinal("SupplierID")) ? (int?)null : reader.GetInt32("SupplierID"),
                                    CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? (int?)null : reader.GetInt32("CustomerID"),
                                    TotalAmount = reader.GetDecimal("TotalAmount"),
                                    Discount = reader.GetDecimal("Discount"),
                                    FinalAmount = reader.GetDecimal("FinalAmount"),
                                    Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString("Note"),
                                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "Pending" : reader.GetString("Status"),
                                    Visible = reader.GetBoolean("Visible")
                                });
                            }
                        }
                    }
                    
                    // Load chi tiết cho mỗi phiếu
                    foreach (var trans in transactions)
                    {
                        using (var detailCmd = new MySqlCommand(
                            includeHidden 
                                ? "SELECT * FROM TransactionDetails WHERE TransactionID=@transId"
                                : "SELECT * FROM TransactionDetails WHERE TransactionID=@transId AND Visible=TRUE", conn))
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
                                        UnitPrice = detailReader.GetDecimal("UnitPrice"),
                                        DiscountRate = detailReader.IsDBNull(detailReader.GetOrdinal("DiscountRate")) ? 0 : detailReader.GetDouble("DiscountRate"),
                                        SubTotal = detailReader.GetDecimal("SubTotal"),
                                        Visible = detailReader.GetBoolean("Visible")
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phiếu: " + ex.Message);
            }
            return transactions;
        }

        /// <summary>
        /// Lấy phiếu theo ID (bao gồm chi tiết)
        /// </summary>
        public Transaction GetTransactionById(int transactionId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    
                    var transaction = new Transaction { TransactionID = transactionId };
                    
                    // Lấy thông tin giao dịch
                    using (var cmd = new MySqlCommand("SELECT * FROM Transactions WHERE TransactionID=@id", conn))
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
                                transaction.SupplierID = reader.IsDBNull(reader.GetOrdinal("SupplierID")) ? (int?)null : reader.GetInt32("SupplierID");
                                transaction.CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? (int?)null : reader.GetInt32("CustomerID");
                                transaction.TotalAmount = reader.GetDecimal("TotalAmount");
                                transaction.Discount = reader.GetDecimal("Discount");
                                transaction.FinalAmount = reader.GetDecimal("FinalAmount");
                                transaction.Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString("Note");
                                transaction.Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "Pending" : reader.GetString("Status");
                                transaction.Visible = reader.GetBoolean("Visible");
                            }
                            else return null;
                        }
                    }

                    // Lấy chi tiết giao dịch
                    using (var detailCmd = new MySqlCommand(
                        "SELECT * FROM TransactionDetails WHERE TransactionID=@transId AND Visible=TRUE", conn))
                    {
                        detailCmd.Parameters.AddWithValue("@transId", transactionId);
                        using (var detailReader = detailCmd.ExecuteReader())
                        {
                            while (detailReader.Read())
                            {
                                transaction.Details.Add(new TransactionDetail
                                {
                                    DetailID = detailReader.GetInt32("DetailID"),
                                    TransactionID = detailReader.GetInt32("TransactionID"),
                                    ProductID = detailReader.GetInt32("ProductID"),
                                    ProductName = detailReader.IsDBNull(detailReader.GetOrdinal("ProductName")) ? "" : detailReader.GetString("ProductName"),
                                    Quantity = detailReader.GetInt32("Quantity"),
                                    UnitPrice = detailReader.GetDecimal("UnitPrice"),
                                    DiscountRate = detailReader.IsDBNull(detailReader.GetOrdinal("DiscountRate")) ? 0 : detailReader.GetDouble("DiscountRate"),
                                    SubTotal = detailReader.GetDecimal("SubTotal"),
                                    Visible = detailReader.GetBoolean("Visible")
                                });
                            }
                        }
                    }
                    return transaction;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phiếu ID {transactionId}: " + ex.Message);
            }
        }

        /// <summary>
        /// Tạo phiếu nhập/xuất mới
        /// </summary>
        public int CreateTransaction(Transaction transaction)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO Transactions (Type, DateCreated, CreatedByUserID, SupplierID, CustomerID, TotalAmount, Discount, FinalAmount, Note, Status, Visible) " +
                        "VALUES (@type, @date, @userId, @supId, @custId, @total, @discount, @final, @note, @status, @visible); SELECT LAST_INSERT_ID();", conn))
                    {
                        cmd.Parameters.AddWithValue("@type", transaction.Type);
                        cmd.Parameters.AddWithValue("@date", transaction.DateCreated);
                        cmd.Parameters.AddWithValue("@userId", transaction.CreatedByUserID);
                        cmd.Parameters.AddWithValue("@supId", (object)transaction.SupplierID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@custId", (object)transaction.CustomerID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@total", transaction.TotalAmount);
                        cmd.Parameters.AddWithValue("@discount", transaction.Discount);
                        cmd.Parameters.AddWithValue("@final", transaction.FinalAmount);
                        cmd.Parameters.AddWithValue("@note", transaction.Note ?? "");
                        cmd.Parameters.AddWithValue("@status", transaction.Status ?? "Pending");
                        cmd.Parameters.AddWithValue("@visible", transaction.Visible);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo phiếu: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm chi tiết vào phiếu
        /// </summary>
        public bool AddTransactionDetail(TransactionDetail detail)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice, DiscountRate, Visible) " +
                        "VALUES (@transId, @prodId, @prodName, @qty, @price, @discountRate, @visible)", conn))
                    {
                        cmd.Parameters.AddWithValue("@transId", detail.TransactionID);
                        cmd.Parameters.AddWithValue("@prodId", detail.ProductID);
                        cmd.Parameters.AddWithValue("@prodName", detail.ProductName ?? "");
                        cmd.Parameters.AddWithValue("@qty", detail.Quantity);
                        cmd.Parameters.AddWithValue("@price", detail.UnitPrice);
                        cmd.Parameters.AddWithValue("@discountRate", detail.DiscountRate);
                        cmd.Parameters.AddWithValue("@visible", detail.Visible);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm chi tiết phiếu: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật tổng giá trị của phiếu
        /// </summary>
        public bool UpdateTransactionTotal(int transactionId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    // Tính lại TotalAmount từ Details (Quantity * Price) - Đây là tổng tiền hàng chưa chiết khấu
                    // Tính Discount từ Details: Sum(Qty * Price * Rate / 100)
                    // Final = Total - Discount
                    // Tuy nhiên DB hiện tại có cột Discount ở bảng Transactions là tổng. 
                    // Logic cũ: TotalAmount = Sum(SubTotal), Final = TotalAmount - Transaction.Discount (Fixed).
                    // Logic mới: Transaction.Discount phải được cập nhật = Sum(ItemDiscount).
                    // SubTotal trong DB là generated (Qty*Price).
                    
                    // Cập nhật Discount = SUM(Quantity * UnitPrice * DiscountRate / 100)
                    // Cập nhật TotalAmount = SUM(Quantity * UnitPrice)
                    // FinalAmount = TotalAmount - Discount
                    
                    string query = @"
                        UPDATE Transactions 
                        SET TotalAmount = (SELECT COALESCE(SUM(Quantity * UnitPrice), 0) FROM TransactionDetails WHERE TransactionID = @transId AND Visible=TRUE),
                            Discount = (SELECT COALESCE(SUM(Quantity * UnitPrice * DiscountRate / 100.0), 0) FROM TransactionDetails WHERE TransactionID = @transId AND Visible=TRUE),
                            FinalAmount = (SELECT COALESCE(SUM(Quantity * UnitPrice * (1 - DiscountRate / 100.0)), 0) FROM TransactionDetails WHERE TransactionID = @transId AND Visible=TRUE)
                        WHERE TransactionID = @transId";
                        
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@transId", transactionId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật tổng giá trị phiếu: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa phiếu (Soft delete)
        /// </summary>
        public bool SoftDeleteTransaction(int transactionId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("UPDATE Transactions SET Visible=FALSE WHERE TransactionID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", transactionId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa mềm phiếu: " + ex.Message);
            }
        }
        /// <summary>
        /// Lấy danh sách phiếu theo loại (Import/Export)
        /// </summary>
        public List<Transaction> GetTransactionsByType(string type)
        {
            var transactions = new List<Transaction>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM Transactions WHERE Type=@type AND Visible=TRUE ORDER BY DateCreated DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@type", type);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new Transaction
                                {
                                    TransactionID = reader.GetInt32("TransactionID"),
                                    Type = reader.GetString("Type"),
                                    DateCreated = reader.GetDateTime("DateCreated"),
                                    CreatedByUserID = reader.IsDBNull(reader.GetOrdinal("CreatedByUserID")) ? 0 : reader.GetInt32("CreatedByUserID"),
                                    SupplierID = reader.IsDBNull(reader.GetOrdinal("SupplierID")) ? (int?)null : reader.GetInt32("SupplierID"),
                                    CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? (int?)null : reader.GetInt32("CustomerID"),
                                    TotalAmount = reader.GetDecimal("TotalAmount"),
                                    Discount = reader.GetDecimal("Discount"),
                                    FinalAmount = reader.GetDecimal("FinalAmount"),

                                    Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString("Note"),
                                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "Pending" : reader.GetString("Status"),
                                    Visible = reader.GetBoolean("Visible")
                                });
                            }
                        }
                    }
                    
                    // Note: Could load details here if needed, but for listing maybe not critical. 
                    // To follow pattern:
                    foreach(var t in transactions) {
                        t.Details = new TransactionDetailRepository().GetDetailsByTransactionId(t.TransactionID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy phiếu theo loại: " + ex.Message);
            }
            return transactions;
        }

        /// <summary>
        /// Lấy danh sách phiếu trong khoảng thời gian
        /// </summary>
        public List<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            var transactions = new List<Transaction>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT * FROM Transactions WHERE DateCreated BETWEEN @start AND @end AND Visible=TRUE ORDER BY DateCreated DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@start", startDate);
                        cmd.Parameters.AddWithValue("@end", endDate);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new Transaction
                                {
                                    TransactionID = reader.GetInt32("TransactionID"),
                                    Type = reader.GetString("Type"),
                                    DateCreated = reader.GetDateTime("DateCreated"),
                                    CreatedByUserID = reader.IsDBNull(reader.GetOrdinal("CreatedByUserID")) ? 0 : reader.GetInt32("CreatedByUserID"),
                                    SupplierID = reader.IsDBNull(reader.GetOrdinal("SupplierID")) ? (int?)null : reader.GetInt32("SupplierID"),
                                    CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? (int?)null : reader.GetInt32("CustomerID"),
                                    TotalAmount = reader.GetDecimal("TotalAmount"),
                                    Discount = reader.GetDecimal("Discount"),
                                    FinalAmount = reader.GetDecimal("FinalAmount"),
                                    Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString("Note"),
                                    Visible = reader.GetBoolean("Visible")
                                });
                            }
                        }
                    }
                    foreach(var t in transactions) {
                        t.Details = new TransactionDetailRepository().GetDetailsByTransactionId(t.TransactionID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy phiếu theo ngày: " + ex.Message);
            }
            return transactions;
        }

        public bool UpdateTransactionStatus(int transactionId, string status)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("UPDATE Transactions SET Status=@status WHERE TransactionID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@id", transactionId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật trạng thái phiếu: " + ex.Message);
            }
        }
    }
}