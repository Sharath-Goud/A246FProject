using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL
{
    public class ShellCrimpBAL
    {
        ShellCrimpDAL _dal = new ShellCrimpDAL();

        public List<Line> GetLine() => _dal.GetLine();

        public List<Project> GetProject() => _dal.GetProject();

        public List<ModelNo> GetModelNo() => _dal.GetModelNo();

        public List<A246FMachines> GetMachines() => _dal.GetMachines();

        public List<PartNo> GetPartNoByModel(int modelId)
            => _dal.GetPartNoByModel(modelId);

        public List<ModelNo> GetModelNoByProject(int projectId)
            => _dal.GetModelNoByProject(projectId);

        public DataTable GetShellCrimpData(int projectId, int lineId, int machineId)
            => _dal.GetShellCrimpData(projectId, lineId, machineId);

        public int InsertBulkShellCrimp(
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
            return _dal.InsertBulkShellCrimp(
                dt, lineId, projectId, modelId,
                partId, leader, checkedBy, approvedBy, createdBy);
        }
    }
}