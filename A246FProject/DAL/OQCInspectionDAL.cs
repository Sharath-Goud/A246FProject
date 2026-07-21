using System.Data;
using A246FProject.Models;
using Microsoft.Data.SqlClient;

namespace A246FProject.DAL
{
    public class OQCInspectionDAL
    {
        DbClass _db;

        public OQCInspectionDAL()
        {
            _db = DbClass.GetInstance();
        }

        public List<Project> GetProject()
        {
            List<Project> list = new List<Project>();

            DataTable dt =
                _db.ExecuteProcedureForDataTable("[ipqc].[GetProject]");

            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["ProjectId"]) == 31)
                {
                    list.Add(new Project
                    {
                        ProjectId = Convert.ToInt32(dr["ProjectId"]),
                        ProjectName = dr["ProjectName"].ToString()
                    });
                }
            }

            return list;
        }

        public DataTable GetInspectionData(int projectId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ProjectId", projectId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "[ipqc].[GetOQCInspectionData]",
                parameters);
        }
    }
}