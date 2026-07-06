using A246FProject.Models;
using System.Data;

public class FirstArticleInspectionBAL
{
    FirstArticleInspectionDAL _dal = new FirstArticleInspectionDAL();

    public List<Line> GetLine()
    {
        return _dal.GetLine();
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

    public List<InspectionDto> GetInspectionData(int lineId, int projectId)
    {
        return _dal.GetInspectionData(lineId, projectId);
    }

    public int SaveInspection(SaveInspectionDto model)
    {
        return _dal.SaveInspection(model);
    }

    public int SubmitInspection(FirstArticleInspectionViewModel model)
    {
        return _dal.SubmitInspection(model);
    }
}