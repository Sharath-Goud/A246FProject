using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class FirstArticleDimensionReportDAL
    {
        private readonly DbClass _db;

        public FirstArticleDimensionReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetFirstArticleDimensionReport(
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
                "ipqc.Rpt_getFirstAirticleDimension",
                parms);
        }
    }
}