using A246FProject.DAL;
using System.Data;

namespace A246FProject.BAL
{
    public class TrapTestBAL
    {
        private readonly TrapTestDAL _dal;

        public TrapTestBAL()
        {
            _dal = new TrapTestDAL();
        }


        public DataTable GetTrapTestData(
            int lineId,
            int projectId)
        {
            return _dal.GetTrapTestData(
                lineId,
                projectId);
        }

        public int InsertBulkTrapTest(
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
            return _dal.InsertBulkTrapTest(
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