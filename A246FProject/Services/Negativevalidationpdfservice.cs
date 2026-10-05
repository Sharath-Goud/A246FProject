using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace A246FProject.Services
{
    public class NegativeValidationPdfService
    {
        private readonly string _webRoot;

        // pass IWebHostEnvironment.WebRootPath from the controller
        public NegativeValidationPdfService(string webRootPath)
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
            string project)
        {
            if (dt == null || dt.Rows.Count == 0)
                throw new InvalidOperationException("No data available to export.");

            var first = dt.Rows[0];
            string logoPath = Path.Combine(_webRoot, "Images", "foxlink-logo.png");

            // Group CONSECUTIVE rows with the same Station + Inspection + Frequency.
            // This is the block the screen merges with rowspan, and it keeps the SQL order.
            var groups = new List<List<DataRow>>();
            string lastKey = null;

            foreach (DataRow r in dt.Rows)
            {
                string key = S(r, "StationName") + "|" + S(r, "Inspection") + "|" + S(r, "Frequency");

                if (groups.Count == 0 || key != lastKey)
                    groups.Add(new List<DataRow>());

                groups[groups.Count - 1].Add(r);
                lastKey = key;
            }

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(8));

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
                                    text.Line("Negative Validation Report").FontSize(9);
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
                            InfoLabel(table, "");
                            InfoValue(table, "");
                            InfoLabel(table, "");
                            InfoValue(table, "");
                        });

                        col.Item().Height(6);
                    });

                    // ---------- CONTENT ----------
                    page.Content().Column(col =>
                    {
                        col.Item().PaddingBottom(8).Border(0.5f).BorderColor(Colors.Red.Medium)
                           .Padding(6).Column(n =>
                           {
                               n.Item().Text("NOTE:-").Bold().FontColor(Colors.Red.Darken2);

                               n.Item().Text(t =>
                               {
                                   t.Span("Good Sample indicates good. It must be ");
                                   t.Span("OK").Bold();
                                   t.Span(".");
                               });
                               n.Item().Text(t =>
                               {
                                   t.Span("Good Sample indicates abnormal. It must be ");
                                   t.Span("NG").Bold();
                                   t.Span(".");
                               });
                               n.Item().Text(t =>
                               {
                                   t.Span("Fail Sample indicates good. It must be ");
                                   t.Span("NG").Bold();
                                   t.Span(".");
                               });
                               n.Item().Text(t =>
                               {
                                   t.Span("Fail Sample indicates abnormal. It must be ");
                                   t.Span("OK").Bold();
                                   t.Span(".");
                               });
                           });

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(1.5f);  // Station
                                c.RelativeColumn(1.4f);  // Inspection
                                c.RelativeColumn(1.2f);  // Frequency
                                c.RelativeColumn(1.4f);  // Periods
                                c.RelativeColumn(1f);    // Good Sample
                                c.RelativeColumn(1f);    // Fail Sample
                                c.ConstantColumn(60);    // Result
                            });

                            table.Header(h =>
                            {
                                h.Cell().Element(HeadStyle).Text("Station").Bold();
                                h.Cell().Element(HeadStyle).Text("Inspection").Bold();
                                h.Cell().Element(HeadStyle).Text("Frequency").Bold();
                                h.Cell().Element(HeadStyle).Text("Periods").Bold();
                                h.Cell().Element(HeadStyle).Text("Good Sample").Bold();
                                h.Cell().Element(HeadStyle).Text("Fail Sample").Bold();
                                h.Cell().Element(HeadStyle).Text("Result").Bold();
                            });

                            foreach (var rows in groups)
                            {
                                var head = rows[0];
                                uint span = (uint)rows.Count;

                                // merged cells, once per block
                                table.Cell().RowSpan(span).Element(GroupStyle).Text(S(head, "StationName"));
                                table.Cell().RowSpan(span).Element(GroupStyle).Text(S(head, "Inspection"));
                                table.Cell().RowSpan(span).Element(GroupStyle).Text(S(head, "Frequency"));

                                foreach (var r in rows)
                                {
                                    // same rule as the screen: PASS = green, anything else = red
                                    bool pass = S(r, "Result") == "PASS";
                                    string color = pass ? Colors.Green.Darken2 : Colors.Red.Medium;

                                    table.Cell().Element(BodyStyle).Text(S(r, "Periods"));
                                    table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "GoodSample"));
                                    table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "FailSample"));
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

        // "red"/"green" from the color column; falls back to the result text (NG = red)
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

        private static IContainer HeadStyle(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Darken1)
             .Background("#D9D9D9").Padding(3).AlignCenter().AlignMiddle();

        private static IContainer BodyStyle(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Darken1)
             .MinHeight(22).Padding(3).AlignMiddle();

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