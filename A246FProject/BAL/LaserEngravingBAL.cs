using A246FProject.DAL;
using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class LaserEngravingBAL
    {
        LaserEngravingDAL _dal = new LaserEngravingDAL();

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

        public List<PartNo> GetPartNoByModel(int modelId)
        {
            return _dal.GetPartNoByModel(modelId);
        }

        public List<ModelNo> GetModelNoByProject(int projectId)
        {
            return _dal.GetModelNoByProject(projectId);
        }

        public DataTable GetChecklist(int lineId, int projectId)
        {
            return _dal.GetChecklist(lineId, projectId);
        }

        public void SaveLaserChecklist(LaserEngravingViewModel model, DataTable dt, string createdBy)
        {
            _dal.SaveLaserChecklist(model, dt, createdBy);
        }

    }
}