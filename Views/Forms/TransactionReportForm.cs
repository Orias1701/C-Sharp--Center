using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WarehouseManagement.Services;

namespace WarehouseManagement.Views.Forms
{
    /// <summary>
    /// Form bÃ¡o cÃ¡o Nháº­p/Xuáº¥t theo ngÃ y (30 ngÃ y gáº§n nháº¥t)
    /// Hiá»ƒn thá»‹ biá»ƒu Ä‘á»“ cá»™t Ä‘Ã´i vÃ  báº£ng dá»¯ liá»‡u
    /// </summary>
    public class TransactionReportForm : Form
    {
        private PictureBox pictureBox;
        private DataGridView dgvReport;
        private ChartService chartService;
        private List<string> days;
        private List<decimal> imports;
        private List<decimal> exports;
        private decimal maxValue;
        private DateTimePicker dtpAnchorDate;

        public TransactionReportForm()
        {
            chartService = new ChartService();
            days = new List<string>();
            imports = new List<decimal>();
            exports = new List<decimal>();
            Console.WriteLine("[TransactionReportForm] Constructor started");
            InitializeComponent();
            Console.WriteLine("[TransactionReportForm] Constructor completed");
        }

        private void InitializeComponent()
        {
            try
            {
                Console.WriteLine("[INFO] InitializeComponent: Báº¯t Ä‘áº§u");

                // Form settings
                Text = "ðŸ“Š BÃ¡o CÃ¡o Nháº­p/Xuáº¥t";
                Width = 1000;
                Height = 700;
                StartPosition = FormStartPosition.CenterParent;
                MaximizeBox = true;
                MinimizeBox = true;

                // Panel nÃºt (trÃªn cÃ¹ng)
                Panel buttonPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 60,
                    BackColor = Color.LightGray,
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label lblAnchorDate = new Label
                {
                    Text = "Chá»n ngÃ y:",
                    Left = 10,
                    Top = 10,
                    Width = 70,
                    Height = 20
                };
                buttonPanel.Controls.Add(lblAnchorDate);

                dtpAnchorDate = new DateTimePicker
                {
                    Left = 85,
                    Top = 8,
                    Width = 120,
                    Height = 25,
                    Value = DateTime.Now,
                    Format = DateTimePickerFormat.Short
                };
                dtpAnchorDate.ValueChanged += (s, e) => LoadReport();
                buttonPanel.Controls.Add(dtpAnchorDate);

                Button btnExportReport = new Button
                {
                    Text = "ðŸ“„ Xuáº¥t BÃ¡o CÃ¡o",
                    Left = 215,
                    Top = 8,
                    Width = 120,
                    Height = 30
                };
                btnExportReport.Click += BtnExportReport_Click;
                buttonPanel.Controls.Add(btnExportReport);

                // PictureBox cho biá»ƒu Ä‘á»“
                pictureBox = new PictureBox
                {
                    Dock = DockStyle.Top,
                    Height = 350,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.Fixed3D
                };
                pictureBox.Resize += PictureBox_Resize;

                // DataGridView cho báº£ng
                dgvReport = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.Fixed3D
                };

                dgvReport.Columns.Add("Day", "NgÃ y");
                dgvReport.Columns.Add("Import", "Tá»•ng Nháº­p Kho");
                dgvReport.Columns.Add("Export", "Tá»•ng Xuáº¥t Kho");

                Controls.Add(dgvReport);
                Controls.Add(pictureBox);
                Controls.Add(buttonPanel);

                Load += TransactionReportForm_Load;

                Console.WriteLine("[INFO] InitializeComponent: HoÃ n thÃ nh");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] InitializeComponent: {ex.Message}");
                MessageBox.Show($"Lá»—i khá»Ÿi táº¡o form: {ex.Message}");
            }
        }

        private void TransactionReportForm_Load(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void BtnExportReport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx|CSV Files (*.csv)|*.csv",
                    DefaultExt = "xlsx",
                    FileName = $"BaoCaoNhapXuat_{DateTime.Now:yyyyMMdd_HHmmss}"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    if (saveDialog.FileName.EndsWith(".csv"))
                    {
                        ExportToCSV(saveDialog.FileName);
                    }
                    else
                    {
                        ExportToExcel(saveDialog.FileName);
                    }
                    MessageBox.Show("Xuáº¥t bÃ¡o cÃ¡o thÃ nh cÃ´ng!", "ThÃ´ng bÃ¡o", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i xuáº¥t bÃ¡o cÃ¡o: {ex.Message}", "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV(string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Header
                writer.WriteLine("NgÃ y,Tá»•ng Nháº­p Kho,Tá»•ng Xuáº¥t Kho");

                // Data
                for (int i = 0; i < days.Count; i++)
                {
                    writer.WriteLine($"{days[i]},{imports[i]:N0},{exports[i]:N0}");
                }
            }
        }

        private void ExportToExcel(string filePath)
        {
            try
            {
                // Sá»­ dá»¥ng Open XML Ä‘á»ƒ táº¡o file Excel
                using (var spreadsheet = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    using (var writer = new System.IO.StreamWriter(spreadsheet))
                    {
                        // Viáº¿t header XML cá»§a Excel
                        writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        writer.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                        writer.WriteLine("<Worksheet ss:Name=\"BaoCao\">");
                        writer.WriteLine("<Table>");

                        // Header row
                        writer.WriteLine("<Row>");
                        writer.WriteLine("<Cell><Data ss:Type=\"String\">NgÃ y</Data></Cell>");
                        writer.WriteLine("<Cell><Data ss:Type=\"String\">Tá»•ng Nháº­p Kho</Data></Cell>");
                        writer.WriteLine("<Cell><Data ss:Type=\"String\">Tá»•ng Xuáº¥t Kho</Data></Cell>");
                        writer.WriteLine("</Row>");

                        // Data rows
                        for (int i = 0; i < days.Count; i++)
                        {
                            writer.WriteLine("<Row>");
                            writer.WriteLine($"<Cell><Data ss:Type=\"String\">{days[i]}</Data></Cell>");
                            writer.WriteLine($"<Cell><Data ss:Type=\"Number\">{imports[i]}</Data></Cell>");
                            writer.WriteLine($"<Cell><Data ss:Type=\"Number\">{exports[i]}</Data></Cell>");
                            writer.WriteLine("</Row>");
                        }

                        writer.WriteLine("</Table>");
                        writer.WriteLine("</Worksheet>");
                        writer.WriteLine("</Workbook>");
                    }
                }
            }
            catch
            {
                // Fallback to CSV if Excel export fails
                ExportToCSV(filePath.Replace(".xlsx", ".csv"));
            }
        }

        private void PictureBox_Resize(object sender, EventArgs e)
        {
            if (pictureBox.Width > 0 && pictureBox.Height > 0 && days.Count > 0)
            {
                Console.WriteLine($"[INFO] PictureBox_Resize: {pictureBox.Width}x{pictureBox.Height}");
                DrawChart();
            }
        }

        private void LoadReport()
        {
            try
            {
                Console.WriteLine("[TransactionReportForm] LoadReport: Started");

                // Use the selected anchor date or default to today
                DateTime anchorDate = dtpAnchorDate != null ? dtpAnchorDate.Value : DateTime.Now;
                var dailyData = chartService.GetImportExportByDay(anchorDate);
                Console.WriteLine($"[TransactionReportForm] LoadReport: Got {dailyData.Count} days");

                // XÃ³a dá»¯ liá»‡u cÅ©
                dgvReport.Rows.Clear();
                days.Clear();
                imports.Clear();
                exports.Clear();
                maxValue = 0;

                // TÃ­nh toÃ¡n
                foreach (var dayEntry in dailyData)
                {
                    string day = dayEntry.Key;
                    decimal importValue = dayEntry.Value["Import"];
                    decimal exportValue = dayEntry.Value["Export"];

                    Console.WriteLine($"[TransactionReportForm] {day} -> Import={importValue}, Export={exportValue}");

                    days.Add(day);
                    imports.Add(importValue);
                    exports.Add(exportValue);

                    if (importValue > maxValue) maxValue = importValue;
                    if (exportValue > maxValue) maxValue = exportValue;

                    dgvReport.Rows.Add(day, importValue.ToString("C"), exportValue.ToString("C"));
                }

                Console.WriteLine($"[TransactionReportForm] LoadReport: Max value = {maxValue}, days.Count = {days.Count}");

                // Váº½ biá»ƒu Ä‘á»“ náº¿u PictureBox Ä‘Ã£ cÃ³ kÃ­ch thÆ°á»›c
                if (pictureBox.Width > 0 && pictureBox.Height > 0)
                {
                    Console.WriteLine($"[TransactionReportForm] PictureBox ready ({pictureBox.Width}x{pictureBox.Height}), calling DrawChart");
                    DrawChart();
                }
                else
                {
                    Console.WriteLine($"[ReportForm] PictureBox NOT ready ({pictureBox.Width}x{pictureBox.Height})");
                }

                Console.WriteLine("[ReportForm] LoadReport: Completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportForm] LoadReport ERROR: {ex.Message}");
                Console.WriteLine($"[ReportForm] {ex.StackTrace}");
                MessageBox.Show($"Lá»—i táº£i bÃ¡o cÃ¡o: {ex.Message}");
            }
        }

        private void DrawChart()
        {
            try
            {
                Console.WriteLine("[ReportForm] DrawChart: Started");
                Console.WriteLine($"[ReportForm] DrawChart: Size = {pictureBox.Width}x{pictureBox.Height}");
                Console.WriteLine($"[ReportForm] DrawChart: Days = {days.Count}, MaxValue = {maxValue}");

                if (pictureBox.Width <= 0 || pictureBox.Height <= 0)
                {
                    Console.WriteLine($"[ReportForm] DrawChart: PictureBox size invalid ({pictureBox.Width}x{pictureBox.Height})");
                    return;
                }

                if (maxValue <= 0 || days.Count == 0)
                {
                    Console.WriteLine($"[ReportForm] DrawChart: No data (maxValue={maxValue}, days={days.Count})");
                    return;
                }

                Console.WriteLine("[ReportForm] DrawChart: Creating bitmap");

                Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                int margin = 50;
                int chartWidth = pictureBox.Width - (margin * 2);
                int chartHeight = pictureBox.Height - (margin * 2) - 30;
                // TÃ­nh toÃ¡n Ä‘á»ƒ fit 30 ngÃ y: má»—i ngÃ y cÃ³ 2 cá»™t (nháº­p + xuáº¥t) + khoáº£ng cÃ¡ch nhá»
                int barWidth = Math.Max(2, chartWidth / (days.Count * 2 + 10)); // Giáº£m tá»« *3 xuá»‘ng *2
                int spacing = 2; // Khoáº£ng cÃ¡ch giá»¯a 2 cá»™t
                int daySpacing = 3; // Khoáº£ng cÃ¡ch giá»¯a cÃ¡c ngÃ y

                Console.WriteLine($"[ReportForm] DrawChart: chartWidth={chartWidth}, chartHeight={chartHeight}, barWidth={barWidth}");

                // TiÃªu Ä‘á»
                Font titleFont = new Font("Arial", 12, FontStyle.Bold);
                g.DrawString("BÃ¡o CÃ¡o Nháº­p/Xuáº¥t Theo NgÃ y (30 ngÃ y gáº§n nháº¥t)", titleFont, Brushes.Black, margin, 5);

                // Trá»¥c
                Pen axisPen = new Pen(Color.Black, 2);
                g.DrawLine(axisPen, margin, pictureBox.Height - margin, pictureBox.Width - margin, pictureBox.Height - margin);
                g.DrawLine(axisPen, margin, 30, margin, pictureBox.Height - margin);

                // Váº½ cá»™t
                int xPos = margin + 5;
                Brush greenBrush = new SolidBrush(Color.Green);
                Brush redBrush = new SolidBrush(Color.Red);
                Font monthFont = new Font("Arial", 5); // Giáº£m font size Ä‘á»ƒ fit 30 ngÃ y

                for (int i = 0; i < days.Count; i++)
                {
                    int importHeight = maxValue > 0 ? (int)((imports[i] / maxValue) * chartHeight) : 0;
                    int exportHeight = maxValue > 0 ? (int)((exports[i] / maxValue) * chartHeight) : 0;

                    // Nháº­p (xanh)
                    int y1 = pictureBox.Height - margin - importHeight;
                    g.FillRectangle(greenBrush, xPos, y1, barWidth, Math.Max(1, importHeight));
                    if (barWidth > 1)
                        g.DrawRectangle(new Pen(Color.DarkGreen, 1), xPos, y1, barWidth, Math.Max(1, importHeight));

                    // Xuáº¥t (Ä‘á») - váº½ bÃªn cáº¡nh cá»™t nháº­p
                    int xPos2 = xPos + barWidth + spacing;
                    int y2 = pictureBox.Height - margin - exportHeight;
                    g.FillRectangle(redBrush, xPos2, y2, barWidth, Math.Max(1, exportHeight));
                    if (barWidth > 1)
                        g.DrawRectangle(new Pen(Color.DarkRed, 1), xPos2, y2, barWidth, Math.Max(1, exportHeight));

                    // NgÃ y - hiá»ƒn thá»‹ má»—i 5 ngÃ y Ä‘á»ƒ khÃ´ng bá»‹ chá»“ng chÃ©o
                    if (i % 5 == 0)
                    {
                        string dayLabel = days[i].Substring(8); // Láº¥y pháº§n ngÃ y tá»« yyyy-MM-dd
                        g.DrawString(dayLabel, monthFont, Brushes.Black, xPos - 3, pictureBox.Height - margin + 3);
                    }

                    xPos += (barWidth * 2) + spacing + daySpacing;
                }

                Console.WriteLine("[ReportForm] DrawChart: Drawing bars completed");

                // Legend
                int legendX = pictureBox.Width - 150;
                int legendY = 35;
                g.FillRectangle(greenBrush, legendX, legendY, 15, 15);
                g.DrawString("Nháº­p", new Font("Arial", 9), Brushes.Black, legendX + 20, legendY);

                g.FillRectangle(redBrush, legendX, legendY + 20, 15, 15);
                g.DrawString("Xuáº¥t", new Font("Arial", 9), Brushes.Black, legendX + 20, legendY + 20);

                pictureBox.Image = bitmap;
                g.Dispose();

                Console.WriteLine("[ReportForm] DrawChart: Chart set to PictureBox");

                Console.WriteLine("[ReportForm] DrawChart: Completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReportForm] DrawChart ERROR: {ex.Message}");
                Console.WriteLine($"[ReportForm] {ex.StackTrace}");
            }
        }
    }
}




