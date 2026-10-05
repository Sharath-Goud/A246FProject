using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models;
using A246FProject.Models.Reports;
using A246FProject.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Infrastructure;
using System.Data;
using System.Globalization;

namespace A246FProject.Controllers.Reports
{
    public class NewHankingReportController : Controller
    {
        private const string ViewPath = "~/Views/Reports/NewHankingReport.cshtml";

        private readonly NewHankingReportBAL _bal;
        private readonly MasterBAL _master;
        private readonly NewHankingPdfService _pdf;

        static NewHankingReportController()
        {
            // Safe to set again if it is already set elsewhere
            QuestPDF.Settings.License = LicenseType.Community;
        }

        // IWebHostEnvironment is injected by ASP.NET Core automatically
        public NewHankingReportController(IWebHostEnvironment env)
        {
            _bal = new NewHankingReportBAL();
            _master = new MasterBAL();
            _pdf = new NewHankingPdfService(env.WebRootPath);
        }

        [HttpPost]
        public JsonResult GetModelNoByProject(int projectId)
        {
            return Json(_master.GetModelNoByProject(projectId));
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(int modelId)
        {
            return Json(_master.GetPartNoByModel(modelId));
        }

        [HttpGet]
        public IActionResult Index()
        {
            NewHankingReportViewModel model = new NewHankingReportViewModel();

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
        public IActionResult Index(
            NewHankingReportViewModel model,
            string command)
        {
            // Reload dropdown data
            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Machines = _master.GetA246FMachines();

            // Reload Model dropdown
            model.ModelNos = model.ProjectId > 0
                ? _master.GetModelNoByProject(model.ProjectId)
                : new List<ModelNo>();

            // Reload Part dropdown
            model.PartNos = model.ModelId > 0
                ? _master.GetPartNoByModel(model.ModelId)
                : new List<PartNo>();

            // Search and Export use exactly the same validation and the same query
            if (command == "Search" || command == "Export")
            {
                string rawDate = Request.Form["FromDate"].ToString();

                DateTime parsedDate;

                if (string.IsNullOrWhiteSpace(rawDate))
                {
                    ModelState.AddModelError("FromDate", "Please select Date.");
                    model.dtReports = new DataTable();
                    return View(ViewPath, model);
                }

                if (!DateTime.TryParseExact(
                        rawDate,
                        new[] { "MM/dd/yyyy", "MM/dd/yy" },
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out parsedDate))
                {
                    ModelState.AddModelError("FromDate", "Invalid Date. Please select a valid date.");
                    model.dtReports = new DataTable();
                    return View(ViewPath, model);
                }

                // IMPORTANT
                model.FromDate = parsedDate.Date;

                // Validate Line
                if (model.LineId <= 0)
                {
                    ModelState.AddModelError("LineId", "Please select Line.");
                    model.dtReports = new DataTable();
                    return View(ViewPath, model);
                }

                // Validate Shift
                // ShiftId = 0 is a valid Shift in your SP,
                // so DO NOT check ShiftId <= 0 here.

                // Validate Project
                if (model.ProjectId <= 0)
                {
                    ModelState.AddModelError("ProjectId", "Please select Project.");
                    model.dtReports = new DataTable();
                    return View(ViewPath, model);
                }

                // Validate Machine
                if (model.MachineId <= 0)
                {
                    ModelState.AddModelError("MachineId", "Please select Machine.");
                    model.dtReports = new DataTable();
                    return View(ViewPath, model);
                }

                // Validate Part
                if (model.PartId <= 0)
                {
                    ModelState.AddModelError("PartId", "Please select Part Number.");
                    model.dtReports = new DataTable();
                    return View(ViewPath, model);
                }

                // dtReports is not posted back from the browser,
                // so the same query is executed again for both Search and Export
                model.dtReports = _bal.GetNewHankingReport(
                    model.FromDate,
                    model.LineId,
                    model.ShiftId,
                    model.ProjectId,
                    model.MachineId,
                    model.PartId);

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
                    string modelName = model.ModelNos?.FirstOrDefault(x => x.ModelId == model.ModelId)?.Model ?? "";
                    string part = model.PartNos?.FirstOrDefault(x => x.PartId == model.PartId)?.Part ?? "";
                    string machine = model.Machines?.FirstOrDefault(x => x.MachineId == model.MachineId)?.Machine ?? "";
                    string dateText = parsedDate.ToString("dd/MM/yyyy");

                    var bytes = _pdf.GenerateReport(
                        model.dtReports, line, shift, dateText, project, modelName, part, machine);

                    var fileName =
                        $"New_Hanking_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    return File(bytes, "application/pdf", fileName);
                }
            }

            return View(ViewPath, model);
        }
    }
}