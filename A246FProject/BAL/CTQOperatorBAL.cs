using A246FProject.DAL;
using System.Data;

namespace A246FProject.BAL
{
    public class CTQOperatorBAL
    {
        private readonly CTQOperatorDAL _dal;

        public CTQOperatorBAL()
        {
            _dal = new CTQOperatorDAL();
        }


        public DataTable GetCTQOperatorData(
            int lineId,
            int projectId)
        {
            return _dal.GetCTQOperatorData(
                lineId,
                projectId);
        }

        public int InsertBulkCTQOperator(
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
            return _dal.InsertBulkCTQOperator(
                dt,
                lineId,
                projectId,
                leader,
                checkedBy,
                approvedBy,
                modelId,
                partId,
                createdBy);
        }
    }
}
