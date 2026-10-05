using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models.Reports;
using A246FProject.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Infrastructure;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class NegativeValidationReportController : Controller
    {
        private const string ViewPath = "~/Views/Reports/NegativeValidationReport.cshtml";

        private readonly NegativeValidationReportBAL _bal =
            new NegativeValidationReportBAL();

        private readonly MasterBAL _master =
            new MasterBAL();

        private readonly NegativeValidationPdfService _pdf;

        static NegativeValidationReportController()
        {
            // Safe to set again if it is already set elsewhere
            QuestPDF.Settings.License = LicenseType.Community;
        }

        // IWebHostEnvironment is injected by ASP.NET Core automatically
        public NegativeValidationReportController(IWebHostEnvironment env)
        {
            _pdf = new NegativeValidationPdfService(env.WebRootPath);
        }

        [HttpGet]
        public IActionResult Index()
        {
            NegativeValidationReportViewModel model = new();

            model.dtReports = new DataTable();
            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();

            return View(ViewPath, model);
        }

        [HttpPost]
        public IActionResult Index(
            NegativeValidationReportViewModel model,
            string command)
        {
            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();

            // Search and Export run exactly the same query
            if (command == "Search" || command == "Export")
            {
                // your old code used model.FromDate.Value, which crashes when no date is selected
                if (model.FromDate == null)
                {
                    ModelState.AddModelError("FromDate", "Please select Date.");
                    model.dtReports = new DataTable();
                    return View(ViewPath, model);
                }

                model.dtReports = _bal.GetReport(
                    model.FromDate.Value.ToString("MM/dd/yyyy"),
                    model.LineId,
                    model.ShiftId,
                    model.ProjectId);

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
                    string dateText = model.FromDate.Value.ToString("dd/MM/yyyy");

                    var bytes = _pdf.GenerateReport(
                        model.dtReports, line, shift, dateText, project);

                    var fileName =
                        $"Negative_Validation_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    return File(bytes, "application/pdf", fileName);
                }
            }

            return View(ViewPath, model);
        }
    }
}