using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class FirstArticleDimensionReportBAL
    {
        private readonly FirstArticleDimensionReportDAL _dal;

        public FirstArticleDimensionReportBAL()
        {
            _dal = new FirstArticleDimensionReportDAL();
        }

        public DataTable GetFirstArticleDimensionReport(
             string fromDate,
             int lineId,
             int shiftId,
             int projectId)
        {
            return _dal.GetFirstArticleDimensionReport(
                fromDate,
                lineId,
                shiftId,
                projectId);
        }
    }
}
