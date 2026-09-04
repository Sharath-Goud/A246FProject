using Microsoft.Data.SqlClient;
using System.Data;
using A246FProject.Models;

namespace A246FProject.DAL
{
    public class AOIDAL
    {
        DbClass _db;

        public AOIDAL()
        {
            _db = DbClass.GetInstance();
        }

        public List<Line> GetLine()
        {
            DataTable dt = _db.ExecuteProcedureForDataTable("ipqc.GetLine");
            List<Line> list = new();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Line
                {
                    LineId = Convert.ToInt32(dr["LineId"]),
                    LineName = dr["LineName"].ToString()
                });
            }
            return list;
        }

        public List<Project> GetProject()
        {
            DataTable dt = _db.ExecuteProcedureForDataTable("ipqc.GetProject");

            List<Project> list = new();

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

        public List<ModelNo> GetModelNo()
        {
            DataTable dt = _db.ExecuteProcedureForDataTable("ipqc.GetModelNo");

            List<ModelNo> list = new();

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
            SqlParameter[] p =
            {
                new SqlParameter("@ModelId", modelId)
            };

            DataTable dt = _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetPartNoByModel", p);

            List<PartNo> list = new();

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

        public List<A246FMachines> GetMachines()
        {
            DataTable dt = _db.ExecuteProcedureForDataTable("ipqc.GetA246FMachines");

            List<A246FMachines> list = new();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new A246FMachines
                {
                    MachineId = Convert.ToInt32(dr["MachineId"]),
                    Machine = dr["Machine"].ToString()
                });
            }

            return list;
        }

        public List<ModelNo> GetModelNoByProject(int projectId)
        {
            SqlParameter[] p =
            {
                new SqlParameter("@ProjectId", projectId)
            };

            DataTable dt = _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetModelNoByProject",
                p);

            List<ModelNo> list = new();

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

        public DataTable GetAOIData(int projectId, int lineId, int machineId)
        {
            SqlParameter[] p =
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@MachineId", machineId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetFromByA246FAOIData",
                p);
        }

        public int InsertBulkAOI(
    DataTable dtChecklist,
    string createdBy,
    int lineId,
    int projectId,
    string prodLineLeader,
    string checkedBy,
    string approvedBy,
    int modelId,
    int partId)
        {
            int result = 0;

            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Checklist", SqlDbType.Structured)
                    {
                        TypeName = "ipqc.A246FAOICheckList",
                        Value = dtChecklist
                    },

                    new SqlParameter("@CreatedBy", createdBy),

                    new SqlParameter("@LineId", lineId),

                    new SqlParameter("@ProjectId", projectId),

                    new SqlParameter("@Model", DBNull.Value),

                    new SqlParameter("@ProdLineLeader", prodLineLeader),

                    new SqlParameter("@CheckedBy", checkedBy),

                    new SqlParameter("@ApprovedBy", approvedBy),

                    new SqlParameter("@ModelId", modelId),

                    new SqlParameter("@PartId", partId)
                };

                result = _db.ExecuteNonQueryWithParameter(
                    "ipqc.InsertBulkA246FAOICheckList",
                    parameters);
            }
            catch
            {
                throw;
            }

            return result;
        }

    }
}