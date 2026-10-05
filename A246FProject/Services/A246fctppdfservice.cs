using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace A246FProject.Services
{
    public class A246FCTPPdfService
    {
        private const string LogoPath = "wwwroot/Images/foxlink-logo.png";

        public byte[] GenerateReport(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                throw new InvalidOperationException("No data available to export.");

            var first = dt.Rows[0];

            // Same grouping as the screen: one block per station, original SQL order kept
            var groups = dt.Rows.Cast<DataRow>()
                           .GroupBy(r => r["StationId"])
                           .ToList();

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
                            if (File.Exists(LogoPath))
                                logoCell.Image(LogoPath).FitArea();
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
                                    text.Line("A246F CTP Parameter Report").FontSize(9);
                                });
                        });

                        col.Item().Height(6);

                        // Filter / info block (Line, Shift, Date, Project, Machine, Part)
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
                            InfoValue(table, S(first, "LineName"));
                            InfoLabel(table, "Shift");
                            InfoValue(table, S(first, "ShiftName"));
                            InfoLabel(table, "Date");
                            InfoValue(table, D(first, "CreatedDateTime"));

                            InfoLabel(table, "Project");
                            InfoValue(table, S(first, "ProjectName"));
                            InfoLabel(table, "Machine");
                            InfoValue(table, S(first, "Machine"));
                            InfoLabel(table, "Part");
                            InfoValue(table, S(first, "Part"));
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
                                c.ConstantColumn(45);   // Station No
                                c.RelativeColumn(2.2f); // Station
                                c.RelativeColumn(1.8f); // Equipment
                                c.RelativeColumn(2.4f); // Parameter
                                c.ConstantColumn(40);   // Unit
                                c.ConstantColumn(48);   // LSL
                                c.ConstantColumn(48);   // USL
                                c.ConstantColumn(52);   // Value
                                c.ConstantColumn(42);   // Result
                            });

                            table.Header(h =>
                            {
                                h.Cell().Element(HeadStyle).Text("Station No").Bold();
                                h.Cell().Element(HeadStyle).Text("Station").Bold();
                                h.Cell().Element(HeadStyle).Text("Equipment Name/Model").Bold();
                                h.Cell().Element(HeadStyle).Text("Parameter Items").Bold();
                                h.Cell().Element(HeadStyle).Text("Unit").Bold();
                                h.Cell().Element(HeadStyle).Text("LSL").Bold();
                                h.Cell().Element(HeadStyle).Text("USL").Bold();
                                h.Cell().Element(HeadStyle).Text("Value").Bold();
                                h.Cell().Element(HeadStyle).Text("Result").Bold();
                            });

                            foreach (var g in groups)
                            {
                                var rows = g.ToList();
                                var head = rows[0];
                                uint span = (uint)rows.Count;

                                // merged cells, once per station
                                table.Cell().RowSpan(span).Element(StationStyle)
                                     .Text(S(head, "StationNo"));
                                table.Cell().RowSpan(span).Element(StationStyle)
                                     .Text(S(head, "StationName"));
                                table.Cell().RowSpan(span).Element(StationStyle)
                                     .Text(S(head, "EquipmentName"));

                                foreach (var r in rows)
                                {
                                    bool ng = string.Equals(S(r, "Result"), "NG",
                                                            StringComparison.OrdinalIgnoreCase);
                                    string color = ng ? Colors.Red.Medium : Colors.Green.Darken2;

                                    table.Cell().Element(BodyStyle).Text(S(r, "ParameterName"));
                                    table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Unit"));
                                    table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "LSL"));
                                    table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "USL"));
                                    table.Cell().Element(BodyStyle).AlignCenter()
                                         .Text(S(r, "Value")).Bold().FontColor(color);
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

        private static string S(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return "";
            return r[col]?.ToString() ?? "";
        }

        private static string D(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return "";
            return Convert.ToDateTime(r[col]).ToString("dd/MM/yyyy");
        }

        private static IContainer HeadStyle(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Darken1)
             .Background("#D9D9D9").Padding(4).AlignCenter().AlignMiddle();

        private static IContainer BodyStyle(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Darken1)
             .MinHeight(20).Padding(3).AlignMiddle();

        private static IContainer StationStyle(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Darken1)
             .Background("#F0F0F5").Padding(4).AlignMiddle();

        private static void InfoLabel(TableDescriptor table, string text) =>
            table.Cell().Border(0.5f).Background("#E9E9E9")
                 .Padding(4).AlignCenter().AlignMiddle()
                 .Text(text).Bold().FontSize(8);

        private static void InfoValue(TableDescriptor table, string text) =>
            table.Cell().Border(0.5f)
                 .Padding(4).AlignCenter().AlignMiddle()
                 .Text(text).FontSize(8);

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