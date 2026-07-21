using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL
{
    public class OQCInspectionBAL
    {
        OQCInspectionDAL _dal = new OQCInspectionDAL();

        public List<Project> GetProject()
        {
            return _dal.GetProject();
        }

        public DataTable GetInspectionData(int projectId)
        {
            return _dal.GetInspectionData(projectId);
        }
    }
}
