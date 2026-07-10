using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class NewHankingReportDAL
    {
        private readonly DbClass _db;

        public NewHankingReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetNewHankingReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId,
            int partId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date", fromDate ?? (object)DBNull.Value),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ShiftId", shiftId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@MachineId", machineId),
                new SqlParameter("@PartId", partId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_getNewHankingReport",
                parms);
        }
    }
}