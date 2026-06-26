using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL
{
    public class InnerMoldBAL
    {
        InnerMoldDAL _dal =
            new InnerMoldDAL();

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

        public DataTable GetInnerMoldingData(
            int projectId,
            int lineId,
            int machineId)
        {
            return _dal.GetInnerMoldingData(
                projectId,
                lineId,
                machineId);
        }

        public int GetInspectionIdBySection(
            int sectionId)
        {
            return _dal.GetInspectionIdBySection(
                sectionId);
        }

        public int InsertBulkA246FWCMMC1CheckList(
            DataTable dt,
            int lineId,
            int projectId,
            int modelId,
            int partId,
            string leader,
            string checkedBy,
            string approvedBy,
            string createdBy)
        {
            return _dal.InsertBulkA246FWCMMC1CheckList(
                dt, lineId, projectId, modelId, partId,
                leader, checkedBy, approvedBy, createdBy);
        }
    }
}