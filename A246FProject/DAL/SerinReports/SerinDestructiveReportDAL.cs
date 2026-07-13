using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.SerinReports
{
    public class SerinDestructiveReportDAL
    {
        private readonly DbClass _db;

        public SerinDestructiveReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetDestructiveReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date", fromDate),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ShiftId", shiftId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@MachineId", machineId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_getSerinDestructiveReport",
                parms);
        }
    }
}