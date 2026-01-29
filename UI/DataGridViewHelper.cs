using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.UI;

namespace WarehouseManagement.Helpers
{
    public static class DataGridViewHelper
    {
        public static void ApplyHoverEffect(DataGridView dgv)
        {
            dgv.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    // Only apply if not selected to avoid conflict (optional, but cleaner)
                    // But usually user wants to see hover even if selected or not.
                    // However, we don't want to override Selection Color if it's selected.
                    // DataGridView paints SelectionColor if Selected is true. 
                    // Manual BackColor setting overrides 'DefaultCellStyle' but SelectionBackColor is 'SelectionBackColor'.
                    
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = UIConstants.PrimaryColor.HoverLight;
                }
            };

            dgv.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    // Revert to default
                    // We assume the default row background is defined by ThemeManager
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = ThemeManager.Instance.BackgroundDefault;
                }
            };
        }

        public static void ApplySelectionEffect(DataGridView dgv)
        {
            // Apply Selection Colors
            dgv.DefaultCellStyle.SelectionBackColor = UIConstants.PrimaryColor.Light;
            dgv.DefaultCellStyle.SelectionForeColor = ThemeManager.Instance.TextPrimary;
            
            // Header Selection Colors (keep same as normal header)
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = UIConstants.PrimaryColor.Default;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = UIConstants.TextOnColor.Default;
        }
    }
}
