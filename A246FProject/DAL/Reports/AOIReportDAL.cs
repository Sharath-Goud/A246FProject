using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
        public class AOIReportDAL
        {
            private readonly DbClass _db;

            public AOIReportDAL()
            {
                _db = DbClass.GetInstance();
            }

            public DataTable GetAOIReport(
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
                    "ipqc.Rpt_getAOIReport",
                    parms);
            }
        }
}
