using A246FProject.DAL.SerinReports;
using System.Data;

namespace A246FProject.BAL.SerinReports
{
    public class SerinDimensionReportBAL
    {
        private readonly SerinDimensionReportDAL _dal;

        public SerinDimensionReportBAL()
        {
            _dal = new SerinDimensionReportDAL();
        }

        public DataTable GetDimensionReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId)
        {
            return _dal.GetDimensionReport(
                fromDate,
                lineId,
                shiftId,
                projectId,
                machineId);
        }
    }
}