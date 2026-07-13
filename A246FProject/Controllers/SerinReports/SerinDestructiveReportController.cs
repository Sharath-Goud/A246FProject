using A246FProject.BAL;
using A246FProject.BAL.SerinReports;
using A246FProject.Models;
using A246FProject.Models.SerinReports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.SerinReports
{
    public class SerinDestructiveReportController : Controller
    {
        private readonly SerinDestructiveReportBAL _bal;
        private readonly MasterBAL _master;

        public SerinDestructiveReportController()
        {
            _bal = new SerinDestructiveReportBAL();
            _master = new MasterBAL();
        }

        [HttpGet]
        public IActionResult Index()
        {
            SerinDestructiveReportViewModel model = new();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Machines = _master.GetA246FMachines();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            return View("~/Views/SerinReports/SerinDestructiveReport.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SerinDestructiveReportViewModel model, string command)
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
                string date = model.FromDate;


                if (string.IsNullOrEmpty(date))
                {
                    model.dtReports = new DataTable();
                }
                else
                {
                    model.dtReports = _bal.GetDestructiveReport(
                        date,
                        model.LineId,
                        model.ShiftId,
                        model.ProjectId,
                        model.MachineId);
                }


                HttpContext.Session.SetString("SearchDone", "true");
            }

            return View("~/Views/SerinReports/SerinDestructiveReport.cshtml", model);
        }
    }
}