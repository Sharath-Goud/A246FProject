using A246FProject.DAL;
using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class FirstArticleInspectionDAL
{
    DbClass _db;

    public FirstArticleInspectionDAL()
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
        SqlParameter[] p =
        {
            new SqlParameter("@ProjectId", projectId)
        };

        DataTable dt =
            _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetModelNoByProject",
                p);

        List<ModelNo> list = new List<ModelNo>();

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

    public List<InspectionDto> GetInspectionData(int lineId, int projectId)
    {
        SqlParameter[] p =
        {
        new SqlParameter("@LineId", lineId),
        new SqlParameter("@ProjectId", projectId)
    };

        DataTable dt = _db.ExecuteProcedureWithParameterForDataTable(
            "ipqc.GetA246FFirstAirticleInspec",
            p);

        List<InspectionDto> list = new();

        foreach (DataRow dr in dt.Rows)
        {
            list.Add(new InspectionDto
            {
                StationName = dr["StationName"].ToString(),
                ItemName = dr["ItemName"].ToString(),
                Content = dr["Content"].ToString(),
                ContentId = Convert.ToInt32(dr["ContentId"])
            });
        }

        return list;
    }

    public int SaveInspection(SaveInspectionDto model)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Id");
        dt.Columns.Add("ContentId");
        dt.Columns.Add("SRId");
        dt.Columns.Add("Result");
        dt.Columns.Add("RejectDescribe");

        dt.Rows.Add(
            1,
            model.ContentId,
            model.SRId,
            model.Result,
            model.RejectDescribe);

        SqlParameter[] p =
        {
            new SqlParameter("@Checklist1", dt),
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
            "ipqc.InsertBulkA246FFirstArticleInspection",
            p);
    }

    public int SubmitInspection(FirstArticleInspectionViewModel model)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Id");
        dt.Columns.Add("ContentId");
        dt.Columns.Add("SRId");
        dt.Columns.Add("Result");
        dt.Columns.Add("RejectDescribe");

        int id = 1;

        foreach (var row in model.CheckListFirstArticleInspections)
        {
            dt.Rows.Add(
                id++,
                row.ContentId,
                row.SRId,
                row.Result,
                row.RejectDescribe);
        }

        SqlParameter[] p =
        {
            new SqlParameter("@Checklist1", dt),
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
            "ipqc.InsertBulkA246FFirstArticleInspection",
            p);
    }
}