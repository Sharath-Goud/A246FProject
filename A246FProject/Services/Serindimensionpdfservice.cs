using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace A246FProject.Services
{
    public class SerinDimensionPdfService
    {
        private readonly string _webRoot;

        // pass IWebHostEnvironment.WebRootPath from the controller
        public SerinDimensionPdfService(string webRootPath)
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

            // Same columns as the screen: every column of the result except "color"
            var columns = new List<DataColumn>();
            foreach (DataColumn c in dt.Columns)
            {
                if (!string.Equals(c.ColumnName, "color", StringComparison.OrdinalIgnoreCase))
                    columns.Add(c);
            }

            int n = columns.Count;
            bool hasColor = dt.Columns.Contains("color");

            // Many columns -> bigger page and smaller font so everything stays readable
            bool useA3 = n > 14;
            float fontSize = n <= 10 ? 8f : (n <= 14 ? 7f : 6.5f);

            // Column widths follow the longest text in each column (header or values)
            var weights = new List<float>();
            foreach (var col in columns)
            {
                int max = col.ColumnName.Length;
                int checkedRows = 0;

                foreach (DataRow r in dt.Rows)
                {
                    int len = Text(r, col).Length;
                    if (len > max) max = len;
                    if (++checkedRows >= 200) break;
                }

                weights.Add(Math.Clamp(max, 6, 28));
            }

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(useA3 ? PageSizes.A3.Landscape() : PageSizes.A4.Landscape());
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(fontSize));

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
                                    text.Line("Serin Dimension Report").FontSize(9);
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
                                foreach (var w in weights)
                                    c.RelativeColumn(w);
                            });

                            table.Header(h =>
                            {
                                foreach (var colDef in columns)
                                    h.Cell().Element(HeadStyle).Text(colDef.ColumnName).Bold();
                            });

                            foreach (DataRow r in dt.Rows)
                            {
                                // same as the screen: DataValue and RootCause use the "color" column
                                string rowColor = hasColor ? ColorOf(Text(r, dt.Columns["color"])) : null;

                                foreach (var colDef in columns)
                                {
                                    bool coloured =
                                        rowColor != null &&
                                        (colDef.ColumnName == "DataValue" || colDef.ColumnName == "RootCause");

                                    var cell = table.Cell().Element(BodyStyle).Text(Text(r, colDef));

                                    if (coloured)
                                        cell.Bold().FontColor(rowColor);
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

        // colour names coming from the stored procedure ("red", "green", or a #hex value)
        private static string ColorOf(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            name = name.Trim();

            if (name.StartsWith("#")) return name;

            switch (name.ToLower())
            {
                case "red": return Colors.Red.Medium;
                case "green": return Colors.Green.Darken2;
                case "orange": return Colors.Orange.Darken2;
                case "blue": return Colors.Blue.Medium;
                default: return null;
            }
        }

        private static string Text(DataRow r, DataColumn c)
        {
            var v = r[c];
            return v == DBNull.Value || v == null ? "" : v.ToString() ?? "";
        }

        private static string S(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return "";
            return r[col]?.ToString() ?? "";
        }

        private static IContainer HeadStyle(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Darken1)
             .Background("#D9D9D9").Padding(3).AlignCenter().AlignMiddle();

        private static IContainer BodyStyle(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Darken1)
             .MinHeight(20).Padding(3).AlignMiddle();

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