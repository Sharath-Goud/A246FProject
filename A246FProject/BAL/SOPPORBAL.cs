using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL
{
    public class SOPPORBAL
    {
        SOPPORDAL _dal =
            new SOPPORDAL();

        public DataTable GetSOPPORData(int lineId, int projectId)
        {
            return _dal.GetSOPPORData(lineId, projectId);
        }

        public List<Result> GetResult()
        {
            return _dal.GetResult();
        }

        public int InsertSOPPORSingle(
            int lineId,
            int projectId,
            int riskId,
            int statusId,
            string idNumber,
            string leader,
            string checkedBy,
            string approvedBy)
        {
            return _dal.InsertSOPPORSingle(
                lineId, projectId, riskId, statusId, idNumber,
                leader, checkedBy, approvedBy);
        }

        public int InsertBulkSOPPOR(
            DataTable dt,
            int lineId,
            int projectId,
            string leader,
            string checkedBy,
            string approvedBy)
        {
            return _dal.InsertBulkSOPPOR(dt, lineId, projectId, leader, checkedBy, approvedBy);
        }

    }
}