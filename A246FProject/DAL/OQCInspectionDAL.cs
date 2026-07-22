using System.Data;
using A246FProject.Models;
using Microsoft.Data.SqlClient;

namespace A246FProject.DAL
{
    public class OQCInspectionDAL
    {
        DbClass _db;

        public OQCInspectionDAL()
        {
            _db = DbClass.GetInstance();
        }

        public List<Project> GetProject()
        {
            List<Project> list = new List<Project>();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("[ipqc].[GetProject]");

            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["ProjectId"]) == 31)
                {
                    list.Add(new Project
                    {
                        ProjectId = Convert.ToInt32(dr["ProjectId"]),
                        ProjectName = dr["ProjectName"].ToString()
                    });
                }
            }

            return list;
        }

        public DataTable GetInspectionData(int projectId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ProjectId", projectId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "[ipqc].[GetOQCInspectionData]",
                parameters);
        }

        public void SaveInspection(OQCInspectionViewModel model)
        {
            DataTable dtChecklist = new DataTable();

            dtChecklist.Columns.Add("Id", typeof(int));
            dtChecklist.Columns.Add("ItemId", typeof(int));
            dtChecklist.Columns.Add("SpecId", typeof(int));
            dtChecklist.Columns.Add("ContentId", typeof(int));
            dtChecklist.Columns.Add("ShiftId", typeof(int));
            dtChecklist.Columns.Add("LineId", typeof(int));
            dtChecklist.Columns.Add("ProjectId", typeof(int));
            dtChecklist.Columns.Add("ApprovalId", typeof(int));
            dtChecklist.Columns.Add("CustomerPin", typeof(string));
            dtChecklist.Columns.Add("LotSize", typeof(string));
            dtChecklist.Columns.Add("FinishedProductNo", typeof(string));
            dtChecklist.Columns.Add("Rev", typeof(string));
            dtChecklist.Columns.Add("PackingListNo", typeof(string));
            dtChecklist.Columns.Add("SimToolPartNumber", typeof(string));
            dtChecklist.Columns.Add("InspectResult", typeof(bool));
            dtChecklist.Columns.Add("DocId", typeof(int));
            dtChecklist.Columns.Add("Result", typeof(string));
            dtChecklist.Columns.Add("CreatedBy", typeof(string));
            dtChecklist.Columns.Add("CreatedDateTime", typeof(DateTime));


            foreach (var item in model.Checklist)
            {
                dtChecklist.Rows.Add(
                    item.Id,
                    item.ItemId,
                    item.SpecId,
                    item.ContentId,
                    0,                              
                    0,                              
                    model.ProjectId ?? 0,           
                    0,                              
                    model.CustomerPN ?? "",
                    model.LotSize ?? "",
                    model.FinishedProductNo ?? "",
                    model.Rev ?? "",
                    model.PackingListNo ?? "",
                    model.SimToolPartNumber ?? "", 
                    model.InspectResult,       
                    0,                              
                    item.Result ?? "",
                    model.CreatedBy,
                    DateTime.Now
                );
            }


            SqlParameter[] parameters =
             {
                new SqlParameter
                {
                    ParameterName = "@Checklist",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "ipqc.A246FCheckLIstOutInspection",
                    Value = dtChecklist
                },

                new SqlParameter("@CreatedBy", SqlDbType.NVarChar,20)
                {
                    Value = model.CreatedBy ?? "Admin"
                },

                new SqlParameter("@LineId", SqlDbType.Int)
                {
                    Value = 0
                },

                new SqlParameter("@ProjectId", SqlDbType.Int)
                {
                    Value = model.ProjectId ?? 0
                },

                new SqlParameter("@CheckedBy", SqlDbType.NVarChar,50)
                {
                    Value = model.CheckedBy ?? ""
                },

                new SqlParameter("@ApprovedBy", SqlDbType.NVarChar,50)
                {
                    Value = model.ApprovedBy ?? ""
                },

                new SqlParameter("@CustomerPin", SqlDbType.NVarChar,50)
                {
                    Value = model.CustomerPN ?? ""
                },

                new SqlParameter("@LotSize", SqlDbType.NVarChar,50)
                {
                    Value = model.LotSize ?? ""
                },

                new SqlParameter("@FinishedProductNo", SqlDbType.NVarChar,50)
                {
                    Value = model.FinishedProductNo ?? ""
                },

                new SqlParameter("@Rev", SqlDbType.NVarChar,50)
                {
                    Value = model.Rev ?? ""
                },

                new SqlParameter("@PackingListNo", SqlDbType.NVarChar,50)
                {
                    Value = model.PackingListNo ?? ""
                },

                new SqlParameter("@SimToolPartNumber", SqlDbType.NVarChar)
                {
                    Value = model.SimToolPartNumber ?? ""
                },

                new SqlParameter("@InspectResult", SqlDbType.Bit)
                {
                    Value = model.InspectResult
                }
            };


            _db.ExecuteNonQueryWithParameter(
                "[ipqc].[InsertBulkOQCInspectionData]",
                parameters
            );
        }
    }
}