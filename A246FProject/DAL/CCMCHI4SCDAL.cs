using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL
{
    public class CCMCHI4SCDAL
    {
        DbClass _db;

        public CCMCHI4SCDAL()
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

        public DataTable GetCCMCHI4SCData(
            int lineId,
            int projectId,
            int machineId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@MachineId", machineId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetFromByA246FCCMMC1DataaNew",
                parms);
        }

        public int InsertBulkA246FCMMC1CheckList(
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
                "ipqc.InsertBulkA246FCMMC1CheckList",
                parms);
        }

        public int GetInspectionIdBySection(int sectionId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@SectionId", sectionId)
            };

            DataTable dt =
                _db.ExecuteProcedureWithParameterForDataTable(
                    "ipqc.GetInspectionIdBySection",
                    parms);

            if (dt == null || dt.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dt.Rows[0]["InspectionId"]);
        }
    }
}
