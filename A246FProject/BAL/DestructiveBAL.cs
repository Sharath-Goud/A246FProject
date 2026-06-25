using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL
{
    public class DestructiveBAL
    {
        DestructiveDAL _dal = new();

        public List<Line> GetLine()
        {
            return _dal.GetLine();
        }

        public List<Project> GetProject()
        {
            return _dal.GetProject();
        }

        public List<ModelNo> GetModelNo()
        {
            return _dal.GetModelNo();
        }

        public List<PartNo> GetPartNoByModel(int modelId)
        {
            return _dal.GetPartNoByModel(modelId);
        }

        public List<ModelNo> GetModelNoByProject(int projectId)
        {
            return _dal.GetModelNoByProject(projectId);
        }

        public List<A246FMachines> GetMachines()
        {
            return _dal.GetMachines();
        }

        public DataTable GetDestructiveData(
            int lineId,
            int projectId,
            int machineId)
        {
            return _dal.GetDestructiveData(
                lineId,
                projectId,
                machineId);
        }

        public int InsertBulkA246FMHB2CheckList(
            DataTable dtChecklist,
            string userId,
            int lineId,
            string prodLineLeader,
            string checkedBy,
            string approvedBy,
            int modelId,
            int partId)
        {
            return _dal.InsertBulkA246FMHB2CheckList(
                dtChecklist,
                userId,
                lineId,
                prodLineLeader,
                checkedBy,
                approvedBy,
                modelId,
                partId);
        }
    }
}