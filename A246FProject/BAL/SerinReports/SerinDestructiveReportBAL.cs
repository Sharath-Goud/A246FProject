
using A246FProject.DAL.SerinReports;
using System.Data;

namespace A246FProject.BAL.SerinReports
{
    public class SerinDestructiveReportBAL
    {
        private readonly SerinDestructiveReportDAL _dal;

        public SerinDestructiveReportBAL()
        {
            _dal = new SerinDestructiveReportDAL();
        }

        public DataTable GetDestructiveReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId)
        {
            return _dal.GetDestructiveReport(
                fromDate,
                lineId,
                shiftId,
                projectId,
                machineId);
        }
    }
}