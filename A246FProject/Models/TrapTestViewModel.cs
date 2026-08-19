using System.Data;

namespace A246FProject.Models
{
    public class TrapTestViewModel
    {
        public DataTable dtChecklist { get; set; } = new DataTable();

        public List<Line> Lines { get; set; } = new List<Line>();

        public List<Project> Projects { get; set; } = new List<Project>();

        public List<ModelNo> ModelNos { get; set; } = new List<ModelNo>();

        public List<PartNo> PartNos { get; set; } = new List<PartNo>();

        public List<Inspector> InspectorSS { get; set; } = new List<Inspector>();

        public int LineId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }

        public int InspecId { get; set; }

        public string ProdLineLeader { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

        public List<TrapTestResult> TrapTestResults { get; set; }
            = new List<TrapTestResult>();

        public string CreatedBy { get; set; }
    }


    public class TrapTestResult
    {
        public int Id { get; set; }

        public int InspectId { get; set; }

        public int InspecId { get; set; }

        public string InspectorName { get; set; }

        public string InspectorId { get; set; }

        public string NoOfCables { get; set; }

        public string CheckResult { get; set; }

        public string SkippedQty { get; set; }

        public string CheckedQty { get; set; }

        public string Sno { get; set; }

        public string Parts { get; set; }

        public string ProjectName { get; set; }
    }


    public class Inspector
    {
        public int InspecId { get; set; }

        public string InspectorName { get; set; }
    }


    public class TrapTestBulkForm
    {
        public int LineId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }

        public string ProdLineLeader { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

        public List<TrapTestResult> TrapTestResults { get; set; }
            = new List<TrapTestResult>();
    }
}