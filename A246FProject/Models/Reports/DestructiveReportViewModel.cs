using System.Data;

namespace A246FProject.Models.Reports
{
    public class DestructiveReportViewModel
    {
        public DataTable dtReports { get; set; } = new DataTable();

        public List<Line> Lines { get; set; } = new();

        public List<Project> Projects { get; set; } = new();

        public List<ModelNo> ModelNos { get; set; } = new();

        public List<PartNo> PartNos { get; set; } = new();

        public List<A246FMachines> Machines { get; set; } = new();

        public List<Shift> Shifts { get; set; } = new();

        public int LineId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }

        public int MachineId { get; set; }

        public int ShiftId { get; set; }

        public DateTime? FromDate { get; set; }

        public string ExportType { get; set; }

        public List<string> ExportTypes { get; set; } = new();
    }
}
