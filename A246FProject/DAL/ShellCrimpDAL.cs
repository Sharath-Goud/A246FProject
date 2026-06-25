using Microsoft.Data.SqlClient;
using System.Data;
using A246FProject.Models;

namespace A246FProject.DAL
{
    public class ShellCrimpDAL
    {
        DbClass _db;

        public ShellCrimpDAL()
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

        public DataTable GetShellCrimpData(int projectId, int lineId, int machineId)
        {
            SqlParameter[] p =
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@MachineId", machineId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetFromByA246FShellCrimpMC1Data",
                p);
        }

        public int InsertBulkShellCrimp(
            DataTable dt,
            int lineId,
            int projectId,
            int modelId,
            int partId,
            string leader,
            string checkedBy,
            string approvedBy,
            string createdBy)
        {
            SqlParameter checklistParam = new SqlParameter("@Checklist", SqlDbType.Structured)
            {
                TypeName = "ipqc.A246FShellCrimpMC1CheckList2",
                Value = dt
            };

            SqlParameter[] p =
            {
                checklistParam,
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@ModelId", modelId),
                new SqlParameter("@PartId", partId),
                new SqlParameter("@ProdLineLeader", leader),
                new SqlParameter("@CheckedBy", checkedBy),
                new SqlParameter("@ApprovedBy", approvedBy),
                new SqlParameter("@CreatedBy", createdBy)
            };

            return _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertBulkA246FShellCrimpMC1CheckList",
                p);
        }
    }
}