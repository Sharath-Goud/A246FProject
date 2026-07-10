using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models;
using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class VisualInspectionReportController : Controller
    {
        private readonly VisualInspectionReportBAL _bal;
        private readonly MasterBAL _master;

        public VisualInspectionReportController()
        {
            _bal = new VisualInspectionReportBAL();
            _master = new MasterBAL();
        }

        [HttpPost]
        public JsonResult GetVisuals(int projectId)
        {
            return Json(_master.GetVisuals(projectId));
        }

        [HttpGet]
        public IActionResult Index()
        {
            VisualInspectionReportViewModel model =
                new VisualInspectionReportViewModel();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Visualss = new List<Visuals>();
            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            return View(
                "~/Views/Reports/VisualInspectionReport.cshtml",
                model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(VisualInspectionReportViewModel model, string command)
        {
            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();

            model.Visualss = model.ProjectId > 0
                ? _master.GetVisuals(model.ProjectId)
                : new List<Visuals>();

            model.ModelNos = model.ProjectId > 0
                ? _master.GetModelNoByProject(model.ProjectId)
                : new List<ModelNo>();

            model.PartNos = model.ModelId > 0
                ? _master.GetPartNoByModel(model.ModelId)
                : new List<PartNo>();


            if (command == "Search")
            {
                var date = model.FromDate?.ToString("MM/dd/yyyy");

                model.dtReports = _bal.GetVisualInspectionReport(
                     date,
                     model.LineId,
                     model.ShiftId,
                     model.ProjectId,
                     model.VisualsId);
            }


            return View(
                "~/Views/Reports/VisualInspectionReport.cshtml",
                model);
        }
    }
}