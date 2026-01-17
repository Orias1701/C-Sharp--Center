using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;

namespace WarehouseManagement.Repositories
{
    /// <summary>
    /// Repository Ä‘á»ƒ quáº£n lÃ½ nháº­t kÃ½ hÃ nh Ä‘á»™ng (há»— trá»£ Undo)
    /// </summary>
    public class LogRepository : BaseRepository
    {
        /// <summary>
        /// Láº¥y danh sÃ¡ch nháº­t kÃ½ (chá»‰ visible records)
        /// </summary>
        public List<Actions> GetAllLogs()
        {
            var logs = new List<Actions>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM Actions WHERE Visible=TRUE ORDER BY CreatedAt DESC", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new Actions
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
                throw new Exception("Lá»—i khi láº¥y danh sÃ¡ch nháº­t kÃ½: " + ex.Message);
            }
            return logs;
        }

        /// <summary>
        /// Ghi láº¡i nháº­t kÃ½ hÃ nh Ä‘á»™ng
        /// </summary>
        public bool LogAction(string actionType, string descriptions, string dataBefore = "")
        {
            if (string.IsNullOrEmpty(dataBefore)) dataBefore = "{}";
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO Actions (ActionType, Descriptions, DataBefore, Visible, CreatedAt) " +
                        "VALUES (@type, @desc, @data, TRUE, @created)", conn))
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
                throw new Exception("Lá»—i khi ghi nháº­t kÃ½: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y nháº­t kÃ½ theo loáº¡i hÃ nh Ä‘á»™ng (chá»‰ visible records)
        /// </summary>
        public List<Actions> GetLogsByActionType(string actionType)
        {
            var logs = new List<Actions>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM Actions WHERE ActionType=@type AND Visible=TRUE ORDER BY CreatedAt DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@type", actionType);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new Actions
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
                throw new Exception($"Lá»—i khi láº¥y nháº­t kÃ½ loáº¡i {actionType}: " + ex.Message);
            }
            return logs;
        }

        /// <summary>
        /// Láº¥y N hÃ nh Ä‘á»™ng gáº§n nháº¥t (LIFO stack - Last In First Out)
        /// </summary>
        public List<Actions> GetLastNLogs(int count = 10)
        {
            var logs = new List<Actions>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        $"SELECT * FROM Actions WHERE Visible=TRUE AND ActionType != 'UNDO_ACTION' ORDER BY CreatedAt DESC LIMIT {Math.Min(count, 10)}", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new Actions
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
                throw new Exception("Lá»—i khi láº¥y nháº­t kÃ½ gáº§n nháº¥t: " + ex.Message);
            }
            return logs;
        }

        /// <summary>
        /// XÃ³a má»m hÃ nh Ä‘á»™ng tá»« undo stack (set Visible=FALSE)
        /// </summary>
        public bool RemoveFromUndoStack(int logId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE Actions SET Visible=FALSE WHERE LogID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", logId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a hÃ nh Ä‘á»™ng khá»i undo stack: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a nháº­t kÃ½ cÅ© (hÆ¡n N ngÃ y)
        /// </summary>
        public bool DeleteOldLogs(int daysOld)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "DELETE FROM Actions WHERE CreatedAt < DATE_SUB(NOW(), INTERVAL @days DAY)", conn))
                    {
                        cmd.Parameters.AddWithValue("@days", daysOld);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a nháº­t kÃ½ cÅ©: " + ex.Message);
            }
        }
    }
}




