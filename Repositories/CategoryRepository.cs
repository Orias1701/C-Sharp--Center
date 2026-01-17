using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;

namespace WarehouseManagement.Repositories
{
    /// <summary>
    /// Repository Ä‘á»ƒ quáº£n lÃ½ danh má»¥c sáº£n pháº©m
    /// </summary>
    public class CategoryRepository : BaseRepository
    {
        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ danh má»¥c (chá»‰ visible records)
        /// </summary>
        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM Categories WHERE Visible=TRUE ORDER BY CategoryID DESC", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(new Category
                                {
                                    CategoryID = reader.GetInt32("CategoryID"),
                                    CategoryName = reader.GetString("CategoryName")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh sÃ¡ch danh má»¥c: " + ex.Message);
            }
            return categories;
        }

        /// <summary>
        /// Láº¥y danh má»¥c theo ID
        /// </summary>
        public Category GetCategoryById(int categoryId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM Categories WHERE CategoryID = @id AND Visible=TRUE", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", categoryId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Category
                                {
                                    CategoryID = reader.GetInt32("CategoryID"),
                                    CategoryName = reader.GetString("CategoryName")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lá»—i khi láº¥y danh má»¥c ID {categoryId}: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// ThÃªm danh má»¥c má»›i
        /// </summary>
        public int AddCategory(Category category)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO Categories (CategoryName, Visible) " +
                        "VALUES (@name, TRUE); SELECT LAST_INSERT_ID();", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", category.CategoryName);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi thÃªm danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t danh má»¥c
        /// </summary>
        public bool UpdateCategory(Category category)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE Categories SET CategoryName=@name WHERE CategoryID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", category.CategoryName);
                        cmd.Parameters.AddWithValue("@id", category.CategoryID);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi cáº­p nháº­t danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a danh má»¥c (soft delete)
        /// </summary>
        public bool DeleteCategory(int categoryId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE Categories SET Visible=FALSE WHERE CategoryID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", categoryId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// Phá»¥c há»“i danh má»¥c Ä‘Ã£ xÃ³a (restore deleted category)
        /// </summary>
        public bool RestoreDeletedCategory(int categoryId, string categoryName)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE Categories SET Visible=TRUE, CategoryName=@name WHERE CategoryID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", categoryId);
                        cmd.Parameters.AddWithValue("@name", categoryName);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi phá»¥c há»“i danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiá»ƒm tra danh má»¥c cÃ³ sáº£n pháº©m hay khÃ´ng
        /// </summary>
        public bool CategoryHasProducts(int categoryId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM Products WHERE CategoryID=@catId AND Visible=TRUE", conn))
                    {
                        cmd.Parameters.AddWithValue("@catId", categoryId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi kiá»ƒm tra danh má»¥c: " + ex.Message);
            }
        }
    }
}




