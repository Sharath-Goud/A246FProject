using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Data;
using System.IO;

namespace A246FProject.Services
{
    public class NewHankingPdfService
    {
        private readonly string _webRoot;

        // pass IWebHostEnvironment.WebRootPath from the controller
        public NewHankingPdfService(string webRootPath)
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
            string modelName,
            string part,
            string machine)
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
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(7.5f));

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
                                    text.Line("New Hanking Report").FontSize(9);
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
                            InfoLabel(table, "Model");
                            InfoValue(table, modelName);
                            InfoLabel(table, "Part Number");
                            InfoValue(table, part);

                            InfoLabel(table, "Machine");
                            InfoValue(table, machine);
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
                                c.RelativeColumn(1.3f);  // Work Station
                                c.RelativeColumn(1.5f);  // Inspection Project
                                c.RelativeColumn(2.2f);  // Inspection Specifications
                                c.RelativeColumn(1f);    // Frequency
                                c.RelativeColumn(0.9f);  // Section
                                c.ConstantColumn(40);    // Value1
                                c.ConstantColumn(40);    // Value2
                                c.ConstantColumn(40);    // Value3
                                c.ConstantColumn(40);    // Value4
                                c.ConstantColumn(40);    // Value5
                                c.ConstantColumn(55);    // Inspection Results
                            });

                            table.Header(h =>
                            {
                                h.Cell().Element(HeadStyle).Text("Work Station").Bold();
                                h.Cell().Element(HeadStyle).Text("Inspection Project").Bold();
                                h.Cell().Element(HeadStyle).Text("Inspection Specifications").Bold();
                                h.Cell().Element(HeadStyle).Text("Frequency").Bold();
                                h.Cell().Element(HeadStyle).Text("Section").Bold();
                                h.Cell().Element(HeadStyle).Text("Value1").Bold();
                                h.Cell().Element(HeadStyle).Text("Value2").Bold();
                                h.Cell().Element(HeadStyle).Text("Value3").Bold();
                                h.Cell().Element(HeadStyle).Text("Value4").Bold();
                                h.Cell().Element(HeadStyle).Text("Value5").Bold();
                                h.Cell().Element(HeadStyle).Text("Inspection Results").Bold();
                            });

                            foreach (DataRow r in dt.Rows)
                            {
                                // same rule as the screen: NG = red, everything else = green
                                bool ng = S(r, "InspectionResults") == "NG";
                                string color = ng ? Colors.Red.Medium : Colors.Green.Darken2;

                                table.Cell().Element(BodyStyle).Text(S(r, "WorkStation"));
                                table.Cell().Element(BodyStyle).Text(S(r, "InspectionProject"));
                                table.Cell().Element(BodyStyle).Text(S(r, "InspectionSpecifications"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Frequency"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Section"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Value1"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Value2"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Value3"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Value4"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Value5"));
                                table.Cell().Element(BodyStyle).AlignCenter()
                                     .Text(S(r, "InspectionResults")).Bold().FontColor(color);
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