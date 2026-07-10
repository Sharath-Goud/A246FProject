using System.Data;

namespace A246FProject.Models
{
    public class NewHankingViewModel
    {
        public DataTable dtChecklist { get; set; }

        public List<Line> Lines { get; set; }

        public List<Project> Projects { get; set; }

        public List<ModelNo> ModelNos { get; set; }

        public List<PartNo> PartNos { get; set; }

        public List<A246FMachines> Machines { get; set; }

        public int LineId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }

        public int MachineId { get; set; }

        public string ProdLineLeader { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

    }

    public class NewHankingChecklistItem
    {
        public int SectionId { get; set; }
        public decimal? Value1 { get; set; }
        public decimal? Value2 { get; set; }
        public decimal? Value3 { get; set; }
        public decimal? Value4 { get; set; }
        public decimal? Value5 { get; set; }
    }

    public class SubmitChecklistRequest
    {
        public int LineId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }

        public int MachineId { get; set; }

        public string ProdLineLeader { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

        public List<NewHankingChecklistItem> Items { get; set; } = new List<NewHankingChecklistItem>();
    }
}