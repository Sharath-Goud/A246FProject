using A246FProject.Models;
using System.Data;

public class VisualInspectionBAL
{
    VisualInspectionDAL _dal = new VisualInspectionDAL();

    public List<Line> GetLine()
    {
        return _dal.GetLine();
    }

    public List<Shift> GetShift()
    {
        return _dal.GetShift();
    }

    public List<Project> GetProject()
    {
        return _dal.GetProject();
    }

    public List<ModelNo> GetModelNoByProject(int projectId)
    {
        return _dal.GetModelNoByProject(projectId);
    }

    public List<PartNo> GetPartNoByModel(int modelId)
    {
        return _dal.GetPartNoByModel(modelId);
    }

    public List<Visuals> GetVisuals(int projectId)
    {
        return _dal.GetVisuals(projectId);
    }

    public DataTable GetVisualInspectionData(
    int lineId,
    int projectId,
    int visualsId)
    {
        return _dal.GetVisualInspectionData(
            lineId,
            projectId,
            visualsId);
    }

    public int InsertBulkVisualInspection(
        DataTable dt,
        string userId,
        int lineId,
        int projectId,
        string model,
        string leader,
        string checkedBy,
        string approvedBy,
        int modelId,
        int partId)
    {
        return _dal.InsertBulkVisualInspection(
            dt,
            userId,
            lineId,
            projectId,
            model,
            leader,
            checkedBy,
            approvedBy,
            modelId,
            partId
        );
    }
}