using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL
{
    public class NegativeValidationDAL
    {
        DbClass _db;

        public NegativeValidationDAL()
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

        public DataTable GetNegativeValidationData(int lineId, int projectId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetA246FNegativeValidation",
                parms);
        }

        public int InsertBulkNegativeValidation(
             DataTable dtChecklist,
             int lineId,
             int projectId,
             string leader,
             string checkedBy,
             string approvedBy,
             int modelId,
             int partId,
             string createdBy)
        {
            SqlParameter checklist = new SqlParameter("@Checklist", dtChecklist);
            checklist.SqlDbType = SqlDbType.Structured;
            checklist.TypeName = "ipqc.A246FNegativeValidationTypo";

            SqlParameter[] parms =
            {
                checklist,

                new SqlParameter("@CreatedBy", createdBy),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@ProdLineLeader", leader),
                new SqlParameter("@CheckedBy", checkedBy),
                new SqlParameter("@ApprovedBy", approvedBy),
                new SqlParameter("@ModelId", modelId),
                new SqlParameter("@PartId", partId)
            };

            return _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertBulkA246FNativeValidation",
                parms);
        }
    }
}
