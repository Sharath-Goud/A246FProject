using A246FProject.DAL;
using A246FProject.Models;

namespace A246FProject.BAL
{
    public class FirstArticleDimensionBAL
    {

        FirstArticleDimensionDAL _dal = new FirstArticleDimensionDAL();

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

        public List<DimensionDto> GetDimensionData(int lineId, int projectId)
        {
            return _dal.GetDimensionData(lineId, projectId);
        }

        public int SaveDimension(SaveDimensionDto model)
        {
            return _dal.SaveDimension(model);
        }

        public int SubmitDimension(FirstArticleDimensionViewModel model)
        {
            return _dal.SubmitDimension(model);
        }
    }

}