using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL
{
    public class WireCombMoldingDAL
    {
        DbClass _db;

        public WireCombMoldingDAL()
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

        public List<A246FMachines> GetMachines()
        {
            List<A246FMachines> list = new();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("[ipqc].[GetA246FMachines]");

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

        public List<ModelNo> GetModelNo()
        {
            List<ModelNo> list = new();

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
            SqlParameter[] parms =
            {
                new SqlParameter("@ProjectId", projectId)
            };

            List<ModelNo> list = new();

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
            SqlParameter[] parms =
            {
                new SqlParameter("@ModelId", modelId)
            };

            List<PartNo> list = new();

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

        public DataTable GetWireCombMoldingData(
            int projectId,
            int lineId,
            int machineId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@MachineId", machineId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetFromByA246FWCMMC1Data",
                parms);
        }

        public int GetInspectionIdBySection(
            int sectionId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@SectionId", sectionId)
            };

            DataTable dt =
                _db.ExecuteProcedureWithParameterForDataTable(
                    "ipqc.GetInspectionIdBySection",

                    parms);

            if (dt.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(
                dt.Rows[0]["InspectionId"]);
        }

        public int InsertBulkA246FWCMMC1CheckList(
            DataTable dtChecklist,
            int lineId,
            int projectId,
            int modelId,
            int partId,
            string leader,
            string checkedBy,
            string approvedBy,
            string createdBy)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Checklist", dtChecklist),
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
                "ipqc.InsertBulkA246FWCMMC1CheckList",
                parms);
        }
    }
}