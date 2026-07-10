using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class FirstArticleInspectionReportBAL
    {
        private readonly FirstArticleInspectionReportDAL _dal;

        public FirstArticleInspectionReportBAL()
        {
            _dal = new FirstArticleInspectionReportDAL();
        }

        public DataTable GetFirstArticleInspectionReport(
             string fromDate,
             int lineId,
             int shiftId,
             int projectId)
        {
            return _dal.GetFirstArticleInspectionReport(
                fromDate,
                lineId,
                shiftId,
                projectId);
        }
    }
}