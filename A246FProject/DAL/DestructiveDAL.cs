using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL
{
    public class DestructiveDAL
    {
        DbClass _db;

        public DestructiveDAL()
        {
            _db = DbClass.GetInstance();
        }

        public List<Line> GetLine()
        {
            List<Line> list = new();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("ipqc.GetLine");

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
            List<Project> list = new();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("ipqc.GetProject");

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
            List<ModelNo> list = new();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("ipqc.GetModelNo");

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
            List<PartNo> list = new();

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
            List<ModelNo> list = new();

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

        public List<A246FMachines> GetMachines()
        {
            List<A246FMachines> list = new();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("ipqc.GetA246FMachines");

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

        public DataTable GetDestructiveData(
            int lineId,
            int projectId,
            int machineId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@MachineId", machineId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetFromByA246FMHB2Data",
                parms);
        }

        public int InsertBulkA246FMHB2CheckList(
            DataTable dtChecklist,
            string userId,
            int lineId,
            string prodLineLeader,
            string checkedBy,
            string approvedBy,
            int modelId,
            int partId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Checklist", dtChecklist),
                new SqlParameter("@CreatedBy", userId),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProdLineLeader", prodLineLeader),
                new SqlParameter("@CheckedBy", checkedBy),
                new SqlParameter("@ApprovedBy", approvedBy),
                new SqlParameter("@ModelId", modelId),
                new SqlParameter("@PartId", partId)
            };

            return _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertBulkA246FMHB2CheckList",
                parms);
        }
    }
}