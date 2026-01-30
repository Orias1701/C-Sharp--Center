using System;
using System.Drawing;
using System.Windows.Forms;

namespace WarehouseManagement.UI.Components
{
    /// <summary>
    /// Panel xem và thử các component, màu sắc, font, icon đang dùng trong dự án.
    /// </summary>
    public class ComponentsTestPanel : CustomPanel
    {
        private const int CardPadding = UIConstants.Spacing.Padding.Large;
        private const int SectionSpacing = UIConstants.Spacing.Margin.Large;
        private const int CardRadius = UIConstants.Borders.RadiusMedium;
        private const int ContentTopMargin = UIConstants.Spacing.Margin.Medium;
        private const int RowMarginVertical = UIConstants.Spacing.Margin.Medium;

        private Panel scrollContent;

        public ComponentsTestPanel()
        {
            Dock = DockStyle.Fill;
            AutoScroll = true;
            ShowBorder = false;
            BackColor = ThemeManager.Instance.BackgroundDefault;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SuspendLayout();

            scrollContent = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new Point(CardPadding, CardPadding),
                Width = 1100,
                BackColor = Color.Transparent
            };

            int y = 0;

            y = AddPageHeader(scrollContent, y);
            y += SectionSpacing;

            y = AddCardSection(scrollContent, y, "Màu sắc", "Bảng màu theme (Primary, Background, Semantic).", CreateColorsSection());
            y += SectionSpacing;

            y = AddCardSection(scrollContent, y, "Font chữ", "Các cấp độ typography trong ứng dụng.", CreateFontsSection());
            y += SectionSpacing;

            y = AddCardSection(scrollContent, y, "Icon đang dùng", "Chỉ hiển thị icon được sử dụng trong dự án. Click để copy.", CreateIconsSection());
            y += SectionSpacing;

            y = AddCardSection(scrollContent, y, "Button", "5 kiểu CustomButton.", CreateButtonsSection());
            y += SectionSpacing;

            y = AddCardSection(scrollContent, y, "Ô nhập liệu", "CustomTextBox và CustomComboBox.", CreateInputsSection());
            y += SectionSpacing;

            y = AddCardSection(scrollContent, y, "Chọn ngày giờ", "CustomDateTimePicker với các định dạng.", CreateDateTimePickerSection());
            y += SectionSpacing;

            y = AddCardSection(scrollContent, y, "Vùng văn bản", "CustomTextArea.", CreateTextAreaSection());
            y += SectionSpacing;

            y = AddCardSection(scrollContent, y, "Panel", "CustomPanel với BorderRadius khác nhau.", CreatePanelsSection());
            y += SectionSpacing;

            y = AddCardSection(scrollContent, y, "Khoảng cách", "Spacing (Padding) chuẩn.", CreateSpacingSection());

            Controls.Add(scrollContent);
            ResumeLayout(false);
        }

        private int AddPageHeader(Panel container, int yPos)
        {
            var card = CreateCard(0, yPos, 1100, 72);
            card.Padding = new Padding(CardPadding);

            var lblTitle = new Label
            {
                Text = "Thư viện Components",
                Font = ThemeManager.Instance.FontXXLarge,
                ForeColor = ThemeManager.Instance.TextPrimary,
                AutoSize = true,
                Location = new Point(CardPadding, CardPadding)
            };
            var lblSub = new Label
            {
                Text = "Màu sắc, font, icon và control đang dùng trong ứng dụng.",
                Font = ThemeManager.Instance.FontRegular,
                ForeColor = ThemeManager.Instance.TextSecondary,
                AutoSize = true,
                Location = new Point(CardPadding, CardPadding + 32)
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblSub);
            container.Controls.Add(card);
            return yPos + 72;
        }

        private int AddCardSection(Panel container, int yPos, string title, string description, Control content)
        {
            int contentHeight = content.Height;
            int headerHeight = 56 + ContentTopMargin;
            int cardHeight = headerHeight + ContentTopMargin + contentHeight + CardPadding * 2;

            var card = CreateCard(0, yPos, 1100, cardHeight);
            card.Padding = new Padding(CardPadding);

            var lblTitle = new Label
            {
                Text = title,
                Font = ThemeManager.Instance.FontXLarge,
                ForeColor = ThemeManager.Instance.PrimaryDefault,
                AutoSize = true,
                Location = new Point(CardPadding, CardPadding)
            };
            var lblDesc = new Label
            {
                Text = description,
                Font = ThemeManager.Instance.FontSmall,
                ForeColor = ThemeManager.Instance.TextSecondary,
                AutoSize = true,
                Location = new Point(CardPadding, CardPadding + 26)
            };
            content.Location = new Point(CardPadding, headerHeight + CardPadding + ContentTopMargin);

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblDesc);
            card.Controls.Add(content);
            container.Controls.Add(card);

            return yPos + cardHeight;
        }

        private CustomPanel CreateCard(int x, int y, int width, int height)
        {
            var card = new CustomPanel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BorderRadius = CardRadius,
                ShowBorder = true,
                BorderColor = ThemeManager.Instance.TextHint,
                BackColor = ThemeManager.Instance.BackgroundDefault,
                Padding = new Padding(CardPadding)
            };
            return card;
        }

        private Control CreateColorsSection()
        {
            var panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false
            };

            panel.Controls.Add(CreateColorRow("Primary", new[]
            {
                ("Default", UIConstants.PrimaryColor.Default),
                ("Active", UIConstants.PrimaryColor.Active),
                ("Hover", UIConstants.PrimaryColor.Hover),
                ("Pressed", UIConstants.PrimaryColor.Pressed),
                ("Disabled", UIConstants.PrimaryColor.Disabled),
                ("Light", UIConstants.PrimaryColor.Light)
            }));

            panel.Controls.Add(CreateColorRow("Background Light", new[]
            {
                ("Default", UIConstants.BackgroundLight.Default),
                ("Lighter", UIConstants.BackgroundLight.Lighter),
                ("Light", UIConstants.BackgroundLight.Light),
                ("Medium", UIConstants.BackgroundLight.Medium),
                ("Dark", UIConstants.BackgroundLight.Dark)
            }));

            panel.Controls.Add(CreateColorRow("Background Dark", new[]
            {
                ("Default", UIConstants.BackgroundDark.Default),
                ("Lighter", UIConstants.BackgroundDark.Lighter),
                ("Light", UIConstants.BackgroundDark.Light),
                ("Medium", UIConstants.BackgroundDark.Medium),
                ("Dark", UIConstants.BackgroundDark.Dark)
            }));

            panel.Controls.Add(CreateColorRow("Semantic", new[]
            {
                ("Success", UIConstants.SemanticColors.Success),
                ("Warning", UIConstants.SemanticColors.Warning),
                ("Error", UIConstants.SemanticColors.Error),
                ("Info", UIConstants.SemanticColors.Info)
            }));

            return panel;
        }

        private Control CreateColorRow(string rowName, (string name, Color color)[] colors)
        {
            var row = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0, RowMarginVertical, 0, RowMarginVertical)
            };

            var lblName = new Label
            {
                Text = rowName + ":",
                Width = 130,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = ThemeManager.Instance.FontRegular,
                ForeColor = ThemeManager.Instance.TextPrimary
            };
            row.Controls.Add(lblName);

            foreach (var (name, color) in colors)
            {
                var colorItem = new Panel { Width = 82, Height = 58, Margin = new Padding(UIConstants.Spacing.Padding.XSmall, 0, UIConstants.Spacing.Padding.XSmall, 0) };
                var colorBox = new Panel
                {
                    Width = 82,
                    Height = 40,
                    BackColor = color,
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(0, 0)
                };
                var capturedColor = color;
                colorBox.Paint += (s, e) =>
                {
                    using (var brush = new SolidBrush(capturedColor))
                        e.Graphics.FillRectangle(brush, colorBox.ClientRectangle);
                };
                var lblColorName = new Label
                {
                    Text = name,
                    Width = 82,
                    Height = 16,
                    Location = new Point(0, 42),
                    TextAlign = ContentAlignment.TopCenter,
                    Font = new Font(UIConstants.Fonts.FontFamily, UIConstants.Fonts.XXSmall),
                    ForeColor = ThemeManager.Instance.TextSecondary
                };
                colorItem.Controls.Add(colorBox);
                colorItem.Controls.Add(lblColorName);
                row.Controls.Add(colorItem);
            }
            return row;
        }

        private Control CreateFontsSection()
        {
            var panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false
            };

            var fonts = new[]
            {
                ("XXSmall 9px", ThemeManager.Instance.FontXXSmall),
                ("XSmall 10px", ThemeManager.Instance.FontXSmall),
                ("Small 11px", ThemeManager.Instance.FontSmall),
                ("Regular 12px", ThemeManager.Instance.FontRegular),
                ("Medium 14px", ThemeManager.Instance.FontMedium),
                ("Large 16px", ThemeManager.Instance.FontLarge),
                ("XLarge 20px", ThemeManager.Instance.FontXLarge),
                ("XXLarge 24px", ThemeManager.Instance.FontXXLarge)
            };

            foreach (var (name, font) in fonts)
            {
                var lbl = new Label
                {
                    Text = $"{name} — The quick brown fox jumps over the lazy dog",
                    Font = font,
                    AutoSize = true,
                    ForeColor = ThemeManager.Instance.TextPrimary,
                    Margin = new Padding(0, RowMarginVertical, 0, RowMarginVertical)
                };
                panel.Controls.Add(lbl);
            }
            return panel;
        }

        private Control CreateIconsSection()
        {
            var usedIcons = new (string name, string icon)[]
            {
                ("Warehouse", UIConstants.Icons.Warehouse),
                ("Dashboard", UIConstants.Icons.Dashboard),
                ("Category", UIConstants.Icons.Category),
                ("Product", UIConstants.Icons.Product),
                ("Supplier", UIConstants.Icons.Supplier),
                ("Customer", UIConstants.Icons.Customer),
                ("Transaction", UIConstants.Icons.Transaction),
                ("Check", UIConstants.Icons.Check),
                ("Settings", UIConstants.Icons.Settings),
                ("User", UIConstants.Icons.User),
                ("Add", UIConstants.Icons.Add),
                ("Edit", UIConstants.Icons.Edit),
                ("Delete", UIConstants.Icons.Delete),
                ("Save", UIConstants.Icons.Save),
                ("Cancel", UIConstants.Icons.Cancel),
                ("Undo", UIConstants.Icons.Undo),
                ("Print", UIConstants.Icons.Print),
                ("Report", UIConstants.Icons.Report),
                ("Radio", UIConstants.Icons.Radio),
                ("Close", UIConstants.Icons.Close),
                ("FileText", UIConstants.Icons.FileText),
                ("Import", UIConstants.Icons.Import),
                ("Export", UIConstants.Icons.Export),
                ("List", UIConstants.Icons.List),
                ("Package", UIConstants.Icons.Package),
                ("Money", UIConstants.Icons.Money),
                ("Chart", UIConstants.Icons.Chart),
                ("Success", UIConstants.Icons.Success),
                ("Error", UIConstants.Icons.Error),
                ("Warning", UIConstants.Icons.Warning),
                ("Question", UIConstants.Icons.Question),
                ("Sun", UIConstants.Icons.Sun),
                ("Moon", UIConstants.Icons.Moon),
                ("Eye", UIConstants.Icons.Eye)
            };

            var grid = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = true,
                Width = 1060
            };

            foreach (var (name, icon) in usedIcons)
            {
                var item = new Panel
                {
                    Width = 100,
                    Height = 72,
                    Margin = new Padding(UIConstants.Spacing.Padding.Small, RowMarginVertical, UIConstants.Spacing.Padding.Small, RowMarginVertical)
                };

                var lblIcon = new Label
                {
                    Text = icon,
                    Width = 100,
                    Height = 40,
                    Location = new Point(0, 0),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(UIConstants.Fonts.FontFamily, 22),
                    Cursor = Cursors.Hand,
                    ForeColor = ThemeManager.Instance.TextPrimary
                };
                var tooltip = new ToolTip();
                tooltip.SetToolTip(lblIcon, $"Click để copy: {name}");
                lblIcon.Click += (s, e) =>
                {
                    try
                    {
                        Clipboard.SetText(icon);
                        tooltip.SetToolTip(lblIcon, "Đã copy.");
                    }
                    catch { }
                };

                var lblName = new Label
                {
                    Text = name,
                    Width = 100,
                    Height = 28,
                    Location = new Point(0, 42),
                    TextAlign = ContentAlignment.TopCenter,
                    Font = new Font(UIConstants.Fonts.FontFamily, UIConstants.Fonts.XXSmall),
                    ForeColor = ThemeManager.Instance.TextSecondary
                };

                item.Controls.Add(lblIcon);
                item.Controls.Add(lblName);
                grid.Controls.Add(item);
            }

            return grid;
        }

        private Control CreateButtonsSection()
        {
            var panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false
            };

            var styles = new[]
            {
                (ButtonStyle.Outlined, "Outlined — nền BG, viền Primary"),
                (ButtonStyle.Filled, "Filled — nền Primary"),
                (ButtonStyle.Text, "Text — nền trong suốt"),
                (ButtonStyle.FilledNoOutline, "Filled No Outline"),
                (ButtonStyle.Ghost, "Ghost — trong suốt")
            };

            foreach (var (style, desc) in styles)
            {
                var row = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Margin = new Padding(0, RowMarginVertical, 0, RowMarginVertical)
                };
                var lbl = new Label
                {
                    Text = desc,
                    Width = 280,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = ThemeManager.Instance.FontRegular,
                    ForeColor = ThemeManager.Instance.TextPrimary
                };
                var btnNormal = new CustomButton { Text = "Normal", ButtonStyleType = style, Width = 110 };
                var btnDisabled = new CustomButton { Text = "Disabled", ButtonStyleType = style, Width = 110, Enabled = false };
                row.Controls.Add(lbl);
                row.Controls.Add(btnNormal);
                row.Controls.Add(btnDisabled);
                panel.Controls.Add(row);
            }
            return panel;
        }

        private Control CreateInputsSection()
        {
            var panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false
            };

            var row1 = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Margin = new Padding(0, RowMarginVertical, 0, RowMarginVertical)
            };
            row1.Controls.Add(LabelFor("CustomTextBox:", 140));
            var txt1 = new CustomTextBox { Width = 240, Placeholder = "Nhập văn bản..." };
            var txt2 = new CustomTextBox { Width = 240, Text = "Có giá trị" };
            row1.Controls.Add(txt1);
            row1.Controls.Add(txt2);
            panel.Controls.Add(row1);

            var row2 = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Margin = new Padding(0, RowMarginVertical, 0, RowMarginVertical)
            };
            row2.Controls.Add(LabelFor("CustomComboBox:", 140));
            var combo = new CustomComboBox { Width = 240 };
            combo.Items.AddRange(new[] { "Tùy chọn 1", "Tùy chọn 2", "Tùy chọn 3" });
            combo.SelectedIndex = 0;
            row2.Controls.Add(combo);
            panel.Controls.Add(row2);

            return panel;
        }

        private Label LabelFor(string text, int width)
        {
            return new Label
            {
                Text = text,
                Width = width,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = ThemeManager.Instance.FontRegular,
                ForeColor = ThemeManager.Instance.TextPrimary
            };
        }

        private Control CreateDateTimePickerSection()
        {
            var panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false
            };

            panel.Controls.Add(RowWithLabelAndDtp("dd/MM/yyyy", "dd/MM/yyyy"));
            panel.Controls.Add(RowWithLabelAndDtp("dd/MM/yyyy HH:mm", "dd/MM/yyyy HH:mm"));
            panel.Controls.Add(RowWithLabelAndDtp("HH:mm:ss", "HH:mm:ss"));

            return panel;
        }

        private Control RowWithLabelAndDtp(string labelText, string format)
        {
            var row = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Margin = new Padding(0, RowMarginVertical, 0, RowMarginVertical)
            };
            row.Controls.Add(LabelFor(labelText + ":", 160));
            var dtp = new CustomDateTimePicker { Width = 240, Value = DateTime.Now, CustomFormat = format };
            row.Controls.Add(dtp);
            return row;
        }

        private Control CreateTextAreaSection()
        {
            var panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = false
            };
            panel.Controls.Add(new CustomTextArea { Width = 380, Height = 100, Placeholder = "Nhập văn bản dài..." });
            panel.Controls.Add(new CustomTextArea { Width = 380, Height = 100, Text = "Đoạn văn mẫu\nNhiều dòng\nĐể test TextArea" });
            return panel;
        }

        private Control CreatePanelsSection()
        {
            var container = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = false
            };
            foreach (int radius in new[] { 0, 4, 8, 12, 16 })
            {
                var p = new CustomPanel
                {
                    Width = 140,
                    Height = 90,
                    BorderRadius = radius,
                    Margin = new Padding(UIConstants.Spacing.Padding.Medium)
                };
                var lbl = new Label
                {
                    Text = $"Radius: {radius}px",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = ThemeManager.Instance.TextPrimary
                };
                p.Controls.Add(lbl);
                container.Controls.Add(p);
            }
            return container;
        }

        private Control CreateSpacingSection()
        {
            var panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false
            };
            var spacings = new[]
            {
                ("XXSmall", UIConstants.Spacing.Padding.XXSmall),
                ("XSmall", UIConstants.Spacing.Padding.XSmall),
                ("Small", UIConstants.Spacing.Padding.Small),
                ("Medium", UIConstants.Spacing.Padding.Medium),
                ("Large", UIConstants.Spacing.Padding.Large),
                ("XLarge", UIConstants.Spacing.Padding.XLarge),
                ("XXLarge", UIConstants.Spacing.Padding.XXLarge)
            };
            foreach (var (name, value) in spacings)
            {
                var row = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                Margin = new Padding(0, RowMarginVertical, 0, RowMarginVertical)
            };
            row.Controls.Add(LabelFor($"{name} ({value}px):", 140));
                var box = new Panel
                {
                    Width = Math.Max(20, value * 8),
                    Height = 20,
                    BackColor = ThemeManager.Instance.PrimaryDefault,
                    BorderStyle = BorderStyle.FixedSingle
                };
                row.Controls.Add(box);
                panel.Controls.Add(row);
            }
            return panel;
        }
    }
}
