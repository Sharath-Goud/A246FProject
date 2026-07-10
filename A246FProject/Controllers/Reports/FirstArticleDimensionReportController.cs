using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models;
using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class FirstArticleDimensionReportController : Controller
    {
        private readonly FirstArticleDimensionReportBAL _bal;
        private readonly MasterBAL _master;

        public FirstArticleDimensionReportController()
        {
            _bal = new FirstArticleDimensionReportBAL();
            _master = new MasterBAL();
        }

        [HttpGet]
        public IActionResult Index()
        {
            FirstArticleDimensionReportViewModel model =
                new FirstArticleDimensionReportViewModel();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Machines = _master.GetA246FMachines();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            return View(
                "~/Views/Reports/FirstArticleDimensionReport.cshtml",
                model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(FirstArticleDimensionReportViewModel model, string command)
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

                model.dtReports = _bal.GetFirstArticleDimensionReport(
                    date,
                    model.LineId,
                    model.ShiftId,
                    model.ProjectId);

                HttpContext.Session.SetString("SearchDone", "true");
            }

            return View("~/Views/Reports/FirstArticleDimensionReport.cshtml", model);
        }
    }
}