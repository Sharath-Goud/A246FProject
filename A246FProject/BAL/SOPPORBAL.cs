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

        public int InsertBulkSOPPOR(
            DataTable dt,
            int lineId,
            int projectId,
            string leader,
            string checkedBy,
            string approvedBy,
            int modelId,
            int partId,
            string createdBy)
        {
            return _dal.InsertBulkSOPPOR(
                dt,
                lineId,
                projectId,
                leader,
                checkedBy,
                approvedBy,
                modelId,
                partId,
                createdBy
            );
        }

    }
}