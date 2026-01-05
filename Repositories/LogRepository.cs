using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;

namespace WarehouseManagement.Repositories
{
    /// <summary>
    /// Repository để quản lý nhật ký hành động (hỗ trợ Undo)
    /// </summary>
    public class LogRepository : BaseRepository
    {
        /// <summary>
        /// Lấy danh sách nhật ký
        /// </summary>
        public List<ActionLog> GetAllLogs()
        {
            var logs = new List<ActionLog>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM ActionLogs ORDER BY CreatedAt DESC", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new ActionLog
                                {
                                    LogID = reader.GetInt32("LogID"),
                                    ActionType = reader.GetString("ActionType"),
                                    Descriptions = reader.IsDBNull(reader.GetOrdinal("Descriptions")) ? "" : reader.GetString("Descriptions"),
                                    DataBefore = reader.IsDBNull(reader.GetOrdinal("DataBefore")) ? "" : reader.GetString("DataBefore"),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách nhật ký: " + ex.Message);
            }
            return logs;
        }

        /// <summary>
        /// Ghi lại nhật ký hành động
        /// </summary>
        public bool LogAction(string actionType, string descriptions, string dataBefore = "")
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO ActionLogs (ActionType, Descriptions, DataBefore, CreatedAt) " +
                        "VALUES (@type, @desc, @data, @created)", conn))
                    {
                        cmd.Parameters.AddWithValue("@type", actionType);
                        cmd.Parameters.AddWithValue("@desc", descriptions);
                        cmd.Parameters.AddWithValue("@data", dataBefore);
                        cmd.Parameters.AddWithValue("@created", DateTime.Now);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi ghi nhật ký: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy nhật ký theo loại hành động
        /// </summary>
        public List<ActionLog> GetLogsByActionType(string actionType)
        {
            var logs = new List<ActionLog>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM ActionLogs WHERE ActionType=@type ORDER BY CreatedAt DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@type", actionType);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new ActionLog
                                {
                                    LogID = reader.GetInt32("LogID"),
                                    ActionType = reader.GetString("ActionType"),
                                    Descriptions = reader.IsDBNull(reader.GetOrdinal("Descriptions")) ? "" : reader.GetString("Descriptions"),
                                    DataBefore = reader.IsDBNull(reader.GetOrdinal("DataBefore")) ? "" : reader.GetString("DataBefore"),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy nhật ký loại {actionType}: " + ex.Message);
            }
            return logs;
        }

        /// <summary>
        /// Xóa nhật ký cũ (hơn N ngày)
        /// </summary>
        public bool DeleteOldLogs(int daysOld)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "DELETE FROM ActionLogs WHERE CreatedAt < DATE_SUB(NOW(), INTERVAL @days DAY)", conn))
                    {
                        cmd.Parameters.AddWithValue("@days", daysOld);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa nhật ký cũ: " + ex.Message);
            }
        }
    }
}
