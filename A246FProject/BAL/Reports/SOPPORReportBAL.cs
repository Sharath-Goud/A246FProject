using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class SOPPORReportBAL
    {
        private readonly SOPPORReportDAL _dal;

        public SOPPORReportBAL()
        {
            _dal = new SOPPORReportDAL();
        }

        public DataTable GetSOPPORReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId)
        {
            return _dal.GetSOPPORReport(fromDate, lineId, shiftId, projectId);
        }
    }
}