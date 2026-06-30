using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class CCMCHI4SCReportDAL
    {
        private readonly DbClass _db;

        public CCMCHI4SCReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetCCMCHI4SCReport(
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
                "ipqc.Rpt_getParameterA246FCMMC1CheckList",
                parms);
        }
    }
}