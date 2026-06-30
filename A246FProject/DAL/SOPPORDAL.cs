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

        public DataTable GetSOPPORData(int lineId, int projectId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetFormByA246FQualityAudit",
                parms);
        }

        public int InsertSOPPORSingle(
           int lineId,
           int projectId,
           int riskId,
           int statusId,
           string idNumber,
           string createdBy,
           string checkedBy,
           string approvedBy)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@RiskId", riskId),
                new SqlParameter("@StatusId", statusId),
                new SqlParameter("@IdNumber", idNumber),
                new SqlParameter("@CreatedBy", createdBy),
                new SqlParameter("@CheckedBy", checkedBy),
                new SqlParameter("@ApprovedBy", approvedBy)
            };

            return _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertSOPPORSingle",
                parms);
        }

        public int InsertBulkSOPPOR(
           DataTable dtChecklist,
           int lineId,
           int projectId,
           string createdBy,
           string checkedBy,
           string approvedBy)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Checklist", dtChecklist),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@CreatedBy", createdBy),
                new SqlParameter("@CheckedBy", checkedBy),
                new SqlParameter("@ApprovedBy", approvedBy)
            };

            return _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertBulkSOPPOR",
                parms);
        }
    }
}