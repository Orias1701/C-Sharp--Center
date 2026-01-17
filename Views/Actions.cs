using System;
using System.Windows.Forms;
using WarehouseManagement.Services;
using WarehouseManagement.Controllers;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Actions - Xá»­ lÃ½ cÃ¡c hÃ nh Ä‘á»™ng Save vÃ  Undo
    /// 
    /// TRÃCH NHIá»†M:
    /// - Xá»­ lÃ½ lÆ°u dá»¯ liá»‡u (CommitChanges)
    /// - Xá»­ lÃ½ hoÃ n tÃ¡c hÃ nh Ä‘á»™ng (UndoLastAction)
    /// - Cáº­p nháº­t tráº¡ng thÃ¡i lÆ°u (UpdateChangeStatus)
    /// - Hiá»ƒn thá»‹ cÃ¡c thÃ´ng bÃ¡o liÃªn quan
    /// </summary>
    public class Actions
    {
        private readonly ActionsService _actionsService;
        private readonly InventoryController _inventoryController;
        private readonly Label _lblChangeStatus;
        private readonly Button _btnSave;
        private readonly Action _onDataRefresh;

        public Actions(ActionsService actionsService, InventoryController inventoryController, Label lblChangeStatus, Button btnSave, Action onDataRefresh)
        {
            _actionsService = actionsService ?? ActionsService.Instance;
            _inventoryController = inventoryController;
            _lblChangeStatus = lblChangeStatus;
            _btnSave = btnSave;
            _onDataRefresh = onDataRefresh;
        }

        /// <summary>
        /// Xá»­ lÃ½ lÆ°u thay Ä‘á»•i vÃ o database
        /// </summary>
        public void Save()
        {
            try
            {
                if (!_actionsService.HasUnsavedChanges)
                {
                    MessageBox.Show("KhÃ´ng cÃ³ thay Ä‘á»•i nÃ o Ä‘á»ƒ lÆ°u.", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show(
                    $"Báº¡n muá»‘n lÆ°u {_actionsService.ChangeCount} thay Ä‘á»•i vÃ o database?",
                    "XÃ¡c nháº­n lÆ°u",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _actionsService.CommitChanges();
                    UpdateChangeStatus();
                    MessageBox.Show("ÄÃ£ lÆ°u thay Ä‘á»•i thÃ nh cÃ´ng!", "ThÃ nh cÃ´ng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i khi lÆ°u: {ex.Message}", "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xá»­ lÃ½ hoÃ n tÃ¡c hÃ nh Ä‘á»™ng gáº§n nháº¥t
        /// </summary>
        public void Undo()
        {
            try
            {
                if (_inventoryController == null)
                {
                    MessageBox.Show("Lá»—i: KhÃ´ng kháº£ dá»¥ng Ä‘á»ƒ hoÃ n tÃ¡c", "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = _inventoryController.UndoLastAction();
                if (result)
                {
                    MessageBox.Show("HoÃ n tÃ¡c thÃ nh cÃ´ng!\nHÃ nh Ä‘á»™ng Ä‘Ã£ bá»‹ xÃ³a khá»i lá»‹ch sá»­.", "ThÃ nh cÃ´ng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Giáº£m sá»‘ lÆ°á»£ng thay Ä‘á»•i chÆ°a lÆ°u
                    _actionsService.DecrementChangeCount();
                    
                    // Cáº­p nháº­t dá»¯ liá»‡u vÃ  UI
                    _onDataRefresh?.Invoke();
                    UpdateChangeStatus();
                }
                else
                {
                    MessageBox.Show("KhÃ´ng cÃ³ hÃ nh Ä‘á»™ng Ä‘á»ƒ hoÃ n tÃ¡c", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i khi hoÃ n tÃ¡c: {ex.Message}", "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t tráº¡ng thÃ¡i lÆ°u trÃªn UI
        /// </summary>
        public void UpdateChangeStatus()
        {
            if (_actionsService.HasUnsavedChanges)
            {
                _lblChangeStatus.Text = $"âš ï¸ ChÆ°a lÆ°u: {_actionsService.ChangeCount} thay Ä‘á»•i";
                _lblChangeStatus.ForeColor = System.Drawing.Color.Red;
                _btnSave.Enabled = true;
            }
            else
            {
                _lblChangeStatus.Text = "âœ“ Táº¥t cáº£ thay Ä‘á»•i Ä‘Ã£ Ä‘Æ°á»£c lÆ°u";
                _lblChangeStatus.ForeColor = System.Drawing.Color.Green;
                _btnSave.Enabled = false;
            }
        }
    }
}




