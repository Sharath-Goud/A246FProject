using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL
{
    public class NewHankingBAL
    {
        NewHankingDAL _dal =
            new NewHankingDAL();

        public List<Line> GetLine()
        {
            return _dal.GetLine();
        }

        public List<Project> GetProject()
        {
            return _dal.GetProject();
        }

        public List<A246FMachines> GetMachines()
        {
            return _dal.GetMachines();
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

        public DataTable GetNewHankingData(
            int lineId,
            int projectId,
            int machineId,
            int partId)
        {
            return _dal.GetNewHankingData(
                lineId,
                projectId,
                machineId,
                partId);
        }

        public int InsertBulkA246FNewHanking(
            List<NewHankingChecklistItem> items,
            string createdBy,
            int lineId,
            int projectId,
            string modelName,
            string prodLineLeader,
            string checkedBy,
            string approvedBy,
            int modelId,
            int partId)
        {
            return _dal.InsertBulkA246FNewHanking(
                items, createdBy, lineId, projectId, modelName,
                prodLineLeader, checkedBy, approvedBy, modelId, partId);
        }
    }
}