using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class OQCInspectionReportBAL
    {
        private readonly OQCInspectionReportDAL _dal;

        public OQCInspectionReportBAL()
        {
            _dal = new OQCInspectionReportDAL();
        }

        public DataTable GetOQCInspectionReport(
        string fromDate,
        string toDate,
        string trackNumber)
        {
            return _dal.GetOQCInspectionReport(
                fromDate,
                toDate,
                trackNumber);
        }
    }
}
