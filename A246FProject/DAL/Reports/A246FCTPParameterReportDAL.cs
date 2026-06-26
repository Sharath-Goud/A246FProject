//using Microsoft.Data.SqlClient;
//using System.Data;

//namespace A246FProject.DAL.Reports
//{
//    public class A246FCTPParameterReportDAL
//    {
//        public DataTable GetCTPParameterReport(
//            DateTime? fromDate,
//            int lineId,
//            int shiftId,
//            int projectId,
//            int machineId,
//            int modelId,
//            int partId)
//        {
//            SqlParameter[] parms =
//            {
//        new SqlParameter("@Date",fromDate),

//        new SqlParameter("@LineId",lineId),

//        new SqlParameter("@ShiftId",shiftId),

//        new SqlParameter("@ProjectId",projectId),

//        new SqlParameter("@MachineId",machineId),

//        new SqlParameter("@ModelId",modelId),

//        new SqlParameter("@PartId",partId)
//    };

//            return _db.ExecuteProcedureWithParameterForDataTable(
//                "ipqc.Rpt_GetA246FCTPParameter",
//                parms);
//        }
//    }
//}
