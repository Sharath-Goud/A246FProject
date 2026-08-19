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
            DateTime? fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId,
            int partId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date", SqlDbType.Date)
                {
                    Value = fromDate.HasValue
                        ? fromDate.Value.Date
                        : DBNull.Value
                },

                new SqlParameter("@LineId", SqlDbType.Int)
                {
                    Value = lineId
                },

                new SqlParameter("@ShiftId", SqlDbType.Int)
                {
                    Value = shiftId
                },

                new SqlParameter("@ProjectId", SqlDbType.Int)
                {
                    Value = projectId
                },

                new SqlParameter("@MachineId", SqlDbType.Int)
                {
                    Value = machineId
                },

                new SqlParameter("@PartId", SqlDbType.Int)
                {
                    Value = partId
                }
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_getNewHankingReport",
                parms);
        }
    }
}