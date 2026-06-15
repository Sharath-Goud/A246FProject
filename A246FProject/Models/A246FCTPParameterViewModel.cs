using System.Data;

namespace A246FProject.Models
{
    public class A246FCTPParameterViewModel
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


    public class A246FMachines
    {
        public int MachineId { get; set; }
        public string Machine { get; set; }
    }

    public class Line
    {
        public int LineId { get; set; }
        public string LineName { get; set; }
    }

    public class Shift
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
    }

    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime createdTime { get; set; }
    }
    public class ModelNo
    {
        public int ModelId { get; set; }
        public string Model { get; set; }

    }

    public class PartNo
    {
        public int PartId { get; set; }
        public string Part { get; set; }
    }
}