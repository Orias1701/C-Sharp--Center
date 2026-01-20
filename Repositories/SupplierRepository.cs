using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;

namespace WarehouseManagement.Repositories
{
    public class SupplierRepository : BaseRepository
    {
        public List<Supplier> GetAllSuppliers(bool includeHidden = false)
        {
            var list = new List<Supplier>();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = includeHidden 
                        ? "SELECT * FROM Suppliers" 
                        : "SELECT * FROM Suppliers WHERE Visible = TRUE";
                    
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Supplier
                            {
                                SupplierID = reader.GetInt32("SupplierID"),
                                SupplierName = reader.GetString("SupplierName"),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString("Phone"),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString("Address"),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                                Visible = reader.GetBoolean("Visible")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách nhà cung cấp: " + ex.Message);
            }
            return list;
        }

        public Supplier GetSupplierById(int id)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM Suppliers WHERE SupplierID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Supplier
                                {
                                    SupplierID = reader.GetInt32("SupplierID"),
                                    SupplierName = reader.GetString("SupplierName"),
                                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString("Phone"),
                                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString("Address"),
                                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                                    Visible = reader.GetBoolean("Visible")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy nhà cung cấp ID {id}: " + ex.Message);
            }
            return null;
        }

        public int AddSupplier(Supplier supplier)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO Suppliers (SupplierName, Phone, Address, Email, Visible) VALUES (@name, @phone, @address, @email, @visible); SELECT LAST_INSERT_ID();", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", supplier.SupplierName);
                        cmd.Parameters.AddWithValue("@phone", supplier.Phone);
                        cmd.Parameters.AddWithValue("@address", supplier.Address);
                        cmd.Parameters.AddWithValue("@email", supplier.Email);
                        cmd.Parameters.AddWithValue("@visible", supplier.Visible);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm nhà cung cấp: " + ex.Message);
            }
        }

        public bool UpdateSupplier(Supplier supplier)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE Suppliers SET SupplierName=@name, Phone=@phone, Address=@address, Email=@email, Visible=@visible WHERE SupplierID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", supplier.SupplierName);
                        cmd.Parameters.AddWithValue("@phone", supplier.Phone);
                        cmd.Parameters.AddWithValue("@address", supplier.Address);
                        cmd.Parameters.AddWithValue("@email", supplier.Email);
                        cmd.Parameters.AddWithValue("@visible", supplier.Visible);
                        cmd.Parameters.AddWithValue("@id", supplier.SupplierID);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật nhà cung cấp: " + ex.Message);
            }
        }

        public bool SoftDeleteSupplier(int id)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("UPDATE Suppliers SET Visible=FALSE WHERE SupplierID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xóa mềm nhà cung cấp: " + ex.Message);
            }
        }
    }
}
