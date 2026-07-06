using System.Data;

namespace A246FProject.Models
{
    public class FirstArticleInspectionViewModel
    {
        public int LineId { get; set; }
        public int ProjectId { get; set; }
        public int ModelId { get; set; }
        public int PartId { get; set; }

        public string ProductName { get; set; }
        public string WorkOrder { get; set; }
        public string DrawingVersion { get; set; }
        public string PackVersion { get; set; }
        public int SamplingQty { get; set; }

        public string ProdLineLeader { get; set; }
        public string CheckedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string CreatedBy { get; set; }

        public List<Line> Lines { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
        public List<ModelNo> ModelNos { get; set; } = new();
        public List<PartNo> PartNos { get; set; } = new();

        public DataTable dtInspection { get; set; } = new();

        public List<SaveInspectionDto> CheckListFirstArticleInspections { get; set; } = new();
    }

    public class InspectionDto
    {
        public string StationName { get; set; }
        public string ItemName { get; set; }
        public string Content { get; set; }
        public int ContentId { get; set; }
    }

    public class SaveInspectionDto
    {
        public int ContentId { get; set; }
        public int SRId { get; set; }

        public string Result { get; set; }
        public string RejectDescribe { get; set; }

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