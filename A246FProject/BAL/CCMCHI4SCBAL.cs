using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL
{
    public class CCMCHI4SCBAL
    {
        CCMCHI4SCDAL _dal =
            new CCMCHI4SCDAL();

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

        public DataTable GetCCMCHI4SCData(
            int lineId,
            int projectId,
            int machineId)
        {
            return _dal.GetCCMCHI4SCData(
                lineId,
                projectId,
                machineId);
        }

        public int InsertBulkA246FCMMC1CheckList(
                DataTable dtChecklist,
                string userId,
                int lineId,
                string prodLineLeader,
                string checkedBy,
                string approvedBy,
                int modelId,
                int partId)
        {
            return _dal.InsertBulkA246FCMMC1CheckList(
                dtChecklist,
                userId,
                lineId,
                prodLineLeader,
                checkedBy,
                approvedBy,
                modelId,
                partId);
        }

        public int GetInspectionIdBySection(int sectionId)
        {
            return _dal.GetInspectionIdBySection(sectionId);
        }
    }
}
