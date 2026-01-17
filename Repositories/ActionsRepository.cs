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
    public class ActionsRepository : BaseRepository
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
        /// Láº¥y nháº­t kÃ½ theo ID
        /// </summary>
        public Actions GetLogById(int logId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM Actions WHERE LogID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", logId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Actions
                                {
                                    LogID = reader.GetInt32("LogID"),
                                    ActionType = reader.GetString("ActionType"),
                                    Descriptions = reader.IsDBNull(reader.GetOrdinal("Descriptions")) ? "" : reader.GetString("Descriptions"),
                                    DataBefore = reader.IsDBNull(reader.GetOrdinal("DataBefore")) ? "" : reader.GetString("DataBefore"),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lá»—i khi láº¥y nháº­t kÃ½ ID {logId}: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// ThÃªm nháº­t kÃ½ hÃ nh Ä‘á»™ng má»›i
        /// </summary>
        public int LogAction(Actions log)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO Actions (ActionType, Descriptions, DataBefore, CreatedAt, Visible) " +
                        "VALUES (@type, @desc, @dataBefore, @createdAt, TRUE); SELECT LAST_INSERT_ID();", conn))
                    {
                        cmd.Parameters.AddWithValue("@type", log.ActionType);
                        cmd.Parameters.AddWithValue("@desc", log.Descriptions ?? "");
                        
                        // Xá»­ lÃ½ DataBefore - náº¿u rá»—ng hoáº·c khÃ´ng há»£p lá»‡ JSON, lÆ°u NULL hoáº·c "{}"
                        string dataBefore = log.DataBefore ?? "";
                        if (string.IsNullOrWhiteSpace(dataBefore) || dataBefore.Trim() == "")
                        {
                            dataBefore = "{}";
                        }
                        cmd.Parameters.AddWithValue("@dataBefore", dataBefore);
                        
                        cmd.Parameters.AddWithValue("@createdAt", log.CreatedAt);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi ghi nháº­t kÃ½: " + ex.Message);
            }
        }

        /// <summary>
        /// ThÃªm nháº­t kÃ½ hÃ nh Ä‘á»™ng má»›i (overload vá»›i parameters)
        /// </summary>
        public int LogAction(string actionType, string descriptions, string dataBefore = "")
        {
            var log = new Actions
            {
                ActionType = actionType,
                Descriptions = descriptions,
                DataBefore = dataBefore,
                CreatedAt = DateTime.Now
            };
            return LogAction(log);
        }

        /// <summary>
        /// XÃ³a nháº­t kÃ½ (soft delete)
        /// </summary>
        public bool DeleteLog(int logId)
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
                throw new Exception("Lá»—i khi xÃ³a nháº­t kÃ½: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y nháº­t kÃ½ theo loáº¡i hÃ nh Ä‘á»™ng
        /// </summary>
        public List<Actions> GetLogsByActionType(string actionType)
        {
            var logs = new List<Actions>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT * FROM Actions WHERE ActionType=@type AND Visible=TRUE ORDER BY CreatedAt DESC", conn))
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
                throw new Exception("Lá»—i khi láº¥y nháº­t kÃ½ theo loáº¡i: " + ex.Message);
            }
            return logs;
        }

        /// <summary>
        /// Láº¥y nháº­t kÃ½ trong má»™t khoáº£ng thá»i gian
        /// </summary>
        public List<Actions> GetLogsByDateRange(DateTime startDate, DateTime endDate)
        {
            var logs = new List<Actions>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT * FROM Actions WHERE CreatedAt BETWEEN @start AND @end AND Visible=TRUE ORDER BY CreatedAt DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@start", startDate);
                        cmd.Parameters.AddWithValue("@end", endDate);
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
                throw new Exception("Lá»—i khi láº¥y nháº­t kÃ½ theo ngÃ y: " + ex.Message);
            }
            return logs;
        }

        /// <summary>
        /// XÃ³a táº¥t cáº£ nháº­t kÃ½ (hard delete - xÃ³a hoÃ n toÃ n khá»i database)
        /// </summary>
        public bool ClearAllLogs()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "DELETE FROM Actions", conn))
                    {
                        return cmd.ExecuteNonQuery() >= 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a táº¥t cáº£ nháº­t kÃ½: " + ex.Message);
            }
        }
    }
}




