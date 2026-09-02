using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Data;

namespace A246FProject.Services
{
    public class OQCPdfService
    {

        public byte[] GenerateOQCReport(DataTable dt)
        {

            var document = Document.Create(container =>
            {

                container.Page(page =>
                {

                    page.Size(PageSizes.A4);

                    page.Margin(35);

                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(8));


                    page.Content()
                        .Column(col =>
                        {

                            col.Item()
                                .Table(table =>
                                {

                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(90);
                                        columns.RelativeColumn();
                                    });

                                    table.Cell()
                                        .Border(1)
                                        .Height(45)
                                        .Padding(5)
                                        .Image("wwwroot/Images/foxlink-logo.png")
                                        .FitArea();

                                    table.Cell()
                                        .Border(1)
                                        .Background("#F5F5F5")
                                        .AlignCenter()
                                        .AlignMiddle()
                                        .Text(text =>
                                        {

                                            text.Line("FOXLINK INDIA ELECTRIC PVT LTD")
                                                .Bold()
                                                .FontSize(12);


                                            text.Line("OQC Outgoing Inspection Record")
                                                .FontSize(9);

                                        });

                                });

                            col.Item()
                                .Height(8);

                            col.Item()
                                .PaddingBottom(6)
                                .Table(table =>
                                {

                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(70);
                                        columns.RelativeColumn();
                                        columns.ConstantColumn(50);
                                        columns.RelativeColumn();
                                    });

                                    table.Cell().Text("Customer:").Bold().FontSize(8);
                                    table.Cell().Text("APPLE").FontSize(8);
                                    table.Cell().AlignRight().Text("REF ID:").Bold().FontSize(8);
                                    table.Cell().Text(dt.Rows[0]["TrackNumber"].ToString()).FontSize(8);

                                });

                            col.Item()
                                .Table(table =>
                                {

                                    table.ColumnsDefinition(columns =>
                                    {

                                        columns.RelativeColumn(1);
                                        columns.RelativeColumn(1.4f);

                                        columns.RelativeColumn(1);
                                        columns.RelativeColumn(1);

                                        columns.RelativeColumn(0.8f);
                                        columns.RelativeColumn(1);

                                    });


                                    LabelCell(table, "Customer P/N");

                                    ValueCell(table,
                                        dt.Rows[0]["CustomerPin"].ToString());

                                    LabelCell(table, "Lot Size");

                                    ValueCell(table,
                                        dt.Rows[0]["LotSize"].ToString());

                                    LabelCell(table, "Date");

                                    ValueCell(table,
                                        Convert.ToDateTime(
                                                dt.Rows[0]["CreatedDateTime"])
                                            .ToString("dd/MM/yyyy"));

                                    LabelCell(table, "Finished Product No.");

                                    ValueCell(table,
                                        dt.Rows[0]["FinishedProductNo"].ToString());

                                    LabelCell(table, "Rev");

                                    ValueCell(table,
                                        dt.Rows[0]["Rev"].ToString());

                                    LabelCell(table, "Packing List No.");

                                    ValueCell(table,
                                        dt.Rows[0]["PackingListNo"].ToString());



                                });

                            col.Item()
                                .Height(5);

                            col.Item()
                                .Border(1)
                                .Padding(6)
                                .Table(table =>
                                {

                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(100);
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });

                                    table.Cell().Text("Inspection method:").Bold().FontSize(8);

                                    table.Cell().AlignMiddle().Text(t =>
                                    {
                                        t.Span("☑ ").FontSize(9);
                                        t.Span("Sampling inspection").FontSize(8);
                                    });

                                    table.Cell().AlignMiddle().Text(t =>
                                    {
                                        t.Span("☐ ").FontSize(9);
                                        t.Span("100% Inspection").FontSize(8);
                                    });

                                    table.Cell().AlignMiddle().Text(t =>
                                    {
                                        t.Span("☐ ").FontSize(9);
                                        t.Span("Others").FontSize(8);
                                    });

                                });

                            col.Item()
                                .PaddingTop(8)

                                .Table(table =>
                                {

                                    table.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(35);

                                        columns.RelativeColumn(1.4f);

                                        columns.RelativeColumn(1.5f);

                                        columns.RelativeColumn(2.5f);

                                        columns.RelativeColumn(0.8f);


                                    });


                                    table.Header(header =>
                                    {

                                        HeaderCell(header, "Item");

                                        HeaderCell(header, "Inspection Items");

                                        HeaderCell(header, "Inspection Spec");

                                        HeaderCell(header, "Inspection Contents");

                                        HeaderCell(header, "Judgement Result");

                                    });


                                    int itemNo = 1;

                                    foreach (DataRow row in dt.Rows)
                                    {


                                        BodyCell(table,
                                            itemNo.ToString());


                                        BodyCell(table,
                                            row["Item"].ToString());


                                        BodyCell(table,
                                            row["Inspecs"].ToString());


                                        BodyCell(table,
                                            row["Contents"].ToString());


                                        ResultCell(table,
                                            row["Result"].ToString());



                                        itemNo++;

                                    }

                                });

                            col.Item()
                                .Height(25);

                            col.Item()
                                .BorderTop(1)
                                .PaddingTop(8)
                                .Table(table =>
                                {

                                    table.ColumnsDefinition(columns =>
                                    {

                                        columns.RelativeColumn(1);

                                        columns.RelativeColumn(1);

                                    });



                                    table.Cell()
                                        .Border(0.5f)
                                        .Padding(5)
                                        .AlignMiddle()
                                        .Text("Outgoing Lots Judgement")
                                        .Bold()
                                        .FontSize(8);



                                    table.Cell()
                                        .Border(0.5f)
                                        .Padding(5)
                                        .AlignCenter()
                                        .AlignMiddle()
                                        .Text("QUALIFIED")
                                        .Bold()
                                        .FontSize(9);



                                });


                            col.Item()
                                .PaddingTop(15)
                                .Table(table =>
                                {


                                    table.ColumnsDefinition(columns =>
                                    {

                                        columns.RelativeColumn();

                                        columns.RelativeColumn();

                                    });

                                    table.Cell()
                                        .Border(0.5f)
                                        .Height(45)
                                        .AlignCenter()
                                        .AlignMiddle()
                                        .Text(text =>
                                        {

                                            text.Line("Approved By")
                                            .Bold()
                                            .FontSize(8);

                                            text.Line(dt.Rows[0]["ApprovedBy"].ToString())
                                                .FontSize(8);


                                        });

                                    table.Cell()
                                        .Border(0.5f)
                                        .Height(45)
                                        .AlignCenter()
                                        .AlignMiddle()
                                        .Text(text =>
                                        {

                                            text.Line("Inspector:")
                                            .Bold()
                                            .FontSize(8);

                                            text.Line(dt.Rows[0]["CheckedBy"].ToString())
                                                .FontSize(8);


                                        });

                                });

                            col.Item()
                                .PaddingTop(10)
                                .AlignRight()
                                .Text("TCO-063")
                                .FontSize(8)
                                .Bold();



                        });


                });


            });


            return document.GeneratePdf();


        }

        private void LabelCell(TableDescriptor table, string text)
        {


            table.Cell()

                .Border(0.5f)

                .Background("#E9E9E9")

                .Padding(4)

                .AlignCenter()

                .AlignMiddle()

                .Text(text)

                .Bold()

                .FontSize(8);


        }


        private void ValueCell(TableDescriptor table, string text)
        {


            table.Cell()

                .Border(0.5f)

                .Padding(4)

                .AlignCenter()

                .AlignMiddle()

                .Text(text)

                .FontSize(8);

        }

        private void HeaderCell(TableCellDescriptor header, string text)
        {
            header.Cell()
                .Border(0.5f)
                .Background("#D9D9D9")
                .Padding(5)
                .AlignCenter()
                .AlignMiddle()
                .Text(text)
                .Bold()
                .FontSize(8);
        }

        private void BodyCell(TableDescriptor table, string text)
        {


            table.Cell()

                .Border(0.5f)

                .MinHeight(25)

                .Padding(4)

                .AlignCenter()

                .AlignMiddle()

                .Text(text)

                .FontSize(7);

        }

        private void ResultCell(TableDescriptor table, string result)
        {

            table.Cell()
                .Border(0.5f)
                .MinHeight(25)
                .Padding(4)
                .AlignCenter()
                .AlignMiddle()
                .Text(result)
                .Bold()
                .FontSize(8);

        }

    }
}