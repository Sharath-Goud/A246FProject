using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL
{
    public class NewHankingDAL
    {
        DbClass _db;

        public NewHankingDAL()
        {
            _db = DbClass.GetInstance();
        }

        public List<Line> GetLine()
        {
            List<Line> list = new List<Line>();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("ipqc.GetLine");

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Line
                {
                    LineId = Convert.ToInt32(dr[0]),
                    LineName = dr[1].ToString()
                });
            }

            return list;
        }

        public List<Project> GetProject()
        {
            List<Project> list = new List<Project>();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("[ipqc].[GetProject]");

            foreach (DataRow dr in dt.Rows)
            {
                // ProjectId = 31 (A246F)
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

        public List<A246FMachines> GetMachines()
        {
            List<A246FMachines> list = new List<A246FMachines>();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("[ipqc].[GetA246CMachines]");

            foreach (DataRow dr in dt.Rows)
            {
                int machineId = Convert.ToInt32(dr["MachineId"]);

                if (machineId >= 1 && machineId <= 5)
                {
                    list.Add(new A246FMachines
                    {
                        MachineId = machineId,
                        Machine = dr["Machine"].ToString()
                    });
                }
            }

            return list;
        }

        public List<ModelNo> GetModelNo()
        {
            List<ModelNo> list = new List<ModelNo>();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("[ipqc].[GetModelNo]");

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new ModelNo
                {
                    ModelId = Convert.ToInt32(dr["ModelId"]),
                    Model = dr["Model"].ToString()
                });
            }

            return list;
        }

        public List<PartNo> GetPartNoByModel(int modelId)
        {
            List<PartNo> list = new List<PartNo>();

            SqlParameter[] parms =
            {
                new SqlParameter("@ModelId", modelId)
            };

            DataTable dt =
                _db.ExecuteProcedureWithParameterForDataTable(
                    "ipqc.GetPartNoByModel",
                    parms);

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new PartNo
                {
                    PartId = Convert.ToInt32(dr["PartId"]),
                    Part = dr["Part"].ToString()
                });
            }

            return list;
        }

        public List<ModelNo> GetModelNoByProject(int projectId)
        {
            List<ModelNo> list = new List<ModelNo>();

            SqlParameter[] parms =
            {
                new SqlParameter("@ProjectId", projectId)
            };

            DataTable dt =
                _db.ExecuteProcedureWithParameterForDataTable(
                    "ipqc.GetModelNoByProject",
                    parms);

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new ModelNo
                {
                    ModelId = Convert.ToInt32(dr["ModelId"]),
                    Model = dr["Model"].ToString()
                });
            }

            return list;
        }

        public DataTable GetNewHankingData(
            int lineId,
            int projectId,
            int machineId,
            int partId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@MachineId", machineId),
                new SqlParameter("@PartId", partId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetFromByA246FNewHankingData",
                parms);
        }

        public int InsertBulkA246FNewHanking(
            List<NewHankingChecklistItem> items,
            string createdBy,
            int lineId,
            int projectId,
            string modelName,
            string prodLineLeader,
            string checkedBy,
            string approvedBy,
            int modelId,
            int partId)
        {
            DataTable dtChecklist = BuildChecklistTable(items);

            SqlParameter checklistParam = new SqlParameter();
            checklistParam.ParameterName = "@Checklist";
            checklistParam.SqlDbType = SqlDbType.Structured;
            checklistParam.TypeName = "ipqc.A246FCMMC1CheckList";
            checklistParam.Value = dtChecklist;

            SqlParameter[] parms =
            {
                checklistParam,
                new SqlParameter("@CreatedBy", (object)createdBy ?? DBNull.Value),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@Model", (object)modelName ?? DBNull.Value),
                new SqlParameter("@ProdLineLeader", (object)prodLineLeader ?? DBNull.Value),
                new SqlParameter("@CheckedBy", (object)checkedBy ?? DBNull.Value),
                new SqlParameter("@ApprovedBy", (object)approvedBy ?? DBNull.Value),
                new SqlParameter("@ModelId", modelId),
                new SqlParameter("@PartId", partId)
            };

            return _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertBulkA246FNewHankingCheckList",
                parms);
        }

        private DataTable BuildChecklistTable(List<NewHankingChecklistItem> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("SectionId", typeof(int));
            dt.Columns.Add("Value1", typeof(decimal));
            dt.Columns.Add("Value2", typeof(decimal));
            dt.Columns.Add("Value3", typeof(decimal));
            dt.Columns.Add("Value4", typeof(decimal));
            dt.Columns.Add("Value5", typeof(decimal));
            dt.Columns.Add("InspectionResults", typeof(string));

            int rowId = 1;
            foreach (var item in items)
            {
                DataRow row = dt.NewRow();
                row["id"] = rowId++;
                row["SectionId"] = item.SectionId;
                row["Value1"] = item.Value1.HasValue ? item.Value1.Value : DBNull.Value;
                row["Value2"] = item.Value2.HasValue ? item.Value2.Value : DBNull.Value;
                row["Value3"] = item.Value3.HasValue ? item.Value3.Value : DBNull.Value;
                row["Value4"] = item.Value4.HasValue ? item.Value4.Value : DBNull.Value;
                row["Value5"] = item.Value5.HasValue ? item.Value5.Value : DBNull.Value;
                row["InspectionResults"] = DBNull.Value; 
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}