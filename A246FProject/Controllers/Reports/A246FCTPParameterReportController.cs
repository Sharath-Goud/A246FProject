using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models;
using A246FProject.Models.Reports;
using A246FProject.Services;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Infrastructure;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class A246FCTPParameterReportController : Controller
    {
        private const string ViewPath = "~/Views/Reports/A246FCTPParamterReport.cshtml";

        private readonly A246FCTPParameterReportBAL _bal;
        private readonly MasterBAL _master;
        private readonly A246FCTPPdfService _pdf;

        static A246FCTPParameterReportController()
        {
            // Safe to set again if it is already set elsewhere
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public A246FCTPParameterReportController()
        {
            _bal = new A246FCTPParameterReportBAL();
            _master = new MasterBAL();
            _pdf = new A246FCTPPdfService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            A246FCTPParameterReportViewModel model = new();

            model.dtReports = new DataTable();

            FillDropdowns(model);

            return View(ViewPath, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(A246FCTPParameterReportViewModel model, string command)
        {
            FillDropdowns(model);

            if (command == "Search" || command == "Export")
            {
                var date = model.FromDate?.ToString("MM/dd/yyyy");

                // dtReports is not posted back from the browser,
                // so the same query is executed again for both Search and Export
                model.dtReports = _bal.GetCTPParameterReport(
                    date,
                    model.LineId,
                    model.ShiftId,
                    model.ProjectId,
                    model.MachineId);

                HttpContext.Session.SetString("SearchDone", "true");

                if (command == "Export")
                {
                    if (model.dtReports == null || model.dtReports.Rows.Count == 0)
                    {
                        TempData["Error"] = "No data found to export.";
                        return View(ViewPath, model);
                    }

                    // Only PDF is implemented here; add Excel etc. below if needed
                    var bytes = _pdf.GenerateReport(model.dtReports);

                    var fileName =
                        $"A246F_CTP_Parameter_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    return File(bytes, "application/pdf", fileName);
                }
            }

            return View(ViewPath, model);
        }

        private void FillDropdowns(A246FCTPParameterReportViewModel model)
        {
            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Machines = _master.GetA246FMachines();

            model.ModelNos = model.ProjectId > 0
                ? _master.GetModelNoByProject(model.ProjectId)
                : new List<ModelNo>();

            model.PartNos = model.ModelId > 0
                ? _master.GetPartNoByModel(model.ModelId)
                : new List<PartNo>();
        }
    }
}