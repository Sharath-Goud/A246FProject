using System.Data;

namespace A246FProject.Models
{
    public class FirstArticleDimensionViewModel
    {

        public int LineId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }


        public string ProductName { get; set; }

        public string WorkOrder { get; set; }

        public string DrawingVersion { get; set; }

        public string PackVersion { get; set; }

        public string SamplingQty { get; set; }


        public string ProdLineLeader { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

        public string CreatedBy { get; set; }



        public List<Line> Lines { get; set; } = new();

        public List<Project> Projects { get; set; } = new();

        public List<ModelNo> ModelNos { get; set; } = new();

        public List<PartNo> PartNos { get; set; } = new();



        public DataTable dtDimension { get; set; } = new();



        public List<SaveDimensionDto> CheckListFirstArticleDimensions { get; set; } = new();

    }



    public class DimensionDto
    {

        public string StationName { get; set; }

        public int SpecId { get; set; }

        public string Specification { get; set; }

        public int TotalCount { get; set; }

    }



    public class SaveDimensionDto
    {

        public int SpecId { get; set; }


        public decimal? Check1 { get; set; }

        public decimal? Check2 { get; set; }

        public decimal? Check3 { get; set; }

        public decimal? Check4 { get; set; }

        public decimal? Check5 { get; set; }



        public string SerialNo1 { get; set; }

        public string SerialNo2 { get; set; }

        public string SerialNo3 { get; set; }

        public string SerialNo4 { get; set; }

        public string SerialNo5 { get; set; }



        public int LineId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }



        public string ProductName { get; set; }

        public string WorkOrder { get; set; }

        public string DrawingVersion { get; set; }

        public string PackVersion { get; set; }

        public string SamplingQty { get; set; }



        public string ProdLineLeader { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

        public string CreatedBy { get; set; }

    }
}