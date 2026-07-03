using A246FProject.DAL;
using A246FProject.DAL.Reports;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class AOIReportBAL
    {
        private readonly AOIReportDAL _dal;

        public AOIReportBAL()
        {
            _dal = new AOIReportDAL();
        }

        public DataTable GetAOIReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId)
        {
            return _dal.GetAOIReport(
                fromDate,
                lineId,
                shiftId,
                projectId,
                machineId);
        }
    }
}
