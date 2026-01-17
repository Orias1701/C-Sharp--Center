using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;

namespace WarehouseManagement.Repositories
{
    /// <summary>
    /// Repository Ä‘á»ƒ quáº£n lÃ½ sáº£n pháº©m (CRUD + cáº­p nháº­t tá»“n kho)
    /// </summary>
    public class ProductRepository : BaseRepository
    {
        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ sáº£n pháº©m (chá»‰ nhá»¯ng sáº£n pháº©m Visible=true)
        /// </summary>
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM Products WHERE Visible=TRUE ORDER BY ProductID DESC", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductID = reader.GetInt32("ProductID"),
                                    ProductName = reader.GetString("ProductName"),
                                    CategoryID = reader.GetInt32("CategoryID"),
                                    Price = reader.GetDecimal("Price"),
                                    Quantity = reader.GetInt32("Quantity"),
                                    MinThreshold = reader.GetInt32("MinThreshold")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh sÃ¡ch sáº£n pháº©m: " + ex.Message);
            }
            return products;
        }

        /// <summary>
        /// Láº¥y sáº£n pháº©m theo ID
        /// </summary>
        public Product GetProductById(int productId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM Products WHERE ProductID = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Product
                                {
                                    ProductID = reader.GetInt32("ProductID"),
                                    ProductName = reader.GetString("ProductName"),
                                    CategoryID = reader.GetInt32("CategoryID"),
                                    Price = reader.GetDecimal("Price"),
                                    Quantity = reader.GetInt32("Quantity"),
                                    MinThreshold = reader.GetInt32("MinThreshold")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lá»—i khi láº¥y sáº£n pháº©m ID {productId}: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// ThÃªm sáº£n pháº©m má»›i
        /// </summary>
        public int AddProduct(Product product)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO Products (ProductName, CategoryID, Price, Quantity, InventoryValue, MinThreshold) " +
                        "VALUES (@name, @catId, @price, @qty, @invValue, @threshold); SELECT LAST_INSERT_ID();", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", product.ProductName);
                        cmd.Parameters.AddWithValue("@catId", product.CategoryID);
                        cmd.Parameters.AddWithValue("@price", product.Price);
                        cmd.Parameters.AddWithValue("@qty", product.Quantity);
                        cmd.Parameters.AddWithValue("@invValue", product.Quantity * product.Price);
                        cmd.Parameters.AddWithValue("@threshold", product.MinThreshold);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi thÃªm sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t thÃ´ng tin sáº£n pháº©m
        /// </summary>
        public bool UpdateProduct(Product product)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE Products SET ProductName=@name, CategoryID=@catId, Price=@price, " +
                        "Quantity=@qty, InventoryValue=@invValue, MinThreshold=@threshold WHERE ProductID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", product.ProductName);
                        cmd.Parameters.AddWithValue("@catId", product.CategoryID);
                        cmd.Parameters.AddWithValue("@price", product.Price);
                        cmd.Parameters.AddWithValue("@qty", product.Quantity);
                        cmd.Parameters.AddWithValue("@invValue", product.Quantity * product.Price);
                        cmd.Parameters.AddWithValue("@threshold", product.MinThreshold);
                        cmd.Parameters.AddWithValue("@id", product.ProductID);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi cáº­p nháº­t sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a sáº£n pháº©m - kiá»ƒm tra phá»¥ thuá»™c khÃ³a vÃ  xÃ³a má»m hoáº·c váº­t lÃ½
        /// </summary>
        public bool DeleteProduct(int productId)
        {
            try
            {
                // Kiá»ƒm tra phá»¥ thuá»™c khÃ³a
                if (HasForeignKeyReferences(productId))
                {
                    // CÃ³ phá»¥ thuá»™c - xÃ³a má»m (soft delete)
                    return SoftDeleteProduct(productId);
                }
                else
                {
                    // KhÃ´ng cÃ³ phá»¥ thuá»™c - xÃ³a váº­t lÃ½ (hard delete)
                    return HardDeleteProduct(productId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t tá»“n kho
        /// </summary>
        public bool UpdateQuantity(int productId, int newQuantity)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    // First get the price to calculate InventoryValue
                    using (var cmd = new MySqlCommand("SELECT Price FROM Products WHERE ProductID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productId);
                        var price = cmd.ExecuteScalar();
                        if (price == null || price == DBNull.Value)
                            throw new Exception($"Sáº£n pháº©m ID {productId} khÃ´ng tá»“n táº¡i");

                        decimal productPrice = Convert.ToDecimal(price);
                        decimal inventoryValue = newQuantity * productPrice;

                        using (var updateCmd = new MySqlCommand("UPDATE Products SET Quantity=@qty, InventoryValue=@invValue WHERE ProductID=@id", conn))
                        {
                            updateCmd.Parameters.AddWithValue("@qty", newQuantity);
                            updateCmd.Parameters.AddWithValue("@invValue", inventoryValue);
                            updateCmd.Parameters.AddWithValue("@id", productId);
                            return updateCmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi cáº­p nháº­t tá»“n kho: " + ex.Message);
            }
        }



        /// <summary>
        /// Kiá»ƒm tra xem sáº£n pháº©m vá»›i ID Ä‘Ã£ tá»“n táº¡i hay chÆ°a
        /// </summary>
        public bool ProductIdExists(int productId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM Products WHERE ProductID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi kiá»ƒm tra tá»“n táº¡i sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiá»ƒm tra sáº£n pháº©m cÃ³ Ä‘Æ°á»£c tham chiáº¿u bá»Ÿi TransactionDetails hoáº·c cÃ¡c báº£ng khÃ¡c hay khÃ´ng
        /// </summary>
        public bool HasForeignKeyReferences(int productId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    // Kiá»ƒm tra TransactionDetails
                    using (var cmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM TransactionDetails WHERE ProductID=@id AND Visible=TRUE", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i kiá»ƒm tra phá»¥ thuá»™c khÃ³a ngoÃ i: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a má»m (soft delete) - Ä‘áº·t Visible = false
        /// </summary>
        public bool SoftDeleteProduct(int productId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE Products SET Visible=FALSE WHERE ProductID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a má»m sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a váº­t lÃ½ (hard delete) - xÃ³a toÃ n bá»™ báº£n ghi
        /// </summary>
        public bool HardDeleteProduct(int productId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "DELETE FROM Products WHERE ProductID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a sáº£n pháº©m: " + ex.Message);
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
                        "SELECT COUNT(*) FROM Products WHERE CategoryID=@id AND Visible=TRUE", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", categoryId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i kiá»ƒm tra danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a má»m danh má»¥c (Visible = false)
        /// </summary>
        public bool SoftDeleteCategory(int categoryId)
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
                throw new Exception("Lá»—i xÃ³a má»m danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a váº­t lÃ½ danh má»¥c
        /// </summary>
        public bool HardDeleteCategory(int categoryId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "DELETE FROM Categories WHERE CategoryID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", categoryId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i xÃ³a váº­t lÃ½ danh má»¥c: " + ex.Message);
            }
        }
    }
}





