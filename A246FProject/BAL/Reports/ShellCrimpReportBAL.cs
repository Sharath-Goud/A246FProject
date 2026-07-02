using A246FProject.DAL;
using A246FProject.DAL.Reports;
using A246FProject.Models;
using System.Data;

namespace A246FProject.BAL.Reports
{
    public class ShellCrimpReportBAL
    {
        private readonly ShellCrimpReportDAL _dal;

        public ShellCrimpReportBAL()
        {
            _dal = new ShellCrimpReportDAL();
        }

        public DataTable GetShellCrimpReport(
            string fromDate,
            int lineId,
            int shiftId,
            int projectId,
            int machineId)
        {
            return _dal.GetShellCrimpReport(
                fromDate,
                lineId,
                shiftId,
                projectId,
                machineId);
        }
    }
}
