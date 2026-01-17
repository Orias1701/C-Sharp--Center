using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xá»­ lÃ½ logic nháº­t kÃ½ hÃ nh Ä‘á»™ng (há»— trá»£ Undo) + Quáº£n lÃ½ Save/Commit
    /// 
    /// CHá»¨C NÄ‚NG:
    /// - Quáº£n lÃ½ nháº­t kÃ½ (CRUD): ThÃªm, xem, xÃ³a
    /// - TÃ¬m kiáº¿m nháº­t kÃ½: Theo loáº¡i hÃ nh Ä‘á»™ng, ngÃ y thÃ¡ng
    /// - Undo: KhÃ´i phá»¥c dá»¯ liá»‡u trÆ°á»›c khi thay Ä‘á»•i
    /// - Save state tracking: ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i chÆ°a lÆ°u
    /// 
    /// LUá»’NG:
    /// 1. Validation: Kiá»ƒm tra Ä‘áº§u vÃ o
    /// 2. Repository call: Gá»i DB Ä‘á»ƒ thá»±c hiá»‡n thao tÃ¡c
    /// 3. Return: Tráº£ vá» káº¿t quáº£
    /// </summary>
    public class ActionsService
    {
        private readonly ActionsRepository _logRepo;
        
        // Save state tracking (merged from SaveManager)
        private bool _hasUnsavedChanges = false;
        private DateTime _lastSaveTime = DateTime.Now;
        private int _changeCount = 0;

        // Singleton pattern
        private static ActionsService _instance;

        public static ActionsService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ActionsService();
                return _instance;
            }
        }

        private ActionsService()
        {
            _logRepo = new ActionsRepository();
            _lastSaveTime = DateTime.Now;
        }

        #region Action Logging Methods

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ nháº­t kÃ½
        /// </summary>
        public List<Actions> GetAllLogs()
        {
            try
            {
                return _logRepo.GetAllLogs();
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh sÃ¡ch nháº­t kÃ½: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y nháº­t kÃ½ theo ID
        /// </summary>
        public Actions GetLogById(int logId)
        {
            try
            {
                if (logId <= 0)
                    throw new ArgumentException("ID nháº­t kÃ½ khÃ´ng há»£p lá»‡");
                return _logRepo.GetLogById(logId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y nháº­t kÃ½: " + ex.Message);
            }
        }

        /// <summary>
        /// Ghi nháº­t kÃ½ hÃ nh Ä‘á»™ng má»›i
        /// </summary>
        public int LogAction(string actionType, string descriptions, string dataBefore = "")
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(actionType))
                    throw new ArgumentException("Loáº¡i hÃ nh Ä‘á»™ng khÃ´ng Ä‘Æ°á»£c trá»‘ng");

                var log = new Actions
                {
                    ActionType = actionType.Trim(),
                    Descriptions = descriptions ?? "",
                    DataBefore = dataBefore ?? "",
                    CreatedAt = DateTime.Now
                };

                return _logRepo.LogAction(log);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi ghi nháº­t kÃ½: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a nháº­t kÃ½ (soft delete)
        /// </summary>
        public bool DeleteLog(int logId)
        {
            try
            {
                if (logId <= 0)
                    throw new ArgumentException("ID nháº­t kÃ½ khÃ´ng há»£p lá»‡");

                return _logRepo.DeleteLog(logId);
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
            try
            {
                if (string.IsNullOrWhiteSpace(actionType))
                    throw new ArgumentException("Loáº¡i hÃ nh Ä‘á»™ng khÃ´ng Ä‘Æ°á»£c trá»‘ng");

                return _logRepo.GetLogsByActionType(actionType.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y nháº­t kÃ½ theo loáº¡i: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y nháº­t kÃ½ trong má»™t khoáº£ng thá»i gian
        /// </summary>
        public List<Actions> GetLogsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    throw new ArgumentException("NgÃ y báº¯t Ä‘áº§u khÃ´ng Ä‘Æ°á»£c lá»›n hÆ¡n ngÃ y káº¿t thÃºc");

                return _logRepo.GetLogsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y nháº­t kÃ½ theo ngÃ y: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y nháº­t kÃ½ gáº§n nháº¥t cá»§a má»™t loáº¡i hÃ nh Ä‘á»™ng
        /// </summary>
        public Actions GetLatestLog(string actionType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionType))
                    throw new ArgumentException("Loáº¡i hÃ nh Ä‘á»™ng khÃ´ng Ä‘Æ°á»£c trá»‘ng");

                var logs = _logRepo.GetLogsByActionType(actionType.Trim());
                if (logs.Count > 0)
                    return logs[0]; // Má»›i nháº¥t Ä‘Æ°á»£c sort first
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y nháº­t kÃ½ gáº§n nháº¥t: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiá»ƒm tra cÃ³ nháº­t kÃ½ nÃ o khÃ´ng
        /// </summary>
        public bool HasLogs()
        {
            try
            {
                return _logRepo.GetAllLogs().Count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi kiá»ƒm tra nháº­t kÃ½: " + ex.Message);
            }
        }

        /// <summary>
        /// Äáº¿m tá»•ng sá»‘ nháº­t kÃ½
        /// </summary>
        public int CountLogs()
        {
            try
            {
                return _logRepo.GetAllLogs().Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi Ä‘áº¿m nháº­t kÃ½: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a táº¥t cáº£ nháº­t kÃ½ khi káº¿t thÃºc phiÃªn
        /// </summary>
        public bool ClearAllLogs()
        {
            try
            {
                return _logRepo.ClearAllLogs();
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a táº¥t cáº£ nháº­t kÃ½: " + ex.Message);
            }
        }

        #endregion

        #region Save State Management (merged from SaveManager)

        /// <summary>
        /// ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i chÆ°a lÆ°u
        /// ÄÆ°á»£c gá»i tá»« cÃ¡c Service methods (AddProduct, ImportStock, v.v...)
        /// </summary>
        public void MarkAsChanged()
        {
            _hasUnsavedChanges = true;
            _changeCount++;
        }

        /// <summary>
        /// Giáº£m sá»‘ lÆ°á»£ng thay Ä‘á»•i khi hoÃ n tÃ¡c hÃ nh Ä‘á»™ng
        /// ÄÆ°á»£c gá»i tá»« Undo functionality
        /// </summary>
        public void DecrementChangeCount()
        {
            if (_changeCount > 0)
            {
                _changeCount--;
            }
            
            // Náº¿u khÃ´ng cÃ²n thay Ä‘á»•i nÃ o, reset tráº¡ng thÃ¡i
            if (_changeCount == 0)
            {
                _hasUnsavedChanges = false;
            }
        }

        /// <summary>
        /// Kiá»ƒm tra cÃ³ thay Ä‘á»•i chÆ°a lÆ°u hay khÃ´ng
        /// </summary>
        public bool HasUnsavedChanges => _hasUnsavedChanges;

        /// <summary>
        /// Láº¥y sá»‘ lÆ°á»£ng thay Ä‘á»•i tá»« láº§n save cuá»‘i cÃ¹ng
        /// </summary>
        public int ChangeCount => _changeCount;

        /// <summary>
        /// Láº¥y thá»i gian Save cuá»‘i cÃ¹ng
        /// </summary>
        public DateTime LastSaveTime => _lastSaveTime;

        /// <summary>
        /// LÆ°u cÃ¡c thay Ä‘á»•i vÃ o database (CommitChanges)
        /// 
        /// LUá»’NG:
        /// 1. Táº¥t cáº£ thay Ä‘á»•i Ä‘Ã£ Ä‘Æ°á»£c thá»±c hiá»‡n qua cÃ¡c Service methods
        /// 2. ÄÃ£ Ä‘Æ°á»£c ghi vÃ o Actions vá»›i CreatedAt = now
        /// 3. Chá»‰ cáº§n update láº¡i _lastSaveTime
        /// 4. Reset tráº¡ng thÃ¡i HasUnsavedChanges vÃ  ChangeCount
        /// 
        /// ÄÆ°á»£c gá»i khi:
        /// - User click nÃºt "LÆ°u" (ðŸ’¾)
        /// - User chá»n "CÃ³" (Yes) khi thoÃ¡t app
        /// </summary>
        public void CommitChanges()
        {
            try
            {
                // Cáº­p nháº­t thá»i gian save cuá»‘i cÃ¹ng
                // Táº¥t cáº£ thay Ä‘á»•i tá»« láº§n save trÆ°á»›c Ä‘áº¿n now Ä‘á»u Ä‘Ã£ Ä‘Æ°á»£c lÆ°u
                _lastSaveTime = DateTime.Now;
                
                // Reset tráº¡ng thÃ¡i
                _hasUnsavedChanges = false;
                _changeCount = 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi lÆ°u thay Ä‘á»•i: " + ex.Message);
            }
        }

        /// <summary>
        /// KhÃ´i phá»¥c táº¥t cáº£ thay Ä‘á»•i tá»« láº§n save cuá»‘i cÃ¹ng
        /// 
        /// LUá»’NG:
        /// 1. Truy váº¥n Actions
        /// 2. TÃ¬m táº¥t cáº£ hÃ nh Ä‘á»™ng tá»« _lastSaveTime trá»Ÿ Ä‘i (CreatedAt >= _lastSaveTime)
        /// 3. Set Visible=FALSE Ä‘á»ƒ "áº©n" nhá»¯ng hÃ nh Ä‘á»™ng Ä‘Ã³
        /// 4. KhÃ´ng xÃ³a váº­t lÃ½, chá»‰ áº©n Ä‘á»ƒ giá»¯ nguyÃªn tÃ­nh lá»‹ch sá»­
        /// 
        /// ÄÆ°á»£c gá»i khi:
        /// - User chá»n "KhÃ´ng" (No) khi thoÃ¡t app
        /// - System cáº§n revert cÃ¡c thay Ä‘á»•i chÆ°a lÆ°u
        /// </summary>
        public void RollbackChanges()
        {
            try
            {
                // Láº¥y connection string tá»« App.config
                string connString = ConfigurationManager.ConnectionStrings["WarehouseDB"].ConnectionString;

                using (var conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    
                    // XÃ³a (áº©n) táº¥t cáº£ hÃ nh Ä‘á»™ng tá»« láº§n save cuá»‘i
                    // Loáº¡i trá»« hÃ nh Ä‘á»™ng Undo Ä‘á»ƒ khÃ´ng áº£nh hÆ°á»Ÿng Ä‘áº¿n undo stack
                    using (var cmd = new MySqlCommand(
                        "UPDATE Actions SET Visible=FALSE " +
                        "WHERE CreatedAt >= @lastSaveTime AND ActionType != 'UNDO_ACTION'", 
                        conn))
                    {
                        cmd.Parameters.AddWithValue("@lastSaveTime", _lastSaveTime);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Reset tráº¡ng thÃ¡i
                _hasUnsavedChanges = false;
                _changeCount = 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi khÃ´i phá»¥c thay Ä‘á»•i: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a toÃ n bá»™ undo stack
        /// 
        /// LUá»’NG:
        /// 1. XÃ³a táº¥t cáº£ hÃ nh Ä‘á»™ng trong LIFO undo stack
        /// 2. Set Visible=FALSE cho táº¥t cáº£ Actions (trá»« UNDO_ACTION)
        /// 3. App sáº½ khá»Ÿi Ä‘á»™ng láº¡i vá»›i tráº¡ng thÃ¡i sáº¡ch sáº½
        /// 
        /// ÄÆ°á»£c gá»i khi:
        /// - App sáº¯p Ä‘Ã³ng (sau CommitChanges hoáº·c RollbackChanges)
        /// - Reset tráº¡ng thÃ¡i toÃ n bá»™
        /// </summary>
        public void ClearUndoStack()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["WarehouseDB"].ConnectionString;

                using (var conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    
                    // XÃ³a (áº©n) táº¥t cáº£ undo stack entry
                    using (var cmd = new MySqlCommand(
                        "UPDATE Actions SET Visible=FALSE WHERE ActionType != 'UNDO_ACTION'", 
                        conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a undo stack: " + ex.Message);
            }
        }

        /// <summary>
        /// Reset tráº¡ng thÃ¡i ActionsService
        /// Sá»­ dá»¥ng khi app khá»Ÿi Ä‘á»™ng láº¡i hoáº·c cáº§n reset toÃ n bá»™
        /// </summary>
        public void Reset()
        {
            _hasUnsavedChanges = false;
            _changeCount = 0;
            _lastSaveTime = DateTime.Now;
        }

        #endregion
    }
}


