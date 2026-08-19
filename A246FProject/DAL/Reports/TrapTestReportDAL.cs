using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class TrapTestReportDAL
    {
        private readonly DbClass _db;

        public TrapTestReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetTrapTestReport(
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
                "ipqc.Rpt_getA246FTrapTest",
                parms);
        }
    }
}
