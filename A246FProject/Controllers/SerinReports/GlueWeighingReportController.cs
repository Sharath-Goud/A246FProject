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
    public class GlueWeighingReportController : Controller
    {
        private const string ViewPath = "~/Views/SerinReports/GlueWeighingReport.cshtml";

        private readonly GlueWeighingReportBAL _bal;
        private readonly MasterBAL _master;
        private readonly GlueWeighingPdfService _pdf;

        static GlueWeighingReportController()
        {
            // Safe to set again if it is already set elsewhere
            QuestPDF.Settings.License = LicenseType.Community;
        }

        // IWebHostEnvironment is injected by ASP.NET Core automatically
        public GlueWeighingReportController(IWebHostEnvironment env)
        {
            _bal = new GlueWeighingReportBAL();
            _master = new MasterBAL();
            _pdf = new GlueWeighingPdfService(env.WebRootPath);
        }

        // used by the Project -> Adhesive dropdown (AJAX) and by the POST action below
        public List<Adhesive> GetAdhesiveByProject(int projectId)
        {
            return _master.GetAdhesive(projectId);
        }

        [HttpGet]
        public IActionResult Index()
        {
            GlueWeighingReportViewModel model = new();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Adhesives = new List<Adhesive>();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            return View(ViewPath, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(GlueWeighingReportViewModel model, string command)
        {
            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Adhesives = model.ProjectId > 0
                ? GetAdhesiveByProject(model.ProjectId)
                : new List<Adhesive>();

            model.ModelNos = model.ProjectId > 0
                ? _master.GetModelNoByProject(model.ProjectId)
                : new List<ModelNo>();

            model.PartNos = model.ModelId > 0
                ? _master.GetPartNoByModel(model.ModelId)
                : new List<PartNo>();

            // Search and Export run exactly the same query
            if (command == "Search" || command == "Export")
            {
                string date = null;

                if (model.FromDate.HasValue)
                {
                    date = model.FromDate.Value.ToString("MM/dd/yyyy");
                }

                // dtReports is not posted back from the browser,
                // so the same query is executed again for both Search and Export
                model.dtReports = _bal.GetGlueWeighingReport(
                    date,
                    model.LineId,
                    model.ShiftId,
                    model.ProjectId,
                    model.AdhesiveId
                );

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
                    string adhesive = model.Adhesives?.FirstOrDefault(x => x.AdhesiveId == model.AdhesiveId)?.AdhesiveName ?? "";
                    string dateText = model.FromDate?.ToString("dd/MM/yyyy") ?? "";

                    var bytes = _pdf.GenerateReport(
                        model.dtReports, line, shift, dateText, project, adhesive);

                    var fileName =
                        $"Glue_Weighing_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    return File(bytes, "application/pdf", fileName);
                }
            }

            return View(ViewPath, model);
        }
    }
}