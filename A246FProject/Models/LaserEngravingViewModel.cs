using System.Data;

namespace A246FProject.Models
{
    public class LaserEngravingViewModel
    {
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

        public DataTable dtChecklist { get; set; }
        public List<LaserEngravingResult> LaserResults { get; set; }
    }

    public class LaserEngravingResult
    {
        public int LocationId { get; set; }

        public string Value1 { get; set; }
        public string SerialNumber1 { get; set; }

        public string Value2 { get; set; }
        public string SerialNumber2 { get; set; }

        public string Value3 { get; set; }
        public string SerialNumber3 { get; set; }

        public string Value4 { get; set; }
        public string SerialNumber4 { get; set; }

        public string Value5 { get; set; }
        public string SerialNumber5 { get; set; }
    }
}