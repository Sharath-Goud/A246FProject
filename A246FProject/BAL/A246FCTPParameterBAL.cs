using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL
{
    public class A246FCTPParameterBAL
    {
        A246FCTPParameterDAL _dal =
            new A246FCTPParameterDAL();

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

        public DataTable GetCTPParameterData(
            int lineId,
            int projectId,
            int machineId)
        {
            return _dal.GetCTPParameterData(
                lineId,
                projectId,
                machineId);
        }

        public List<PartNo> GetPartNoByModel(int modelId)
        {
            return _dal.GetPartNoByModel(modelId);
        }

        public List<ModelNo> GetModelNoByProject(int projectId)
        {
            return _dal.GetModelNoByProject(projectId);
        }
    }
}