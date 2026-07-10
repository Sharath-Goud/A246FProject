using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL
{
    public class FirstArticleDimensionDAL
    {

        DbClass _db;

        public FirstArticleDimensionDAL()
        {
            _db = DbClass.GetInstance();
        }

        public List<Line> GetLine()
        {
            List<Line> list = new List<Line>();

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

        public List<Project> GetProject()
        {
            List<Project> list = new List<Project>();

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
            SqlParameter[] p =
            {
            new SqlParameter("@ModelId", modelId)
        };

            DataTable dt =
                _db.ExecuteProcedureWithParameterForDataTable(
                    "ipqc.GetPartNoByModel",
                    p);

            List<PartNo> list = new List<PartNo>();

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

        public List<DimensionDto> GetDimensionData(int lineId, int projectId)
        {
            SqlParameter[] p =
            {
        new SqlParameter("@LineId", lineId),
        new SqlParameter("@ProjectId", projectId)
    };

            DataTable dt = _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetFormByFirstAirticleDimension",
                p);

            List<DimensionDto> list = new();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new DimensionDto
                {
                    StationName = dr["StationName"].ToString(),

                    SpecId = Convert.ToInt32(dr["SpecId"]),

                    Specification = dr["Specification"].ToString(),

                    TotalCount = Convert.ToInt32(dr["TotalCount"])
                });
            }

            return list;
        }

        public int SaveDimension(SaveDimensionDto model)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id");
            dt.Columns.Add("SpecId");
            dt.Columns.Add("Check1");
            dt.Columns.Add("Check2");
            dt.Columns.Add("Check3");
            dt.Columns.Add("Check4");
            dt.Columns.Add("Check5");
            dt.Columns.Add("SerialNo1");
            dt.Columns.Add("SerialNo2");
            dt.Columns.Add("SerialNo3");
            dt.Columns.Add("SerialNo4");
            dt.Columns.Add("SerialNo5");

            dt.Rows.Add(
                1,
                model.SpecId,
                model.Check1,
                model.Check2,
                model.Check3,
                model.Check4,
                model.Check5,
                model.SerialNo1,
                model.SerialNo2,
                model.SerialNo3,
                model.SerialNo4,
                model.SerialNo5
            );

            SqlParameter[] p =
            {
                new SqlParameter("@Checklist2", dt),
                new SqlParameter("@CreatedBy", model.CreatedBy),
                new SqlParameter("@LineId", model.LineId),
                new SqlParameter("@ProjectId", model.ProjectId),
                new SqlParameter("@ProdLineLeader", model.ProdLineLeader),
                new SqlParameter("@CheckedBy", model.CheckedBy),
                new SqlParameter("@ApprovedBy", model.ApprovedBy),
                new SqlParameter("@ModelId", model.ModelId),
                new SqlParameter("@PartId", model.PartId),
                new SqlParameter("@ProductName", model.ProductName),
                new SqlParameter("@WorkOrder", model.WorkOrder),
                new SqlParameter("@DrawingVersion", model.DrawingVersion),
                new SqlParameter("@PackVersion", model.PackVersion),
                new SqlParameter("@SamplingQty", model.SamplingQty)
            };

            return _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertBulkFirstAirticleDimension",
                p);
        }

        public int SubmitDimension(FirstArticleDimensionViewModel model)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id");
            dt.Columns.Add("SpecId");
            dt.Columns.Add("Check1");
            dt.Columns.Add("Check2");
            dt.Columns.Add("Check3");
            dt.Columns.Add("Check4");
            dt.Columns.Add("Check5");
            dt.Columns.Add("SerialNo1");
            dt.Columns.Add("SerialNo2");
            dt.Columns.Add("SerialNo3");
            dt.Columns.Add("SerialNo4");
            dt.Columns.Add("SerialNo5");

            int id = 1;

            foreach (var row in model.CheckListFirstArticleDimensions)
            {
                dt.Rows.Add(
                    id++,
                    row.SpecId,
                    row.Check1,
                    row.Check2,
                    row.Check3,
                    row.Check4,
                    row.Check5,
                    row.SerialNo1,
                    row.SerialNo2,
                    row.SerialNo3,
                    row.SerialNo4,
                    row.SerialNo5
                );
            }

            SqlParameter[] p =
            {
                new SqlParameter("@Checklist2", dt),
                new SqlParameter("@CreatedBy", model.CreatedBy),
                new SqlParameter("@LineId", model.LineId),
                new SqlParameter("@ProjectId", model.ProjectId),
                new SqlParameter("@ProdLineLeader", model.ProdLineLeader),
                new SqlParameter("@CheckedBy", model.CheckedBy),
                new SqlParameter("@ApprovedBy", model.ApprovedBy),
                new SqlParameter("@ModelId", model.ModelId),
                new SqlParameter("@PartId", model.PartId),
                new SqlParameter("@ProductName", model.ProductName),
                new SqlParameter("@WorkOrder", model.WorkOrder),
                new SqlParameter("@DrawingVersion", model.DrawingVersion),
                new SqlParameter("@PackVersion", model.PackVersion),
                new SqlParameter("@SamplingQty", model.SamplingQty)
            };

            return _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertBulkFirstAirticleDimension",
                p);
        }
    }

}