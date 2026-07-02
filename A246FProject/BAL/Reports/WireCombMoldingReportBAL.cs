using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class WireCombMoldingReportBAL
    {
        private readonly WireCombMoldingReportDAL _dal;

        public WireCombMoldingReportBAL()
        {
            _dal = new WireCombMoldingReportDAL();
        }

        public DataTable GetCCMCHI4SCReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId)
        {
            return _dal.GetWireCombMoldingReport(
                fromDate,
                lineId,
                shiftId,
                projectId,
                machineId);
        }
    }
}
