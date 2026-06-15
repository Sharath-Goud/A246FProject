using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL
{
    public class SOPPORDAL
    {
        DbClass _db;

        public SOPPORDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetSOPPORData(
            int lineId,
            int projectId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetSOPPORData",
                parms);
        }

        public List<Result> GetResult()
        {
            List<Result> list = new List<Result>();

            DataTable dt =
                _db.ExecuteProcedureForDataTable(
                    "ipqc.GetResult");

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Result
                {
                    StatusId =
                        Convert.ToInt32(dr["StatusId"]),

                    Status =
                        dr["Status"].ToString()
                });
            }

            return list;
        }
    }
}