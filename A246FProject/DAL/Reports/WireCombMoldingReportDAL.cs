using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class WireCombMoldingReportDAL
    {
        private readonly DbClass _db;

        public WireCombMoldingReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetWireCombMoldingReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date", fromDate ?? (object)DBNull.Value),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ShiftId", shiftId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@MachineId", machineId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_getParameterA246FWCMMC1CheckList",
                parms);
        }
    }
}
