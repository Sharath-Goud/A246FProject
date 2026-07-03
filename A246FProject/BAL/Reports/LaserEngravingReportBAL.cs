using A246FProject.DAL;
using A246FProject.DAL.Reports;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class LaserEngravingReportBAL
    {
        private readonly LaserEngravingReportDAL _dal;

        public LaserEngravingReportBAL()
        {
            _dal = new LaserEngravingReportDAL();
        }

        public DataTable GetLaserEngravingReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId)
        {
            return _dal.GetLaserEngravingReport(
                fromDate,
                lineId,
                shiftId,
                projectId);
        }
    }
}
