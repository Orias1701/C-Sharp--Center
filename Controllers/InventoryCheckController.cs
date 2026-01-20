using System.Collections.Generic;
using WarehouseManagement.Models;
using WarehouseManagement.Services;

namespace WarehouseManagement.Controllers
{
    public class InventoryCheckController
    {
        private readonly InventoryCheckService _checkService;

        public InventoryCheckController()
        {
            _checkService = new InventoryCheckService();
        }

        public List<InventoryCheck> GetAllChecks()
        {
            return _checkService.GetAllChecks();
        }

        public InventoryCheck GetCheckById(int id)
        {
            return _checkService.GetCheckById(id);
        }

        public int CreateCheck(int userId, string note, List<InventoryCheckDetail> details, string status = "Pending")
        {
            return _checkService.CreateCheck(userId, note, details, status);
        }

        public void CompleteCheck(int checkId, int userId)
        {
            _checkService.CompleteCheck(checkId, userId);
        }
    }
}
