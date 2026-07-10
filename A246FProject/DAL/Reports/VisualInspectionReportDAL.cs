using A246FProject.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace A246FProject.DAL.Reports
{
    public class VisualInspectionReportDAL
    {
        private readonly DbClass _db;

        public VisualInspectionReportDAL()
        {
            _db = DbClass.GetInstance();
        }

        public List<Visuals> GetVisuals(int projectId)
        {
            SqlParameter[] p =
            {
            new SqlParameter("@ProjectId",projectId)
        };

            DataTable dt =
                _db.ExecuteProcedureWithParameterForDataTable(
                    "ipqc.GetA246FVisualsByproject",
                    p);

            List<Visuals> list = new();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Visuals
                {
                    VisualsId = Convert.ToInt32(dr["VisualsId"]),
                    Visual = dr["Visual"].ToString()
                });
            }

            return list;
        }

        public DataTable GetVisualInspectionReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int visualsId)
        {
            SqlParameter[] parms =
            {
                new SqlParameter("@Date", fromDate ?? (object)DBNull.Value),
                new SqlParameter("@LineId", lineId),
                new SqlParameter("@ShiftId", shiftId),
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@VisualsId", visualsId)
            };

            return _db.ExecuteProcedureWithParameterForDataTable(
                "ipqc.Rpt_getVisualInspectionReport",
                parms);
        }
    }
}