using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models;
using A246FProject.Models.Reports;
using A246FProject.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Infrastructure;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class WireCombMoldingReportController : Controller
    {
        private const string ViewPath = "~/Views/Reports/WireCombMoldingReport.cshtml";

        private readonly WireCombMoldingReportBAL _bal;
        private readonly MasterBAL _master;
        private readonly WireCombMoldingPdfService _pdf;

        static WireCombMoldingReportController()
        {
            // Safe to set again if it is already set elsewhere
            QuestPDF.Settings.License = LicenseType.Community;
        }

        // IWebHostEnvironment is injected by ASP.NET Core automatically
        public WireCombMoldingReportController(IWebHostEnvironment env)
        {
            _bal = new WireCombMoldingReportBAL();
            _master = new MasterBAL();
            _pdf = new WireCombMoldingPdfService(env.WebRootPath);
        }

        [HttpGet]
        public IActionResult Index()
        {
            WireCombMoldingReportViewModel model = new();

            model.dtReports = new DataTable();

            FillDropdowns(model);

            return View(ViewPath, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(WireCombMoldingReportViewModel model, string command)
        {
            FillDropdowns(model);

            if (command == "Search" || command == "Export")
            {
                var date = model.FromDate?.ToString("MM/dd/yyyy");

                // dtReports is not posted back from the browser,
                // so the same query is executed again for both Search and Export
                model.dtReports = _bal.GetCCMCHI4SCReport(
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

                    // header values for the PDF come from the selected filters
                    string line = model.Lines?.FirstOrDefault(x => x.LineId == model.LineId)?.LineName ?? "";
                    string shift = model.Shifts?.FirstOrDefault(x => x.ShiftId == model.ShiftId)?.ShiftName ?? "";
                    string project = model.Projects?.FirstOrDefault(x => x.ProjectId == model.ProjectId)?.ProjectName ?? "";
                    string machine = model.Machines?.FirstOrDefault(x => x.MachineId == model.MachineId)?.Machine ?? "";
                    string dateText = model.FromDate?.ToString("dd/MM/yyyy") ?? "";

                    var bytes = _pdf.GenerateReport(
                        model.dtReports, line, shift, dateText, project, machine);

                    var fileName =
                        $"Wire_Comb_Molding_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    return File(bytes, "application/pdf", fileName);
                }
            }

            return View(ViewPath, model);
        }

        private void FillDropdowns(WireCombMoldingReportViewModel model)
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