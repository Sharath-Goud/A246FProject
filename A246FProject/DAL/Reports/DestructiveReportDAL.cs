using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class DestructiveReportDAL
    {
        private readonly DbClass _db;

        public DestructiveReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetDestructiveReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date", fromDate),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ShiftId", shiftId),
                new SqlParameter("@ProjectId", projectId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_getDestructiveReport",
                parms);
        }
    }
}