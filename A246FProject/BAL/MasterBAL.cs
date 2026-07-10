using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

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

        public List<Visuals> GetVisuals(int projectId)
        {
            return _dal.GetVisuals(projectId);
        }

        public List<Adhesive> GetAdhesive(int projectId)
        {
            return _dal.GetAdhesive(projectId);
        }

        public DataTable GetFormByA246FAdhesive(int lineId, int projectId, int adhesiveId)
        {
            return _dal.GetFormByA246FAdhesive(lineId, projectId, adhesiveId);
        }

        public int InsertBulkGlueWeighingData(
            DataTable dtChecklist,
            string createdBy,
            int lineId,
            int projectId,
            string prodLineLeader,
            string checkedBy,
            string approvedBy,
            int modelId,
            int partId)
        {
            return _dal.InsertBulkGlueWeighingData(
                dtChecklist,
                createdBy,
                lineId,
                projectId,
                prodLineLeader,
                checkedBy,
                approvedBy,
                modelId,
                partId);
        }
    }
}