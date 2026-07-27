using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class NegativeValidationReportBAL
    {
        NegativeValidationReportDAL _dal =
            new NegativeValidationReportDAL();

        public DataTable GetReport(
            string date,
            int lineId,
            int shiftId,
            int projectId)
        {
            return _dal.GetReport(
                date,
                lineId,
                shiftId,
                projectId);
        }
    }
}