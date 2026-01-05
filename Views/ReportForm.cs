using System;
using System.Windows.Forms;

namespace WarehouseManagement.Views
{
    /// <summary>
    /// Form hiển thị báo cáo và biểu đồ (sử dụng LiveCharts)
    /// </summary>
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
            Text = "Báo Cáo & Biểu Đồ";
        }

        private void InitializeComponent()
        {
            // TODO: Thêm các control:
            // - ToolStrip: DatePicker từ-đến, Button Xem báo cáo
            // - TabControl:
            //   * Tab Tồn kho: Biểu đồ cột tồn kho theo SP
            //   * Tab Giá trị: Biểu đồ cột giá trị theo SP
            //   * Tab Nhập/Xuất: Biểu đồ đường Nhập/Xuất theo ngày
            //   * Tab Thống kê: Hiển thị con số tổng quát
            // - Sử dụng Chart hoặc LiveCharts
            SuspendLayout();
            ResumeLayout(false);
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            // Tải dữ liệu và vẽ biểu đồ
        }

        /// <summary>
        /// Nút Xem báo cáo
        /// </summary>
        private void BtnViewReport_Click(object sender, EventArgs e)
        {
            // Tải lại dữ liệu và vẽ biểu đồ theo khoảng ngày chọn
        }

        /// <summary>
        /// Nút Xuất Excel
        /// </summary>
        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            // Xuất báo cáo thành file Excel
        }
    }
}
