using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Data;
using System.IO;

namespace A246FProject.Services
{
    public class SOPPORPdfService
    {
        private readonly string _webRoot;

        // pass IWebHostEnvironment.WebRootPath from the controller
        public SOPPORPdfService(string webRootPath)
        {
            _webRoot = webRootPath ?? "";
        }

        public byte[] GenerateReport(DataTable dt)
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
                                    text.Line("SOP/POR Report").FontSize(9);
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
                            InfoValue(table, S(first, "LineName"));
                            InfoLabel(table, "Shift");
                            InfoValue(table, S(first, "ShiftName"));
                            InfoLabel(table, "Date");
                            InfoValue(table, D(first, "CreatedDateTime"));

                            InfoLabel(table, "Project");
                            InfoValue(table, S(first, "ProjectName"));
                            InfoLabel(table, "Model");
                            InfoValue(table, S(first, "Model"));
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
                                c.ConstantColumn(45);    // Process No
                                c.RelativeColumn(1.6f);  // Module
                                c.RelativeColumn(2.4f);  // Critical Risk Points
                                c.RelativeColumn(1.1f);  // Sampling Frequency
                                c.RelativeColumn(1.1f);  // Responsible Department
                                c.ConstantColumn(45);    // Status
                                c.RelativeColumn(1.2f);  // Name & ID
                                c.ConstantColumn(72);    // Image
                                c.RelativeColumn(1.3f);  // Root Cause
                                c.RelativeColumn(1.3f);  // Corrective Action
                            });

                            table.Header(h =>
                            {
                                h.Cell().Element(HeadStyle).Text("Process No").Bold();
                                h.Cell().Element(HeadStyle).Text("Module").Bold();
                                h.Cell().Element(HeadStyle).Text("Critical Risk Points").Bold();
                                h.Cell().Element(HeadStyle).Text("Sampling Frequency").Bold();
                                h.Cell().Element(HeadStyle).Text("Responsible Department").Bold();
                                h.Cell().Element(HeadStyle).Text("STATUS").Bold();
                                h.Cell().Element(HeadStyle).Text("Name & ID").Bold();
                                h.Cell().Element(HeadStyle).Text("Image").Bold();
                                h.Cell().Element(HeadStyle).Text("Root Cause").Bold();
                                h.Cell().Element(HeadStyle).Text("Corrective Action").Bold();
                            });

                            foreach (DataRow r in dt.Rows)
                            {
                                bool ng = string.Equals(S(r, "Status"), "NG",
                                                        StringComparison.OrdinalIgnoreCase);
                                string color = ng ? Colors.Red.Medium : Colors.Green.Darken2;

                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "ProcessNo"));
                                table.Cell().Element(BodyStyle).Text(S(r, "Module"));
                                table.Cell().Element(BodyStyle).Text(S(r, "Points"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "SamplingUnits"));
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Dept"));
                                table.Cell().Element(BodyStyle).AlignCenter()
                                     .Text(S(r, "Status")).Bold().FontColor(color);
                                table.Cell().Element(BodyStyle).AlignCenter().Text(S(r, "Namee"));

                                // image
                                var img = LoadImage(S(r, "Image"));
                                var imgCell = table.Cell().Element(BodyStyle).AlignCenter();
                                if (img != null)
                                    imgCell.Width(62).Height(42).Image(img).FitArea();
                                else
                                    imgCell.Text("-");

                                table.Cell().Element(BodyStyle).Text(S(r, "RootCauseCategory"));
                                table.Cell().Element(BodyStyle).Text(S(r, "FailureMode"));
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

        // ---------------- image loading ----------------

        // Handles: data:image base64 strings, web paths like /uploads/a.jpg or ~/uploads/a.jpg
        // (resolved under wwwroot), and full physical paths. Returns null if not found.
        private byte[] LoadImage(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;

            try
            {
                raw = raw.Trim();

                if (raw.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
                {
                    int i = raw.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
                    return i >= 0 ? Convert.FromBase64String(raw.Substring(i + 7)) : null;
                }

                string rel = raw.Replace("~", "")
                                .TrimStart('/', '\\')
                                .Replace('/', Path.DirectorySeparatorChar);

                string full = Path.Combine(_webRoot, rel);
                if (File.Exists(full)) return File.ReadAllBytes(full);

                if (File.Exists(raw)) return File.ReadAllBytes(raw);
            }
            catch
            {
                // ignore and fall through: the cell will show "-"
            }

            return null;
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
             .MinHeight(24).Padding(3).AlignMiddle();

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