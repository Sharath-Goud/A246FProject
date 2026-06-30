using A246FProject.DAL;
using A246FProject.Models;

namespace A246FProject.BAL
{
    public class MasterBAL
    {
        MasterDAL _dal;

        public MasterBAL()
        {
            _dal = new MasterDAL();
        }

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

        public List<A246FMachines> GetA246FMachines()
        {
            return _dal.GetA246FMachines();
        }

        public List<ModelNo> GetModelNoByProject(int projectId)
        {
            return _dal.GetModelNoByProject(projectId);
        }

        public List<PartNo> GetPartNoByModel(int modelId)
        {
            return _dal.GetPartNoByModel(modelId);
        }
    }
}