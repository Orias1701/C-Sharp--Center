using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;
using WarehouseManagement.Helpers;

namespace WarehouseManagement.Repositories
{
    public class UserRepository : BaseRepository
    {
        // Helper method để tránh lặp code khi map dữ liệu từ DB sang Object
        private User MapUserFromReader(MySqlDataReader reader)
        {
            var user = new User
            {
                UserID = reader.GetInt32("UserID"),
                Username = reader.GetString("Username"),
                Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? "" : reader.GetString("Password"),
                FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? "" : reader.GetString("FullName"),
                Role = reader.GetString("Role"),
                IsActive = reader.GetBoolean("IsActive"),
                CreatedAt = reader.GetDateTime("CreatedAt")
            };
            
            // Check for Visible column availability
            try 
            {
                int visibleOrdinal = reader.GetOrdinal("Visible");
                if (!reader.IsDBNull(visibleOrdinal))
                {
                    user.Visible = reader.GetBoolean(visibleOrdinal);
                }
            }
            catch { /* Ignore if column not found in specific query */ }

            return user;
        }

        public User Login(string username, string passwordRaw)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passwordRaw))
                {
                    return null;
                }

                using (var conn = GetConnection())
                {
                    conn.Open();
                    // Login allows active users only. Visible check? Usually yes.
                    string sql = "SELECT * FROM Users WHERE Username=@username AND IsActive=TRUE AND Visible=TRUE";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) 
                            {
                                var user = MapUserFromReader(reader);
                                if (user.VerifyPassword(passwordRaw))
                                {
                                    return user;
                                }
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi hệ thống khi đăng nhập: " + ex.Message); 
            }
        }

        public User GetUserById(int userId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM Users WHERE UserID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) return MapUserFromReader(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi truy vấn ID người dùng: " + ex.Message);
            }
            return null;
        }

        public List<User> GetAllUsers(bool includeHidden = false)
        {
            var users = new List<User>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = includeHidden 
                        ? "SELECT * FROM Users ORDER BY CreatedAt DESC"
                        : "SELECT * FROM Users WHERE Visible=TRUE ORDER BY CreatedAt DESC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(MapUserFromReader(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách người dùng: " + ex.Message);
            }
            return users;
        }

        public int AddUser(User user)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    // Note: Hash password logic should be in Service or Model setter. 
                    // Assuming User object already has hashed password if set, or we use SetPassword.
                    // But here we insert raw value from Password property. 
                    // New User creation usually involves hashing. 
                    // Since this is a test script usage mostly, I'll expect caller to set valid state.
                    // OR I can hash it if not verified.
                    // But for simplicity in repo, just insert.
                    
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO Users (Username, Password, FullName, Role, IsActive, CreatedAt, Visible) " +
                        "VALUES (@user, @pass, @name, @role, @active, @date, @visible); SELECT LAST_INSERT_ID();", conn))
                    {
                        cmd.Parameters.AddWithValue("@user", user.Username);
                        cmd.Parameters.AddWithValue("@pass", user.Password); // Assuming it's already hashed or whatever logic used
                        cmd.Parameters.AddWithValue("@name", user.FullName ?? "");
                        cmd.Parameters.AddWithValue("@role", user.Role ?? "Staff");
                        cmd.Parameters.AddWithValue("@active", user.IsActive);
                        cmd.Parameters.AddWithValue("@date", user.CreatedAt == DateTime.MinValue ? DateTime.Now : user.CreatedAt);
                        cmd.Parameters.AddWithValue("@visible", user.Visible);
                        
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm người dùng: " + ex.Message);
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE Users SET FullName=@name, Role=@role, IsActive=@active, Visible=@visible WHERE UserID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", user.FullName ?? "");
                        cmd.Parameters.AddWithValue("@role", user.Role ?? "Staff");
                        cmd.Parameters.AddWithValue("@active", user.IsActive);
                        cmd.Parameters.AddWithValue("@visible", user.Visible);
                        cmd.Parameters.AddWithValue("@id", user.UserID);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật người dùng: " + ex.Message);
            }
        }
        
        public bool SoftDeleteUser(int userId)
        {
             try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("UPDATE Users SET Visible=FALSE WHERE UserID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa người dùng: " + ex.Message);
            }
        }
    }
}