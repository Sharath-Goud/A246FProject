using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class TrapTestReportController : Controller
    {
        private readonly TrapTestReportBAL _bal;
        private readonly MasterBAL _master;

        public TrapTestReportController()
        {
            _bal = new TrapTestReportBAL();
            _master = new MasterBAL();
        }

        [HttpGet]
        public IActionResult Index()
        {
            TrapTestReportViewModel model = new();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Machines = _master.GetA246FMachines();

            model.ModelNos = new();
            model.PartNos = new();

            return View(
                "~/Views/Reports/TrapTestReport.cshtml",
                model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(
            TrapTestReportViewModel model,
            string command)
        {
            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Machines = _master.GetA246FMachines();

            model.ModelNos = model.ProjectId > 0
                ? _master.GetModelNoByProject(model.ProjectId)
                : new();

            model.PartNos = model.ModelId > 0
                ? _master.GetPartNoByModel(model.ModelId)
                : new();

            if (command == "Search")
            {
                var date = model.FromDate?.ToString("MM/dd/yyyy");

                model.dtReports = _bal.GetTrapTestReport(
                    date,
                    model.LineId,
                    model.ShiftId,
                    model.ProjectId);

                HttpContext.Session.SetString(
                    "SearchDone",
                    "true");
            }

            return View(
                "~/Views/Reports/TrapTestReport.cshtml",
                model);
        }
    }
}