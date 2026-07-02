using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class HotBarReportBAL
    {
        private readonly HotBarReportDAL _dal;

        public HotBarReportBAL()
        {
            _dal = new HotBarReportDAL();
        }

        public DataTable GetHotBarReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId)
        {
            return _dal.GetHotBarReport(fromDate, lineId, shiftId, projectId);
        }
    }
}
