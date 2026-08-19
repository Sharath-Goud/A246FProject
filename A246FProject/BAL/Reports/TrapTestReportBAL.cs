using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class TrapTestReportBAL
    {
        private readonly TrapTestReportDAL _dal;

        public TrapTestReportBAL()
        {
            _dal = new TrapTestReportDAL();
        }

        public DataTable GetTrapTestReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId)
        {
            return _dal.GetTrapTestReport(fromDate, lineId, shiftId, projectId);
        }
    }
}
