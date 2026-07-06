using A246FProject.DAL;
using Microsoft.Data.SqlClient;
using System.Data;
using A246FProject.Models;

public class VisualInspectionDAL
{
    DbClass _db;

    public VisualInspectionDAL()
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

    public List<Shift> GetShift()
    {
        List<Shift> list = new List<Shift>();

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
            new SqlParameter("@ProjectId",projectId)
        };

        DataTable dt =
            _db.ExecuteProcedureWithParameterForDataTable(
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

    public List<PartNo> GetPartNoByModel(int modelId)
    {
        SqlParameter[] p =
        {
            new SqlParameter("@ModelId",modelId)
        };

        DataTable dt =
            _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.GetPartNoByModel",
                p);

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

    public DataTable GetVisualInspectionData(
    int lineId,
    int projectId,
    int visualsId)
    {
        SqlParameter[] parms =
        {
        new SqlParameter("@ProjectId", projectId),
        new SqlParameter("@LineId", lineId),
        new SqlParameter("@VisualsId", visualsId)
    };

        return _db.ExecuteProcedureWithParameterForDataTable(
            "ipqc.GetVisualInspectionChecklist",
            parms);
    }

    public int InsertBulkVisualInspection(
        DataTable dtChecklist,
        string userId,
        int LineId,
        int ProjectId,
        string Model,
        string ProdLineLeader,
        string CheckedBy,
        string ApprovedBy,
        int ModelId,
        int PartId)
    {
        int result = 0;

        try
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Checklist", dtChecklist),
                new SqlParameter("@CreatedBy", userId),
                new SqlParameter("@LineId", LineId),
                new SqlParameter("@ProjectId", ProjectId),
                new SqlParameter("@Model", Model),
                new SqlParameter("@ProdLineLeader", ProdLineLeader),
                new SqlParameter("@CheckedBy", CheckedBy),
                new SqlParameter("@ApprovedBy", ApprovedBy),
                new SqlParameter("@ModelId", ModelId),
                new SqlParameter("@PartId", PartId)
            };

            result = _db.ExecuteNonQueryWithParameter(
                "ipqc.InsertBulkVisualInspection",
                parameters
            );
        }
        catch (Exception)
        {
            throw;
        }

        return result;
    }
}