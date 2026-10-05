using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace A246FProject.Services
{
    public class FirstArticleDimensionPdfService
    {
        private readonly string _webRoot;

        // pass IWebHostEnvironment.WebRootPath from the controller
        public FirstArticleDimensionPdfService(string webRootPath)
        {
            _webRoot = webRootPath ?? "";
        }

        // Header values come from the search filters, so the PDF header is correct
        // even if the stored procedure does not return them.
        public byte[] GenerateReport(
            DataTable dt,
            string line,
            string shift,
            string date,
            string project,
            string machine)
        {
            if (dt == null || dt.Rows.Count == 0)
                throw new InvalidOperationException("No data available to export.");

            var first = dt.Rows[0];
            string logoPath = Path.Combine(_webRoot, "Images", "foxlink-logo.png");

            // One block per station (same as the screen); first-appearance order is kept
            var groups = dt.Rows.Cast<DataRow>()
                           .GroupBy(r => S(r, "StationName"))
                           .ToList();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(7));

                    // ---------- HEADER ----------
                    page.Header().Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.ConstantColumn(90);
                                c.RelativeColumn();
                            });

                            var logoCell = table.Cell().Border(1).Height(45).Padding(5);
                            if (File.Exists(logoPath))
                                logoCell.Image(File.ReadAllBytes(logoPath)).FitArea();
                            else
                                logoCell.Text("");

                            table.Cell()
                                .Border(1)
                                .Background("#F5F5F5")
                                .AlignCenter()
                                .AlignMiddle()
                                .Text(text =>
                                {
                                    text.AlignCenter();
                                    text.Line("FOXLINK INDIA ELECTRIC PVT LTD").Bold().FontSize(12);
                                    text.Line("First Article Dimension Report").FontSize(9);
                                });
                        });

                        col.Item().Height(6);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(0.8f);
                                c.RelativeColumn(1.2f);
                                c.RelativeColumn(0.8f);
                                c.RelativeColumn(1.2f);
                                c.RelativeColumn(0.8f);
                                c.RelativeColumn(1.2f);
                            });

                            InfoLabel(table, "Line");
                            InfoValue(table, line);
                            InfoLabel(table, "Shift");
                            InfoValue(table, shift);
                            InfoLabel(table, "Date");
                            InfoValue(table, date);

                            InfoLabel(table, "Project");
                            InfoValue(table, project);
                            InfoLabel(table, "Machine");
                            InfoValue(table, machine);
                            InfoLabel(table, "");
                            InfoValue(table, "");
                        });

                        col.Item().Height(6);
                    });

                    // ---------- CONTENT ----------
                    page.Content().Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(1.5f);  // Station Name
                                c.RelativeColumn(1.3f);  // Specification

                                for (int i = 0; i < 5; i++)
                                {
                                    c.ConstantColumn(42);   // Check n
                                    c.RelativeColumn(1f);   // SerialNo n
                                }

                                c.ConstantColumn(45);    // Result
                            });

                            table.Header(h =>
                            {
                                h.Cell().Element(HeadStyle).Text("Station Name").Bold();
                                h.Cell().Element(HeadStyle).Text("Specification").Bold();

                                for (int i = 1; i <= 5; i++)
                                {
                                    h.Cell().Element(HeadStyle).Text("Check " + i).Bold();
                                    h.Cell().Element(HeadStyle).Text("SerialNo" + i).Bold();
                                }

                                h.Cell().Element(HeadStyle).Text("Result").Bold();
                            });

                            foreach (var g in groups)
                            {
                                var rows = g.ToList();
                                uint span = (uint)rows.Count;

                                // merged station cell, once per station
                                table.Cell().RowSpan(span).Element(GroupStyle)
                                     .Text(S(rows[0], "StationName"));

                                foreach (var r in rows)
                                {
                                    table.Cell().Element(BodyStyle).Text(S(r, "Specification"));

                                    for (int i = 1; i <= 5; i++)
                                    {
                                        table.Cell().Element(BodyStyle).AlignCenter()
                                             .Text(Num(r, "Check" + i));
                                        table.Cell().Element(BodyStyle).AlignCenter()
                                             .Text(S(r, "SerialNo" + i));
                                    }

                                    string color = ColorOf(S(r, "Color"), S(r, "Result"));

                                    table.Cell().Element(BodyStyle).AlignCenter()
                                         .Text(S(r, "Result")).Bold().FontColor(color);
                                }
                            }
                        });

                        col.Item().Height(15);

                        // ---------- SIGN-OFF ----------
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn();
                                c.RelativeColumn();
                                c.RelativeColumn();
                                c.RelativeColumn();
                            });

                            SignCell(table, "Checked By", S(first, "CheckedBy"));
                            SignCell(table, "Prod Line Leader", S(first, "ProdLineLeader"));
                            SignCell(table, "IPQC Line Leader", S(first, "IpqcLineLeader"));
                            SignCell(table, "Approved By", S(first, "ApprovedBy"));
                        });
                    });

                    // ---------- FOOTER ----------
                    page.Footer().Row(row =>
                    {
                        row.RelativeItem().Text(t =>
                        {
                            t.Span("Generated: ").Bold().FontSize(7);
                            t.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(7);
                        });

                        row.RelativeItem().AlignRight().Text(t =>
                        {
                            t.Span("Page ").FontSize(7);
                            t.CurrentPageNumber().FontSize(7);
                            t.Span(" / ").FontSize(7);
                            t.TotalPages().FontSize(7);
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }

        // ---------------- helpers ----------------

        private static string ColorOf(string colorName, string result)
        {
            if (!string.IsNullOrWhiteSpace(colorName))
                return colorName.Trim().ToLower() == "red" ? Colors.Red.Medium : Colors.Green.Darken2;

            return string.Equals(result, "NG", StringComparison.OrdinalIgnoreCase)
                ? Colors.Red.Medium
                : Colors.Green.Darken2;
        }

        private static string S(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return "";
            return r[col]?.ToString() ?? "";
        }

        // Same as the screen: numbers shown with 3 decimals (0.000). Empty/NULL stays blank.
        private static string Num(DataRow r, string col)
        {
            string v = S(r, col);
            if (string.IsNullOrWhiteSpace(v)) return "";

            return decimal.TryParse(v, NumberStyles.Any, CultureInfo.CurrentCulture, out var d)
                ? d.ToString("0.000", CultureInfo.InvariantCulture)
                : v;
        }

        private static IContainer HeadStyle(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Darken1)
             .Background("#D9D9D9").Padding(3).AlignCenter().AlignMiddle();

        private static IContainer BodyStyle(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Darken1)
             .MinHeight(20).Padding(3).AlignMiddle();

        private static IContainer GroupStyle(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Darken1)
             .Background("#F0F0F5").Padding(4).AlignMiddle();

        private static void InfoLabel(TableDescriptor table, string text) =>
            table.Cell().Border(0.5f).Background("#E9E9E9")
                 .Padding(4).AlignCenter().AlignMiddle()
                 .Text(text).Bold().FontSize(8);

        private static void InfoValue(TableDescriptor table, string text) =>
            table.Cell().Border(0.5f)
                 .Padding(4).AlignCenter().AlignMiddle()
                 .Text(text ?? "").FontSize(8);

        private static void SignCell(TableDescriptor table, string title, string name) =>
            table.Cell().Border(0.5f).Height(45).AlignCenter().AlignMiddle()
                 .Text(t =>
                 {
                     t.AlignCenter();
                     t.Line(title).Bold().FontSize(8);
                     t.Line(name).FontSize(8);
                 });
    }
}