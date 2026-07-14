using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.SerinReports
{
    public class SerinDimensionReportDAL
    {
        private readonly DbClass _db;

        public SerinDimensionReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetDimensionReport(
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
                "ipqc.Rpt_getSerinA246FDimesions",
                parms);
        }
    }
}