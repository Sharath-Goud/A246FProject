using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class NegativeValidationReportDAL
    {
        DbClass _db = DbClass.GetInstance();

        public DataTable GetReport(
            string date,
            int lineId,
            int shiftId,
            int projectId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date", date),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ShiftId", shiftId),
                new SqlParameter("@ProjectId", projectId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_getA246FNativeValidationData",
                parms);
        }
    }
}