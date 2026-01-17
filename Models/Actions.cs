using System;

namespace WarehouseManagement.Models
{
    /// <summary>
    /// Lá»›p thá»±c thá»ƒ Nháº­t kÃ½ hÃ nh Ä‘á»™ng (Há»— trá»£ Undo)
    /// </summary>
    public class Actions
    {
        public int LogID { get; set; }
        public string ActionType { get; set; } // 'UPDATE_STOCK', 'CREATE_TRANSACTION', etc.
        public string Descriptions { get; set; }
        public string DataBefore { get; set; } // JSON
        public DateTime CreatedAt { get; set; }
    }
}




