using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL
{
    public class LaserEngravingDAL
    {
        DbClass _db;

        public LaserEngravingDAL()
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
                    LineId = Convert.ToInt32(dr["LineId"]),
                    LineName = dr["LineName"].ToString()
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
                // Only A246F Project
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

        public DataTable GetChecklist(int lineId, int projectId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@LineId", lineId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetFromByA246FLaserEngravingData",
                parms);
        }


        public void SaveLaserChecklist(LaserEngravingViewModel model, DataTable dt, string createdBy)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Checklist", dt),
                new SqlParameter("@CreatedBy", createdBy), 
                new SqlParameter("@LineId", model.LineId),
                new SqlParameter("@ProjectId", model.ProjectId),
                new SqlParameter("@ModelId", model.ModelId),
                new SqlParameter("@PartId", model.PartId),
                new SqlParameter("@ProdLineLeader", model.ProdLineLeader),
                new SqlParameter("@CheckedBy", model.CheckedBy),
                new SqlParameter("@ApprovedBy", model.ApprovedBy)
            };

            _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertBulkA246FLaserEngravingCheckList",
                parms);
        }
    }
}