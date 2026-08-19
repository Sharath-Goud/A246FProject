using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class CTQOperatorReportBAL
    {
        private readonly CTQOperatorReportDAL _dal;

        public CTQOperatorReportBAL()
        {
            _dal = new CTQOperatorReportDAL();
        }

        public DataTable GetCTQOperatorReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId)
        {
            return _dal.GetCTQOperatorReport(fromDate, lineId, shiftId, projectId);
        }
    }
}
