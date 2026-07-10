using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL
{
    public class MasterDAL
    {
        DbClass _db;

        public MasterDAL()
        {
            _db = DbClass.GetInstance();
        }

        #region Line

        public List<Line> GetLine()
        {
            List<Line> list = new();

            DataTable dt = _db.ExecuteProcedureForDataTable("ipqc.GetLine");

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

        #endregion

        public List<Visuals> GetVisuals(int projectId)
        {
            SqlParameter[] p =
            {
            new SqlParameter("@ProjectId",projectId)
        };

            DataTable dt =
                _db.ExecuteProcedureWithParameterForDataTable(
                    "ipqc.GetA246FVisualsByproject",
                    p);

            List<Visuals> list = new();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Visuals
                {
                    VisualsId = Convert.ToInt32(dr["VisualsId"]),
                    Visual = dr["Visual"].ToString()
                });
            }

            return list;
        }

        #region Shift

        public List<Shift> GetShift()
        {
            List<Shift> list = new();

            DataTable dt = _db.ExecuteProcedureForDataTable("ipqc.GetShift");

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Shift
                {
                    ShiftId = Convert.ToInt32(dr["ShiftId"]),
                    ShiftName = dr["ShiftName"].ToString()
                });
            }

            return list;
        }

        #endregion

        #region Project

        public List<Project> GetProject()
        {
            List<Project> list = new();

            DataTable dt = _db.ExecuteProcedureForDataTable("ipqc.GetProject");

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

        #endregion

        #region Machine

        public List<A246FMachines> GetA246FMachines()
        {
            List<A246FMachines> list = new();

            DataTable dt = _db.ExecuteProcedureForDataTable("ipqc.GetA246CMachines");

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

        #endregion

        #region Model

        public List<ModelNo> GetModelNoByProject(int projectId)
        {
            List<ModelNo> list = new();

            var parms = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@ProjectId", projectId)
            };

            DataTable dt = _db.ExecuteProcedureWithParameterForDataTable(
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

        #endregion

        #region Part

        public List<PartNo> GetPartNoByModel(int modelId)
        {
            List<PartNo> list = new();

            var parms = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@ModelId", modelId)
            };

            DataTable dt = _db.ExecuteProcedureWithParameterForDataTable(
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

        #endregion

        public List<Adhesive> GetAdhesive(int projectId)
        {
            List<Adhesive> list = new();

            SqlParameter[] parms =
            {
        new SqlParameter("@ProjectId", projectId)
    };

            DataTable dt =
                _db.ExecuteProcedureWithParameterForDataTable(
                    "ipqc.GetAdhesive",
                    parms);

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Adhesive
                {
                    AdhesiveId = Convert.ToInt32(dr["AdhesiveId"]),
                    AdhesiveName = dr["Adhesive"].ToString()
                });
            }

            return list;
        }

        public DataTable GetFormByA246FAdhesive(int lineId, int projectId, int adhesiveId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@AdhesiveId", adhesiveId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetFormByA246FAdhesive",
                parms);
        }

        public int InsertBulkGlueWeighingData(
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
            SqlParameter[] parms =
            {
        new SqlParameter("@LaserChecklist", dtChecklist),
        new SqlParameter("@CreatedBy", createdBy),
        new SqlParameter("@LineId", lineId),
        new SqlParameter("@ProjectId", projectId),
        new SqlParameter("@ProdLineLeader", prodLineLeader),
        new SqlParameter("@CheckedBy", checkedBy),
        new SqlParameter("@ApprovedBy", approvedBy),
        new SqlParameter("@ModelId", modelId),
        new SqlParameter("@PartId", partId)
    };

            return _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertBulkGlueWeighingData",
                parms);
        }
    }
}