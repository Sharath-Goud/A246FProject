using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class CCMCHI4SCReportBAL
    {
        private readonly CCMCHI4SCReportDAL _dal;

        public CCMCHI4SCReportBAL()
        {
            _dal = new CCMCHI4SCReportDAL();
        }

        public DataTable GetCCMCHI4SCReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId)
        {
            return _dal.GetCCMCHI4SCReport(
                fromDate,
                lineId,
                shiftId,
                projectId,
                machineId);
        }
    }
}