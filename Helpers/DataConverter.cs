using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

namespace WarehouseManagement.Helpers
{
    /// <summary>
    /// Helper hỗ trợ chuyển đổi dữ liệu
    /// </summary>
    public static class DataConverter
    {
        /// <summary>
        /// Chuyển đổi DataTable thành List<T>
        /// </summary>
        public static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        /// <summary>
        /// Chuyển đổi DataRow thành Object T
        /// </summary>
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        // Kiểm tra nếu giá trị là DBNull thì bỏ qua hoặc set default
                        if (dr[column.ColumnName] != DBNull.Value)
                        {
                            try 
                            {
                                // Xử lý chuyển đổi kiểu dữ liệu nếu cần
                                object value = dr[column.ColumnName];
                                Type propType = Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType;
                                
                                object safeValue = (value == null) ? null : Convert.ChangeType(value, propType);
                                pro.SetValue(obj, safeValue, null);
                            }
                            catch
                            {
                                // Bỏ qua lỗi mapping nếu không tương thích kiểu
                            }
                        }
                        else
                        {
                            pro.SetValue(obj, null, null);
                        }
                    }
                }
            }
            return obj;
        }
    }
}