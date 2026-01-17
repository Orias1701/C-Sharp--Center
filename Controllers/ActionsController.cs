using System;
using System.Collections.Generic;
using WarehouseManagement.Services;
using WarehouseManagement.Models;

namespace WarehouseManagement.Controllers
{
    /// <summary>
    /// Controller Ä‘iá»u hÆ°á»›ng cÃ¡c thao tÃ¡c liÃªn quan Ä‘áº¿n nháº­t kÃ½ hÃ nh Ä‘á»™ng
    /// </summary>
    public class ActionsController
    {
        private readonly ActionsService _logService;

        public ActionsController()
        {
            _logService = ActionsService.Instance;
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ nháº­t kÃ½
        /// </summary>
        public List<Actions> GetAllLogs()
        {
            return _logService.GetAllLogs();
        }

        /// <summary>
        /// Láº¥y nháº­t kÃ½ theo ID
        /// </summary>
        public Actions GetLogById(int logId)
        {
            return _logService.GetLogById(logId);
        }

        /// <summary>
        /// Ghi nháº­t kÃ½ hÃ nh Ä‘á»™ng má»›i
        /// </summary>
        public int LogAction(string actionType, string descriptions, string dataBefore = "")
        {
            return _logService.LogAction(actionType, descriptions, dataBefore);
        }

        /// <summary>
        /// XÃ³a nháº­t kÃ½
        /// </summary>
        public bool DeleteLog(int logId)
        {
            return _logService.DeleteLog(logId);
        }

        /// <summary>
        /// Láº¥y nháº­t kÃ½ theo loáº¡i hÃ nh Ä‘á»™ng
        /// </summary>
        public List<Actions> GetLogsByActionType(string actionType)
        {
            return _logService.GetLogsByActionType(actionType);
        }

        /// <summary>
        /// Láº¥y nháº­t kÃ½ trong má»™t khoáº£ng thá»i gian
        /// </summary>
        public List<Actions> GetLogsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _logService.GetLogsByDateRange(startDate, endDate);
        }

        /// <summary>
        /// Láº¥y nháº­t kÃ½ gáº§n nháº¥t cá»§a má»™t loáº¡i hÃ nh Ä‘á»™ng
        /// </summary>
        public Actions GetLatestLog(string actionType)
        {
            return _logService.GetLatestLog(actionType);
        }

        /// <summary>
        /// Kiá»ƒm tra cÃ³ nháº­t kÃ½ nÃ o khÃ´ng
        /// </summary>
        public bool HasLogs()
        {
            return _logService.HasLogs();
        }

        /// <summary>
        /// Äáº¿m tá»•ng sá»‘ nháº­t kÃ½
        /// </summary>
        public int CountLogs()
        {
            return _logService.CountLogs();
        }

        /// <summary>
        /// XÃ³a táº¥t cáº£ nháº­t kÃ½ khi káº¿t thÃºc phiÃªn
        /// </summary>
        public bool ClearAllLogs()
        {
            return _logService.ClearAllLogs();
        }
    }
}





