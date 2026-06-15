using System.Data;

namespace A246FProject.Models
{
    public class SOPPORViewModel
    {
        public DataTable dtChecklist { get; set; }

        public List<Line> Lines { get; set; }

        public List<Project> Projects { get; set; }

        public List<ModelNo> ModelNos { get; set; }

        public List<PartNo> PartNos { get; set; }

        public List<Result> Statuses { get; set; }

        public int LineId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }

        public int StatusId { get; set; }

        public string ProdLineLeader { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

        public SOPPORResult[] SOPPORResults { get; set; }
    }

    public class SOPPORResult
    {
        public int RiskId { get; set; }

        public int StatusId { get; set; }

        public string Namee { get; set; }

        public string Image { get; set; }
    }

    public class Result
    {
        public int StatusId { get; set; }

        public string Status { get; set; }
    }
}
