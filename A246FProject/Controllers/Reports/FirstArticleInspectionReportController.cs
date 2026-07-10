using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models;
using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class FirstArticleInspectionReportController : Controller
    {
        private readonly FirstArticleInspectionReportBAL _bal;
        private readonly MasterBAL _master;

        public FirstArticleInspectionReportController()
        {
            _bal = new FirstArticleInspectionReportBAL();
            _master = new MasterBAL();
        }

        [HttpGet]
        public IActionResult Index()
        {
            FirstArticleInspectionReportViewModel model = new();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Machines = _master.GetA246FMachines();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            return View("~/Views/Reports/FirstArticleInspectionReport.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(FirstArticleInspectionReportViewModel model, string command)
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

            if (command == "Search")
            {
                var date = model.FromDate?.ToString("MM/dd/yyyy");

                model.dtReports = _bal.GetFirstArticleInspectionReport(
                    date,
                    model.LineId,
                    model.ShiftId,
                    model.ProjectId);

                HttpContext.Session.SetString("SearchDone", "true");
            }

            return View("~/Views/Reports/FirstArticleInspectionReport.cshtml", model);
        }
    }
}