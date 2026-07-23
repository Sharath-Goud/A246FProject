using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class OQCInspectionReportDAL
    {
        private readonly DbClass _db;

        public OQCInspectionReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetOQCInspectionReport(
        string fromDate,
        string toDate,
        string trackNumber)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date",
                    string.IsNullOrEmpty(fromDate)
                    ? DBNull.Value
                    : (object)fromDate),

                new SqlParameter("@ToDate",
                    string.IsNullOrEmpty(toDate)
                    ? DBNull.Value
                    : (object)toDate),

                new SqlParameter("@TrackNumber",
                    string.IsNullOrEmpty(trackNumber)
                    ? DBNull.Value
                    : (object)trackNumber)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_getOQCInspectionReportData",
                parms);
        }
    }
}
