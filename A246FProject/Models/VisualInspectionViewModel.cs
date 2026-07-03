using System.Data;

namespace A246FProject.Models
{
    public class VisualInspectionViewModel
    {
        public int LineId { get; set; }

        public int ShiftId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }

        public int VisualsId { get; set; }

        public List<Line> Lines { get; set; }

        public List<Shift> Shifts { get; set; }

        public List<Project> Projects { get; set; }

        public List<ModelNo> ModelNos { get; set; }

        public List<PartNo> PartNos { get; set; }

        public List<Visuals> Visualss { get; set; }

        public DataTable dtChecklist { get; set; }

        public string ProdLineLeader { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }
    }


}
