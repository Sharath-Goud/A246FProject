using System.Data;

namespace A246FProject.Models.Reports
{
    public class OQCInspectionReportViewModel
    {
        public DataTable dtReports { get; set; } = new DataTable();

        public string TrackNumber { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string ExportType { get; set; }

        public List<string> ExportTypes { get; set; } = new();
    }
}