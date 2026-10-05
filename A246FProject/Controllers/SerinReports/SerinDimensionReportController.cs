using A246FProject.BAL;
using A246FProject.BAL.SerinReports;
using A246FProject.Models;
using A246FProject.Models.SerinReports;
using A246FProject.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Infrastructure;
using System.Data;

namespace A246FProject.Controllers.SerinReports
{
    public class SerinDimensionReportController : Controller
    {
        private const string ViewPath = "~/Views/SerinReports/SerinDimensionReport.cshtml";

        private readonly SerinDimensionReportBAL _bal;
        private readonly MasterBAL _master;
        private readonly SerinDimensionPdfService _pdf;

        static SerinDimensionReportController()
        {
            // Safe to set again if it is already set elsewhere
            QuestPDF.Settings.License = LicenseType.Community;
        }

        // IWebHostEnvironment is injected by ASP.NET Core automatically
        public SerinDimensionReportController(IWebHostEnvironment env)
        {
            _bal = new SerinDimensionReportBAL();
            _master = new MasterBAL();
            _pdf = new SerinDimensionPdfService(env.WebRootPath);
        }

        [HttpGet]
        public IActionResult Index()
        {
            SerinDimensionReportViewModel model = new();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Machines = _master.GetA246FMachines();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            return View(ViewPath, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SerinDimensionReportViewModel model, string command)
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

            // Search and Export run exactly the same query
            if (command == "Search" || command == "Export")
            {
                // FromDate is a string in this model (the date picker sends MM/dd/yyyy)
                string date = model.FromDate;

                if (string.IsNullOrEmpty(date))
                {
                    model.dtReports = new DataTable();
                }
                else
                {
                    // dtReports is not posted back from the browser,
                    // so the same query is executed again for both Search and Export
                    model.dtReports = _bal.GetDimensionReport(
                        date,
                        model.LineId,
                        model.ShiftId,
                        model.ProjectId,
                        model.MachineId);
                }

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

                    var bytes = _pdf.GenerateReport(
                        model.dtReports, line, shift, date, project, machine);

                    var fileName =
                        $"Serin_Dimension_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    return File(bytes, "application/pdf", fileName);
                }
            }

            return View(ViewPath, model);
        }
    }
}