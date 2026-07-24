using A246FProject.DAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL
{
    public class NegativeValidationBAL
    {
        NegativeValidationDAL _dal =
            new NegativeValidationDAL();

        public DataTable GetNegativeValidationData(int lineId, int projectId)
        {
            return _dal.GetNegativeValidationData(lineId, projectId);
        }

        public List<Result> GetResult()
        {
            return _dal.GetResult();
        }

        public int InsertBulkNegativeValidation(
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
            return _dal.InsertBulkNegativeValidation(
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
