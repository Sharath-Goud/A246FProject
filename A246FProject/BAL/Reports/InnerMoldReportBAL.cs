using A246FProject.DAL;
using A246FProject.DAL.Reports;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class InnerMoldReportBAL
    {
        private readonly InnerMoldReportDAL _dal;

        public InnerMoldReportBAL()
        {
            _dal = new InnerMoldReportDAL();
        }

        public DataTable GetInnerMoldReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId)
        {
            return _dal.GetInnerMoldReport(
                fromDate,
                lineId,
                shiftId,
                projectId,
                machineId);
        }
    }
}
