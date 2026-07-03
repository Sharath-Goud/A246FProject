using A246FProject.DAL;
using A246FProject.DAL.Reports;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class DestructiveReportBAL
    {
        private readonly DestructiveReportDAL _dal;

        public DestructiveReportBAL()
        {
            _dal = new DestructiveReportDAL();
        }

        public DataTable GetDestructiveReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId)
        {
            return _dal.GetDestructiveReport(
                fromDate,
                lineId,
                shiftId,
                projectId);
        }
    }
}
