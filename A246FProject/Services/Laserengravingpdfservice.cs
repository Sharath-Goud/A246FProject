using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Data;
using System.IO;

namespace A246FProject.Services
{
    public class LaserEngravingPdfService
    {
        private readonly string _webRoot;

        // pass IWebHostEnvironment.WebRootPath from the controller
        public LaserEngravingPdfService(string webRootPath)
        {
            _webRoot = webRootPath ?? "";
        }

        // Header values (line, shift, date, project) come from the search filters,
        // so the PDF header is correct even if the stored procedure does not return them.
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

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(6.5f));

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
                                    text.Line("Laser Engraving Report").FontSize(9);
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
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(1.3f);  // Locations
                                c.RelativeColumn(1.5f);  // Specification
                                c.ConstantColumn(32);    // Min
                                c.ConstantColumn(32);    // Max
                                c.RelativeColumn(0.8f);  // Frequency

                                for (int i = 0; i < 5; i++)
                                {
                                    c.ConstantColumn(34);   // Value n
                                    c.RelativeColumn(0.9f); // SerialNumber n
                                }

                                c.ConstantColumn(48);    // Inspection Result
                            });

                            table.Header(h =>
                            {
                                h.Cell().Element(HeadStyle).Text("Locations").Bold();
                                h.Cell().Element(HeadStyle).Text("Specification").Bold();
                                h.Cell().Element(HeadStyle).Text("Min").Bold();
                                h.Cell().Element(HeadStyle).Text("Max").Bold();
                                h.Cell().Element(HeadStyle).Text("Frequency").Bold();

                                for (int i = 1; i <= 5; i++)
                                {
                                    h.Cell().Element(HeadStyle).Text("Value" + i).Bold();
                                    h.Cell().Element(HeadStyle).Text("SerialNumber" + i).Bold();
                                }

                                h.Cell().Element(HeadStyle).Text("Inspection Result").Bold();
                            });

                            foreach (DataRow r in dt.Rows)
                            {
                                // same as the screen: colour comes from the "color" column
                                string color = ColorOf(S(r, "color"), S(r, "Result"));

                                table.Cell().Element(BodyStyle).Text(S(r, "Locations"));
                                table.Cell().Element(BodyStyle).Text(S(r, "Specification"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Min"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Max"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Frequency"));

                                for (int i = 1; i <= 5; i++)
                                {
                                    table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Value" + i));
                                    table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "SerialNumber" + i));
                                }

                                table.Cell().Element(BodyStyle).AlignCenter()
                                     .Text(S(r, "Result")).Bold().FontColor(color);
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

        // "red"/"green" from the Color column; falls back to the result text (NG = red)
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
             .Background("#D9D9D9").Padding(4).AlignCenter().AlignMiddle();

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