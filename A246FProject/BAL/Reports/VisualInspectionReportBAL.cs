using A246FProject.DAL.Reports;
using System.Data;

public class VisualInspectionReportBAL
{
    private readonly VisualInspectionReportDAL _dal;

    public VisualInspectionReportBAL()
    {
        _dal = new VisualInspectionReportDAL();
    }

    public DataTable GetVisualInspectionReport(
    string fromDate,
    int lineId,
    int shiftId,
    int projectId,
    int visualsId)
    {
        return _dal.GetVisualInspectionReport(
            fromDate,
            lineId,
            shiftId,
            projectId,
            visualsId);
    }
}