using System.Data;

namespace A246FProject.Models
{
    public class OQCInspectionViewModel
    {
        public OQCInspectionViewModel()
        {
            Projects = new List<Project>();
            dtChecklist = new DataTable();
            Checklist = new List<OQCInspectionItem>();
        }

        public int? ProjectId { get; set; }

        public List<Project> Projects { get; set; }

        public DataTable dtChecklist { get; set; }

        public List<OQCInspectionItem> Checklist { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

        public string CustomerPN { get; set; }

        public string LotSize { get; set; }

        public string FinishedProductNo { get; set; }

        public string Rev { get; set; }

        public string PackingListNo { get; set; }

        public string SimToolPartNumber { get; set; }

        public bool SamplingInspection { get; set; }

        public bool HundredPercentInspection { get; set; }

        public bool OtherInspection { get; set; }

        public bool InspectResult { get; set; }
        public string CreatedBy { get; set; }
    }

    public class OQCInspectionItem
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int SpecId { get; set; }

        public int ContentId { get; set; }

        public string Item { get; set; }

        public string Inspecs { get; set; }

        public string Contents { get; set; }

        public string Result { get; set; }
    }
}