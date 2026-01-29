using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using WarehouseManagement.Controllers;
using WarehouseManagement.Services;
using WarehouseManagement.UI;
using WarehouseManagement.UI.Components;

namespace WarehouseManagement.Views.Panels
{
    public class DashboardPanel : Panel
    {
        private TableLayoutPanel mainGrid;
        private ChartService chartService;
        private ProductController productController;
        private InventoryCheckController inventoryCheckController;
        private StockTransactionController transactionController;
        
        // Charts
        private PictureBox pictureBoxImport;
        private PictureBox pictureBoxExport;
        private PictureBox pictureBoxPie;
        
        // Stat cards
        private CustomPanel cardTotalInventory;
        private CustomPanel cardTotalLoss;
        private CustomPanel cardTotalExport;
        private CustomPanel cardTotalImport;

        public DashboardPanel()
        {
            chartService = new ChartService();
            productController = new ProductController();
            inventoryCheckController = new InventoryCheckController();
            transactionController = new StockTransactionController();
            InitializeComponent();
            
            ThemeManager.Instance.ThemeChanged += OnThemeChanged;
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = ThemeManager.Instance.BackgroundDefault;
            Padding = new Padding(20);

            // Main grid: 5 columns, 4 rows
            mainGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 4,
                BackColor = Color.Transparent
            };

            // Column styles: equal width
            for (int i = 0; i < 5; i++)
            {
                mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            }

            // Row styles: equal height
            for (int i = 0; i < 4; i++)
            {
                mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            }

            // Row 1-2, Col 1-3: Import chart (30 days)
            CustomPanel importChartPanel = CreateChartPanel("Biểu Đồ Nhập Kho (30 ngày)", UIConstants.SemanticColors.Success);
            pictureBoxImport = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.Instance.ChartBackground,
                BorderStyle = BorderStyle.None
            };
            importChartPanel.Controls.Add(pictureBoxImport);
            mainGrid.Controls.Add(importChartPanel, 0, 0);
            mainGrid.SetColumnSpan(importChartPanel, 3);
            mainGrid.SetRowSpan(importChartPanel, 2);

            // Row 1-2, Col 4-5: Pie chart (không có title)
            CustomPanel pieChartPanel = new CustomPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = UIConstants.Borders.RadiusLarge,
                ShowBorder = true,
                HasShadow = true,
                Padding = new Padding(UIConstants.Spacing.Padding.Medium)
            };
            pictureBoxPie = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.Instance.ChartBackground,
                BorderStyle = BorderStyle.None
            };
            pieChartPanel.Controls.Add(pictureBoxPie);
            mainGrid.Controls.Add(pieChartPanel, 3, 0);
            mainGrid.SetColumnSpan(pieChartPanel, 2);
            mainGrid.SetRowSpan(pieChartPanel, 2);

            // Row 3-4, Col 1-2: 4 stat cards (2x2 grid)
            TableLayoutPanel statsGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            for (int i = 0; i < 2; i++)
            {
                statsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                statsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            }

            // Card 1: Tổng giá trị tồn kho
            cardTotalInventory = CreateStatCard("Tổng Giá Trị Tồn Kho", "0", UIConstants.SemanticColors.Success, () => {
                // Mở ProductsPanel
                var mainForm = Application.OpenForms.OfType<Views.Main>().FirstOrDefault();
                mainForm?.ShowPanel(1); // Products panel index
            });
            statsGrid.Controls.Add(cardTotalInventory, 0, 0);

            // Card 2: Tổng tiền thất thoát
            cardTotalLoss = CreateStatCard("Tổng Tiền Thất Thoát", "0", UIConstants.SemanticColors.Error, () => {
                // Mở InventoryChecksPanel
                var mainForm = Application.OpenForms.OfType<Views.Main>().FirstOrDefault();
                mainForm?.ShowPanel(5); // InventoryChecks panel index
            });
            statsGrid.Controls.Add(cardTotalLoss, 1, 0);

            // Card 3: Tổng giá trị xuất
            cardTotalExport = CreateStatCard("Tổng Giá Trị Xuất", "0", UIConstants.SemanticColors.Info, () => {
                // Mở TransactionsPanel
                var mainForm = Application.OpenForms.OfType<Views.Main>().FirstOrDefault();
                mainForm?.ShowPanel(2); // Transactions panel index
            });
            statsGrid.Controls.Add(cardTotalExport, 0, 1);

            // Card 4: Tổng giá trị nhập
            cardTotalImport = CreateStatCard("Tổng Giá Trị Nhập", "0", UIConstants.SemanticColors.Success, () => {
                // Mở TransactionsPanel
                var mainForm = Application.OpenForms.OfType<Views.Main>().FirstOrDefault();
                mainForm?.ShowPanel(2); // Transactions panel index
            });
            statsGrid.Controls.Add(cardTotalImport, 1, 1);

            mainGrid.Controls.Add(statsGrid, 0, 2);
            mainGrid.SetColumnSpan(statsGrid, 2);
            mainGrid.SetRowSpan(statsGrid, 2);

            // Row 3-4, Col 3-5: Export chart (30 days)
            CustomPanel exportChartPanel = CreateChartPanel("Biểu Đồ Xuất Kho (30 ngày)", UIConstants.SemanticColors.Error);
            pictureBoxExport = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.Instance.ChartBackground,
                BorderStyle = BorderStyle.None
            };
            exportChartPanel.Controls.Add(pictureBoxExport);
            mainGrid.Controls.Add(exportChartPanel, 2, 2);
            mainGrid.SetColumnSpan(exportChartPanel, 3);
            mainGrid.SetRowSpan(exportChartPanel, 2);

            Controls.Add(mainGrid);

            VisibleChanged += (s, e) =>
            {
                if (Visible)
                    LoadDashboard();
            };
        }

        private CustomPanel CreateChartPanel(string title, Color titleColor)
        {
            CustomPanel panel = new CustomPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = UIConstants.Borders.RadiusLarge,
                ShowBorder = true,
                HasShadow = true,
                Padding = new Padding(UIConstants.Spacing.Padding.Medium)
            };

            Label lblTitle = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 30,
                Font = ThemeManager.Instance.FontBold,
                ForeColor = titleColor,
                Padding = new Padding(0, 0, 0, 5)
            };
            panel.Controls.Add(lblTitle);

            return panel;
        }

        private CustomPanel CreateStatCard(string title, string value, Color accentColor, Action onClick)
        {
            CustomPanel card = new CustomPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = UIConstants.Borders.RadiusLarge,
                ShowBorder = true,
                HasShadow = true,
                Padding = new Padding(15),
                Margin = new Padding(5),
                Cursor = Cursors.Hand
            };

            Label lblTitle = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 25,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = ThemeManager.Instance.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblValue = new Label
            {
                Text = value,
                Dock = DockStyle.Fill,
                Font = new Font(ThemeManager.Instance.FontBold.FontFamily, 18, FontStyle.Bold),
                ForeColor = accentColor,
                TextAlign = ContentAlignment.MiddleLeft
            };

            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);

            card.Click += (s, e) => onClick?.Invoke();
            lblTitle.Click += (s, e) => onClick?.Invoke();
            lblValue.Click += (s, e) => onClick?.Invoke();

            return card;
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme();
            if (pictureBoxImport != null) pictureBoxImport.BackColor = ThemeManager.Instance.ChartBackground;
            if (pictureBoxExport != null) pictureBoxExport.BackColor = ThemeManager.Instance.ChartBackground;
            if (pictureBoxPie != null) pictureBoxPie.BackColor = ThemeManager.Instance.ChartBackground;
            LoadDashboard();
        }

        private void ApplyTheme()
        {
            BackColor = ThemeManager.Instance.BackgroundDefault;
        }

        public void LoadDashboard()
        {
            try
            {
                // Lấy datetime picker từ ToolsBar thông qua Main
                DateTime anchorDate = DateTime.Now;
                var mainForm = Application.OpenForms.OfType<Views.Main>().FirstOrDefault();
                if (mainForm != null && mainForm.ToolsBar?.DtpAnchorDate != null)
                {
                    anchorDate = mainForm.ToolsBar.DtpAnchorDate.Value;
                }
                
                // Load charts
                LoadImportChart();
                LoadExportChart();
                LoadPieChart(anchorDate);
                
                // Load stats
                LoadStats(anchorDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{UIConstants.Icons.Error} Lỗi tải dashboard: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadImportChart()
        {
            try
            {
                var dailyData = chartService.GetImportExportByDay(DateTime.Now);
                var days = new List<string>();
                var imports = new List<decimal>();
                decimal maxImport = 0;

                foreach (var dayEntry in dailyData)
                {
                    days.Add(dayEntry.Key);
                    decimal importValue = dayEntry.Value["Import"];
                    imports.Add(importValue);
                    if (importValue > maxImport) maxImport = importValue;
                }

                if (pictureBoxImport.Width > 0 && pictureBoxImport.Height > 0)
                {
                    DrawImportChart(days, imports, maxImport);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardPanel] LoadImportChart ERROR: {ex.Message}");
            }
        }

        private void LoadExportChart()
        {
            try
            {
                var dailyData = chartService.GetImportExportByDay(DateTime.Now);
                var days = new List<string>();
                var exports = new List<decimal>();
                decimal maxExport = 0;

                foreach (var dayEntry in dailyData)
                {
                    days.Add(dayEntry.Key);
                    decimal exportValue = dayEntry.Value["Export"];
                    exports.Add(exportValue);
                    if (exportValue > maxExport) maxExport = exportValue;
                }

                if (pictureBoxExport.Width > 0 && pictureBoxExport.Height > 0)
                {
                    DrawExportChart(days, exports, maxExport);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardPanel] LoadExportChart ERROR: {ex.Message}");
            }
        }

        private void LoadPieChart(DateTime anchorDate)
        {
            try
            {
                DateTime startDate = anchorDate.AddDays(-29);
                DateTime endDate = anchorDate;

                // Get data
                decimal totalLoss = chartService.GetTotalLossAmount(startDate, endDate);
                decimal totalExport = chartService.GetTotalExportValue(startDate, endDate);
                decimal totalImport = chartService.GetTotalImportValue(startDate, endDate);

                if (pictureBoxPie.Width > 0 && pictureBoxPie.Height > 0)
                {
                    DrawPieChart(totalLoss, totalExport, totalImport);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardPanel] LoadPieChart ERROR: {ex.Message}");
            }
        }

        private void LoadStats(DateTime anchorDate)
        {
            try
            {
                DateTime startDate = anchorDate.AddDays(-29);
                DateTime endDate = anchorDate;

                // Total inventory value
                decimal totalInventory = productController.GetAllProducts().Sum(p => p.InventoryValue);
                UpdateStatCard(cardTotalInventory, totalInventory.ToString("N0") + " VND");

                // Total loss
                decimal totalLoss = chartService.GetTotalLossAmount(startDate, endDate);
                UpdateStatCard(cardTotalLoss, totalLoss.ToString("N0") + " VND");

                // Total export
                decimal totalExport = chartService.GetTotalExportValue(startDate, endDate);
                UpdateStatCard(cardTotalExport, totalExport.ToString("N0") + " VND");

                // Total import
                decimal totalImport = chartService.GetTotalImportValue(startDate, endDate);
                UpdateStatCard(cardTotalImport, totalImport.ToString("N0") + " VND");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardPanel] LoadStats ERROR: {ex.Message}");
            }
        }

        private void UpdateStatCard(CustomPanel card, string value)
        {
            if (card?.Controls.Count >= 2)
            {
                var lblValue = card.Controls[0] as Label;
                if (lblValue != null)
                {
                    lblValue.Text = value;
                }
            }
        }

        private void DrawImportChart(List<string> days, List<decimal> imports, decimal maxImport)
        {
            try
            {
                if (pictureBoxImport.Width <= 0 || pictureBoxImport.Height <= 0 || days.Count == 0 || maxImport <= 0)
                    return;

                Bitmap bitmap = new Bitmap(pictureBoxImport.Width, pictureBoxImport.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(ThemeManager.Instance.ChartBackground);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                int leftMargin = 50;
                int rightMargin = 10;
                int topMargin = 10;
                int bottomMargin = 35;
                
                int chartWidth = pictureBoxImport.Width - leftMargin - rightMargin;
                int chartHeight = pictureBoxImport.Height - topMargin - bottomMargin;
                
                double scaleFactor = 0.85;
                
                int totalBars = days.Count;
                int spacing = 3;
                int barWidth = Math.Max(8, (chartWidth - (spacing * (totalBars - 1))) / totalBars);

                Pen gridPen = new Pen(ThemeManager.Instance.ChartGridLine, 1);
                Font labelFont = new Font("Segoe UI", 8);
                int gridLines = 5;
                for (int i = 0; i <= gridLines; i++)
                {
                    int y = pictureBoxImport.Height - bottomMargin - (chartHeight * i / gridLines);
                    g.DrawLine(gridPen, leftMargin, y, pictureBoxImport.Width - rightMargin, y);
                    
                    decimal value = maxImport * i / gridLines;
                    string label = value >= 1000000 ? $"{value / 1000000:F1}M" : value >= 1000 ? $"{value / 1000:F0}K" : $"{value:F0}";
                    SizeF labelSize = g.MeasureString(label, labelFont);
                    g.DrawString(label, labelFont, new SolidBrush(ThemeManager.Instance.ChartLabel), leftMargin - labelSize.Width - 5, y - labelSize.Height / 2);
                }

                int xPos = leftMargin;
                Color startColor = UIConstants.SemanticColors.Success;
                Color endColor = UIConstants.SemanticColors.SuccessLight;

                for (int i = 0; i < days.Count; i++)
                {
                    int barHeight = maxImport > 0 ? (int)(((double)imports[i] / (double)maxImport) * chartHeight * scaleFactor) : 0;
                    int y = pictureBoxImport.Height - bottomMargin - barHeight;
                    
                    if (barHeight > 0)
                    {
                        LinearGradientBrush gradientBrush = 
                            new LinearGradientBrush(
                                new Rectangle(xPos, y, barWidth, barHeight),
                                startColor,
                                endColor,
                                LinearGradientMode.Vertical);
                        
                        int radius = Math.Min(4, barWidth / 2);
                        GraphicsPath path = new GraphicsPath();
                        path.AddLine(xPos, y + barHeight, xPos, y + radius);
                        path.AddArc(xPos, y, radius * 2, radius * 2, 180, 90);
                        path.AddLine(xPos + radius, y, xPos + barWidth - radius, y);
                        path.AddArc(xPos + barWidth - radius * 2, y, radius * 2, radius * 2, 270, 90);
                        path.AddLine(xPos + barWidth, y + radius, xPos + barWidth, y + barHeight);
                        path.CloseFigure();
                        
                        g.FillPath(gradientBrush, path);
                        gradientBrush.Dispose();
                    }

                    if (i % Math.Max(1, days.Count / 10) == 0 || i == days.Count - 1)
                    {
                        string dayLabel = days[i].Substring(5);
                        SizeF daySize = g.MeasureString(dayLabel, labelFont);
                        g.DrawString(dayLabel, labelFont, new SolidBrush(ThemeManager.Instance.ChartLabel), xPos + (barWidth - daySize.Width) / 2, pictureBoxImport.Height - bottomMargin + 5);
                    }

                    xPos += barWidth + spacing;
                }

                pictureBoxImport.Image = bitmap;
                g.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardPanel] DrawImportChart ERROR: {ex.Message}");
            }
        }

        private void DrawExportChart(List<string> days, List<decimal> exports, decimal maxExport)
        {
            try
            {
                if (pictureBoxExport.Width <= 0 || pictureBoxExport.Height <= 0 || days.Count == 0 || maxExport <= 0)
                    return;

                Bitmap bitmap = new Bitmap(pictureBoxExport.Width, pictureBoxExport.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(ThemeManager.Instance.ChartBackground);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                int leftMargin = 50;
                int rightMargin = 10;
                int topMargin = 10;
                int bottomMargin = 35;
                
                int chartWidth = pictureBoxExport.Width - leftMargin - rightMargin;
                int chartHeight = pictureBoxExport.Height - topMargin - bottomMargin;
                
                double scaleFactor = 0.85;
                
                int totalBars = days.Count;
                int spacing = 3;
                int barWidth = Math.Max(8, (chartWidth - (spacing * (totalBars - 1))) / totalBars);

                Pen gridPen = new Pen(ThemeManager.Instance.ChartGridLine, 1);
                Font labelFont = new Font("Segoe UI", 8);
                int gridLines = 5;
                for (int i = 0; i <= gridLines; i++)
                {
                    int y = pictureBoxExport.Height - bottomMargin - (chartHeight * i / gridLines);
                    g.DrawLine(gridPen, leftMargin, y, pictureBoxExport.Width - rightMargin, y);
                    
                    decimal value = maxExport * i / gridLines;
                    string label = value >= 1000000 ? $"{value / 1000000:F1}M" : value >= 1000 ? $"{value / 1000:F0}K" : $"{value:F0}";
                    SizeF labelSize = g.MeasureString(label, labelFont);
                    g.DrawString(label, labelFont, new SolidBrush(ThemeManager.Instance.ChartLabel), leftMargin - labelSize.Width - 5, y - labelSize.Height / 2);
                }

                int xPos = leftMargin;
                Color startColor = UIConstants.SemanticColors.Error;
                Color endColor = UIConstants.SemanticColors.ErrorLight;

                for (int i = 0; i < days.Count; i++)
                {
                    int barHeight = maxExport > 0 ? (int)(((double)exports[i] / (double)maxExport) * chartHeight * scaleFactor) : 0;
                    int y = pictureBoxExport.Height - bottomMargin - barHeight;
                    
                    if (barHeight > 0)
                    {
                        LinearGradientBrush gradientBrush = 
                            new LinearGradientBrush(
                                new Rectangle(xPos, y, barWidth, barHeight),
                                startColor,
                                endColor,
                                LinearGradientMode.Vertical);
                        
                        int radius = Math.Min(4, barWidth / 2);
                        GraphicsPath path = new GraphicsPath();
                        path.AddLine(xPos, y + barHeight, xPos, y + radius);
                        path.AddArc(xPos, y, radius * 2, radius * 2, 180, 90);
                        path.AddLine(xPos + radius, y, xPos + barWidth - radius, y);
                        path.AddArc(xPos + barWidth - radius * 2, y, radius * 2, radius * 2, 270, 90);
                        path.AddLine(xPos + barWidth, y + radius, xPos + barWidth, y + barHeight);
                        path.CloseFigure();
                        
                        g.FillPath(gradientBrush, path);
                        gradientBrush.Dispose();
                    }

                    if (i % Math.Max(1, days.Count / 10) == 0 || i == days.Count - 1)
                    {
                        string dayLabel = days[i].Substring(5);
                        SizeF daySize = g.MeasureString(dayLabel, labelFont);
                        g.DrawString(dayLabel, labelFont, new SolidBrush(ThemeManager.Instance.ChartLabel), xPos + (barWidth - daySize.Width) / 2, pictureBoxExport.Height - bottomMargin + 5);
                    }

                    xPos += barWidth + spacing;
                }

                pictureBoxExport.Image = bitmap;
                g.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardPanel] DrawExportChart ERROR: {ex.Message}");
            }
        }

        private void DrawPieChart(decimal totalLoss, decimal totalExport, decimal totalImport)
        {
            try
            {
                if (pictureBoxPie.Width <= 0 || pictureBoxPie.Height <= 0)
                    return;

                Bitmap bitmap = new Bitmap(pictureBoxPie.Width, pictureBoxPie.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(ThemeManager.Instance.ChartBackground);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                decimal total = totalLoss + totalExport + totalImport;
                if (total <= 0)
                {
                    // Draw "No data" message
                    Font font = new Font("Segoe UI", 12);
                    string text = "Không có dữ liệu";
                    SizeF textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, new SolidBrush(ThemeManager.Instance.ChartLabel), 
                        (pictureBoxPie.Width - textSize.Width) / 2, 
                        (pictureBoxPie.Height - textSize.Height) / 2);
                    pictureBoxPie.Image = bitmap;
                    g.Dispose();
                    return;
                }

                int centerX = pictureBoxPie.Width / 2;
                int centerY = pictureBoxPie.Height / 2;
                int radius = Math.Min(pictureBoxPie.Width, pictureBoxPie.Height) / 2 - 40;
                int innerRadius = (int)(radius * 0.5); // Hình tròn trong nhỏ hơn 50%
                
                float startAngle = -90; // Start from top
                
                // Colors
                Color colorLoss = UIConstants.SemanticColors.Error;
                Color colorExport = UIConstants.SemanticColors.Info;
                Color colorImport = UIConstants.SemanticColors.Success;
                Color backgroundColor = ThemeManager.Instance.ChartBackground;

                // Draw pie slices (vẽ như bình thường)
                if (totalLoss > 0)
                {
                    float sweepAngle = (float)(totalLoss / total * 360);
                    g.FillPie(new SolidBrush(colorLoss), centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, sweepAngle);
                    startAngle += sweepAngle;
                }

                if (totalExport > 0)
                {
                    float sweepAngle = (float)(totalExport / total * 360);
                    g.FillPie(new SolidBrush(colorExport), centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, sweepAngle);
                    startAngle += sweepAngle;
                }

                if (totalImport > 0)
                {
                    float sweepAngle = (float)(totalImport / total * 360);
                    g.FillPie(new SolidBrush(colorImport), centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, sweepAngle);
                }

                // Vẽ hình tròn trong màu nền đồng tâm để tạo hiệu ứng donut chart
                g.FillEllipse(new SolidBrush(backgroundColor), centerX - innerRadius, centerY - innerRadius, innerRadius * 2, innerRadius * 2);

                // Draw legend
                Font legendFont = new Font("Segoe UI", 9);
                int legendY = 20;
                int legendX = 20;
                
                DrawLegendItem(g, legendFont, "Thất thoát", totalLoss, colorLoss, legendX, legendY);
                legendY += 25;
                DrawLegendItem(g, legendFont, "Xuất kho", totalExport, colorExport, legendX, legendY);
                legendY += 25;
                DrawLegendItem(g, legendFont, "Nhập kho", totalImport, colorImport, legendX, legendY);

                pictureBoxPie.Image = bitmap;
                g.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardPanel] DrawPieChart ERROR: {ex.Message}");
            }
        }

        private void DrawLegendItem(Graphics g, Font font, string label, decimal value, Color color, int x, int y)
        {
            // Color box
            g.FillRectangle(new SolidBrush(color), x, y, 15, 15);
            g.DrawRectangle(new Pen(ThemeManager.Instance.ChartLegendBorder, 1), x, y, 15, 15);
            
            // Label and value
            string text = $"{label}: {value:N0}";
            g.DrawString(text, font, new SolidBrush(ThemeManager.Instance.TextPrimary), x + 20, y);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeManager.Instance.ThemeChanged -= OnThemeChanged;
            }
            base.Dispose(disposing);
        }
    }
}
