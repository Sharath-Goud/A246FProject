using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class CTQOperatorReportDAL
    {
        private readonly DbClass _db;

        public CTQOperatorReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetCTQOperatorReport(
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
                "ipqc.Rpt_getA246FCTQManPowerCheckList",
                parms);
        }
    }
}
