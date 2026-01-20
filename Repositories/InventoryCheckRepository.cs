using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;

namespace WarehouseManagement.Repositories
{
    public class InventoryCheckRepository : BaseRepository
    {
        public List<InventoryCheck> GetAllChecks(bool includeHidden = false)
        {
            var list = new List<InventoryCheck>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = includeHidden 
                        ? "SELECT * FROM InventoryChecks ORDER BY CheckDate DESC" 
                        : "SELECT * FROM InventoryChecks WHERE Visible = TRUE ORDER BY CheckDate DESC";
                    
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new InventoryCheck
                            {
                                CheckID = reader.GetInt32("CheckID"),
                                CheckDate = reader.GetDateTime("CheckDate"),
                                CreatedByUserID = reader.IsDBNull(reader.GetOrdinal("CreatedByUserID")) ? 0 : reader.GetInt32("CreatedByUserID"),
                                Status = reader.GetString("Status"),
                                Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString("Note"),
                                Visible = reader.GetBoolean("Visible")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phiếu kiểm kê: " + ex.Message);
            }
            return list;
        }

        public InventoryCheck GetCheckById(int id)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    var check = new InventoryCheck { CheckID = id };
                    
                    // Header
                    using (var cmd = new MySqlCommand("SELECT * FROM InventoryChecks WHERE CheckID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                check.CheckDate = reader.GetDateTime("CheckDate");
                                check.CreatedByUserID = reader.IsDBNull(reader.GetOrdinal("CreatedByUserID")) ? 0 : reader.GetInt32("CreatedByUserID");
                                check.Status = reader.GetString("Status");
                                check.Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString("Note");
                                check.Visible = reader.GetBoolean("Visible");
                            }
                            else return null;
                        }
                    }

                    // Details
                    using (var cmd = new MySqlCommand("SELECT * FROM InventoryCheckDetails WHERE CheckID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                check.Details.Add(new InventoryCheckDetail
                                {
                                    DetailID = reader.GetInt32("DetailID"),
                                    CheckID = reader.GetInt32("CheckID"),
                                    ProductID = reader.GetInt32("ProductID"),
                                    SystemQuantity = reader.GetInt32("SystemQuantity"),
                                    ActualQuantity = reader.GetInt32("ActualQuantity"),
                                    Difference = reader.GetInt32("Difference"),
                                    Reason = reader.IsDBNull(reader.GetOrdinal("Reason")) ? "" : reader.GetString("Reason")
                                });
                            }
                        }
                    }
                    return check;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phiếu kiểm kê ID {id}: " + ex.Message);
            }
        }

        public int CreateCheck(InventoryCheck check)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO InventoryChecks (CheckDate, CreatedByUserID, Status, Note, Visible) VALUES (@date, @user, @status, @note, @visible); SELECT LAST_INSERT_ID();", conn))
                    {
                        cmd.Parameters.AddWithValue("@date", check.CheckDate);
                        cmd.Parameters.AddWithValue("@user", check.CreatedByUserID);
                        cmd.Parameters.AddWithValue("@status", check.Status);
                        cmd.Parameters.AddWithValue("@note", check.Note ?? "");
                        cmd.Parameters.AddWithValue("@visible", check.Visible);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo phiếu kiểm kê: " + ex.Message);
            }
        }

        public bool AddCheckDetail(InventoryCheckDetail detail)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO InventoryCheckDetails (CheckID, ProductID, SystemQuantity, ActualQuantity, Reason) VALUES (@cid, @pid, @sys, @act, @reason)", conn))
                    {
                        cmd.Parameters.AddWithValue("@cid", detail.CheckID);
                        cmd.Parameters.AddWithValue("@pid", detail.ProductID);
                        cmd.Parameters.AddWithValue("@sys", detail.SystemQuantity);
                        cmd.Parameters.AddWithValue("@act", detail.ActualQuantity);
                        cmd.Parameters.AddWithValue("@reason", detail.Reason ?? "");
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm chi tiết kiểm kê: " + ex.Message);
            }
        }

        public bool UpdateStatus(int checkId, string status)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("UPDATE InventoryChecks SET Status=@status WHERE CheckID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@id", checkId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật trạng thái kiểm kê: " + ex.Message);
            }
        }
    }
}
