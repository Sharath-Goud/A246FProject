using A246FProject.DAL.Reports;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class GlueWeighingReportBAL
    {
        private readonly GlueWeighingReportDAL _dal;

        public GlueWeighingReportBAL()
        {
            _dal = new GlueWeighingReportDAL();
        }

        public DataTable GetGlueWeighingReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int adhesiveId)
        {
            return _dal.GetGlueWeighingReport(
                fromDate,
                lineId,
                shiftId,
                projectId,
                adhesiveId);
        }
    }
}