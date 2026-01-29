using System;
using System.Windows.Forms;
using WarehouseManagement.Services;
using WarehouseManagement.Controllers;
using WarehouseManagement.UI;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Actions - Xử lý các hành động Save và Undo
    /// 
    /// TRÁCH NHIỆM:
    /// - Xử lý lưu dữ liệu (CommitChanges)
    /// - Xử lý hoàn tác hành động (UndoLastAction)
    /// - Cập nhật trạng thái lưu (UpdateChangeStatus)
    /// - Hiển thị các thông báo liên quan
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
        /// Xử lý lưu thay đổi vào database và xóa toàn bộ bản ghi Actions
        /// </summary>
        public void Save()
        {
            try
            {
                int actionCount = _actionsService.CountLogs();
                
                if (actionCount == 0)
                {
                    MessageBox.Show("Không có thay đổi nào để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show(
                    $"Bạn muốn lưu {actionCount} thay đổi vào database?\nTất cả bản ghi thao tác sẽ được xóa sau khi lưu.",
                    "Xác nhận lưu",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Xóa toàn bộ bản ghi trong bảng Actions
                    _actionsService.ClearAllLogs();
                    
                    // Reset trạng thái
                    _actionsService.Reset();
                    
                    UpdateChangeStatus();
                    MessageBox.Show("Đã lưu thay đổi và xóa lịch sử thao tác thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý hoàn tác hành động gần nhất
        /// </summary>
        public void Undo()
        {
            try
            {
                if (_inventoryController == null)
                {
                    MessageBox.Show("Lỗi: Không khả dụng để hoàn tác", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = _inventoryController.UndoLastAction();
                if (result)
                {
                    MessageBox.Show("Hoàn tác thành công!\nHành động đã bị xóa khỏi lịch sử.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Cập nhật dữ liệu và UI
                    _onDataRefresh?.Invoke();
                    UpdateChangeStatus();
                }
                else
                {
                    MessageBox.Show("Không có hành động để hoàn tác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hoàn tác: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái lưu trên UI - Hiển thị số lượng bản ghi trong bảng Actions
        /// </summary>
        public void UpdateChangeStatus()
        {
            int actionCount = _actionsService.CountLogs();
            
            if (actionCount > 0)
            {
                string changeText = actionCount == 1 ? "1 change" : $"{actionCount} changes";
                _lblChangeStatus.Text = changeText;
                _lblChangeStatus.ForeColor = UIConstants.SemanticColors.Warning;
                _btnSave.Enabled = true;
            }
            else
            {
                _lblChangeStatus.Text = "";
                _lblChangeStatus.ForeColor = UIConstants.TextLight.Hint;
                _btnSave.Enabled = false;
            }
        }
    }
}