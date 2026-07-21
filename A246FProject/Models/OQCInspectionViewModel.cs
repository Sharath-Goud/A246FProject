using System.Data;

namespace A246FProject.Models
{
    public class OQCInspectionViewModel
    {
        public OQCInspectionViewModel()
        {
            Projects = new List<Project>();
            dtChecklist = new DataTable();
        }

        public int? ProjectId { get; set; }

        public List<Project> Projects { get; set; }

        public DataTable dtChecklist { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

        public string CustomerPN { get; set; }

        public string LotSize { get; set; }

        public string FinishedProductNo { get; set; }

        public string Rev { get; set; }

        public string PackingListNo { get; set; }

        public string SimToolPartNumber { get; set; }
    }
}
