using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL
{
    public class TrapTestDAL
    {
        private readonly DbClass _db;

        public TrapTestDAL()
        {
            _db = DbClass.GetInstance();
        }

        public DataTable GetTrapTestData(
            int lineId,
            int projectId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.A246FGetFormByTrapTest",
                parms);
        }

        public int InsertBulkTrapTest(
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
            if (dtChecklist == null || dtChecklist.Rows.Count == 0)
            {
                return 0;
            }

            SqlParameter checklistParameter =
                new SqlParameter("@Checklist", SqlDbType.Structured);

            checklistParameter.TypeName =
                "ipqc.A246FTrapTestCheckList";

            checklistParameter.Value = dtChecklist;

            SqlParameter[] parms =
            {
        checklistParameter,

        new SqlParameter(
            "@CreatedBy",
            string.IsNullOrWhiteSpace(createdBy)
                ? "Admin"
                : createdBy),

        new SqlParameter(
            "@LineId",
            lineId),

        new SqlParameter(
            "@ProjectId",
            projectId),

        new SqlParameter(
            "@ProdLineLeader",
            string.IsNullOrWhiteSpace(leader)
                ? ""
                : leader),

        new SqlParameter(
            "@CheckedBy",
            string.IsNullOrWhiteSpace(checkedBy)
                ? ""
                : checkedBy),

        new SqlParameter(
            "@ApprovedBy",
            string.IsNullOrWhiteSpace(approvedBy)
                ? ""
                : approvedBy),

        new SqlParameter(
            "@ModelId",
            modelId),

        new SqlParameter(
            "@PartId",
            partId)
    };

            return _db.ExecuteNonQueryWithParameter(
                "ipqc.A246FInsertBulkTrapTest",
                parms);
        }
    }
}