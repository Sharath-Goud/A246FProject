using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class GlueWeighingReportDAL
    {
        private readonly DbClass _db;

        public GlueWeighingReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetGlueWeighingReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int adhesiveId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date", fromDate ?? (object)DBNull.Value),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ShiftId", shiftId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@adhesiveId", adhesiveId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_GlueWeighingSerinData",
                parms);
        }
    }
}