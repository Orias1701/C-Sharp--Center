using System;
using System.Collections.Generic;
using WarehouseManagement.Repositories;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Quản lý trạng thái Save/Commit của ứng dụng
    /// Theo dõi thay đổi từ lần Save cuối cùng
    /// </summary>
    public class SaveManager
    {
        private bool _hasUnsavedChanges = false;
        private DateTime _lastSaveTime = DateTime.Now;
        private int _changeCount = 0;
        private readonly LogRepository _logRepo;

        // Singleton pattern
        private static SaveManager _instance;

        public static SaveManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SaveManager();
                return _instance;
            }
        }

        private SaveManager()
        {
            _logRepo = new LogRepository();
            _lastSaveTime = DateTime.Now;
        }

        /// <summary>
        /// Đánh dấu có thay đổi chưa lưu
        /// </summary>
        public void MarkAsChanged()
        {
            _hasUnsavedChanges = true;
            _changeCount++;
        }

        /// <summary>
        /// Kiểm tra có thay đổi chưa lưu hay không
        /// </summary>
        public bool HasUnsavedChanges => _hasUnsavedChanges;

        /// <summary>
        /// Lấy số lượng thay đổi từ lần save cuối
        /// </summary>
        public int ChangeCount => _changeCount;

        /// <summary>
        /// Lấy thời gian Save cuối cùng
        /// </summary>
        public DateTime LastSaveTime => _lastSaveTime;

        /// <summary>
        /// Lưu các thay đổi vào database và đặt lại trạng thái
        /// Hàm này được gọi khi user click Save
        /// </summary>
        public void CommitChanges()
        {
            try
            {
                // Cập nhật database (database context tự động xử lý)
                // Tất cả thay đổi đã được thực hiện qua các service methods

                _hasUnsavedChanges = false;
                _changeCount = 0;
                _lastSaveTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lưu thay đổi: " + ex.Message);
            }
        }

        /// <summary>
        /// Khôi phục tất cả thay đổi từ lần save cuối
        /// Xóa toàn bộ undo stack từ lần save cuối
        /// </summary>
        public void RollbackChanges()
        {
            try
            {
                // Xóa tất cả hành động từ lần save cuối (set Visible=FALSE)
                // Lấy tất cả log từ _lastSaveTime trở đi
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(
                    System.Configuration.ConfigurationManager.ConnectionStrings["WarehouseDB"].ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                        "UPDATE ActionLogs SET Visible=FALSE WHERE CreatedAt >= @lastSaveTime AND ActionType != 'UNDO_ACTION'", conn))
                    {
                        cmd.Parameters.AddWithValue("@lastSaveTime", _lastSaveTime);
                        cmd.ExecuteNonQuery();
                    }
                }

                _hasUnsavedChanges = false;
                _changeCount = 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi khôi phục thay đổi: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa toàn bộ undo stack
        /// Được gọi khi app đóng
        /// </summary>
        public void ClearUndoStack()
        {
            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(
                    System.Configuration.ConfigurationManager.ConnectionStrings["WarehouseDB"].ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                        "UPDATE ActionLogs SET Visible=FALSE WHERE ActionType != 'UNDO_ACTION'", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa undo stack: " + ex.Message);
            }
        }

        /// <summary>
        /// Reset trạng thái (dùng khi app khởi động)
        /// </summary>
        public void Reset()
        {
            _hasUnsavedChanges = false;
            _changeCount = 0;
            _lastSaveTime = DateTime.Now;
        }
    }
}
