using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL
{
    public class AOIBAL
    {
        private readonly AOIDAL _dal = new AOIDAL();

        #region Masters

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

        public List<A246FMachines> GetMachines()
        {
            return _dal.GetMachines();
        }

        public List<ModelNo> GetModelNoByProject(int projectId)
        {
            return _dal.GetModelNoByProject(projectId);
        }

        public List<PartNo> GetPartNoByModel(int modelId)
        {
            return _dal.GetPartNoByModel(modelId);
        }

        #endregion

        #region Search

        public DataTable GetAOIData(int projectId, int lineId, int machineId)
        {
            return _dal.GetAOIData(projectId, lineId, machineId);
        }

        #endregion

        #region Save

        public int InsertBulkAOI(
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
            try
            {
                return _dal.InsertBulkAOI(
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
            catch
            {
                throw;
            }
        }

        #endregion
    }
}