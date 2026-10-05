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
    public class AOIReportController : Controller
    {
        private const string ViewPath = "~/Views/Reports/AOIReport.cshtml";

        private readonly AOIReportBAL _bal;
        private readonly MasterBAL _master;
        private readonly AOIPdfService _pdf;

        static AOIReportController()
        {
            // Safe to set again if it is already set elsewhere
            QuestPDF.Settings.License = LicenseType.Community;
        }

        // IWebHostEnvironment is injected by ASP.NET Core automatically
        public AOIReportController(IWebHostEnvironment env)
        {
            _bal = new AOIReportBAL();
            _master = new MasterBAL();
            _pdf = new AOIPdfService(env.WebRootPath);
        }

        [HttpGet]
        public IActionResult Index()
        {
            AOIReportViewModel model = new();

            model.dtReports = new DataTable();

            FillDropdowns(model);

            return View(ViewPath, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AOIReportViewModel model, string command)
        {
            FillDropdowns(model);

            if (command == "Search" || command == "Export")
            {
                var date = model.FromDate?.ToString("yyyy-MM-dd");

                // dtReports is not posted back from the browser,
                // so the same query is executed again for both Search and Export
                model.dtReports = _bal.GetAOIReport(
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
                        $"AOI_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    return File(bytes, "application/pdf", fileName);
                }
            }

            return View(ViewPath, model);
        }

        private void FillDropdowns(AOIReportViewModel model)
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