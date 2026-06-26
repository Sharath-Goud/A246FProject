using System.Data;

namespace A246FProject.Models.Reports
{
    public class A246FCTPParameterReportViewModel
    {
        public DataTable dtReports { get; set; }

        public List<Line> Lines { get; set; }

        public List<Project> Projects { get; set; }

        public List<ModelNo> ModelNos { get; set; }

        public List<PartNo> PartNos { get; set; }

        public List<A246FMachines> Machines { get; set; }

        public List<Shift> Shifts { get; set; }

        public int LineId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }

        public int MachineId { get; set; }

        public int ShiftId { get; set; }

        public DateTime? FromDate { get; set; }

        public string ExportType { get; set; }

        public List<string> ExportTypes { get; set; }
    }
}
