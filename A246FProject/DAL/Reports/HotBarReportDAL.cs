using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class HotBarReportDAL
    {
        private readonly DbClass _db;

        public HotBarReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetHotBarReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date", fromDate ?? (object)DBNull.Value),
                new SqlParameter("@lineId", lineId),
                new SqlParameter("@ShiftId", shiftId),
                new SqlParameter("@ProjectId", projectId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_Rpt_getA246FHotBarReport",
                parms);
        }
    }
}
