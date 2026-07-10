using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class FirstArticleInspectionReportDAL
    {
        private readonly DbClass _db;

        public FirstArticleInspectionReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetFirstArticleInspectionReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date", fromDate ?? (object)DBNull.Value),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ShiftId", shiftId),
                new SqlParameter("@ProjectId", projectId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_getA246FFirstArticleInspectionReport",
                parms);
        }
    }
}