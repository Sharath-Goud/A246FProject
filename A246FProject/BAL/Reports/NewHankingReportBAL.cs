using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class NewHankingReportBAL
    {
        private readonly NewHankingReportDAL _dal;

        public NewHankingReportBAL()
        {
            _dal = new NewHankingReportDAL();
        }

        public DataTable GetNewHankingReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId,
            int partId)
        {
            return _dal.GetNewHankingReport(
                fromDate,
                lineId,
                shiftId,
                projectId,
                machineId,
                partId);
        }
    }
}