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
                new SqlParameter("@Date",
                    string.IsNullOrEmpty(fromDate)
                    ? DBNull.Value
                    : fromDate),

                new SqlParameter("@lineId",
                    lineId),

                new SqlParameter("@ShiftId",
                    shiftId),

                new SqlParameter("@ProjectId",
                    projectId),

                new SqlParameter("@AdhesiveId",
                    adhesiveId)
            };


            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_GetGlueWeighingReport",
                parms);
        }
    }
}