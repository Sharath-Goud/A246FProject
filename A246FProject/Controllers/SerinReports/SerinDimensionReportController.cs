using A246FProject.BAL;
using A246FProject.BAL.SerinReports;
using A246FProject.Models;
using A246FProject.Models.SerinReports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.SerinReports
{
    public class SerinDimensionReportController : Controller
    {
        private readonly SerinDimensionReportBAL _bal;
        private readonly MasterBAL _master;

        public SerinDimensionReportController()
        {
            _bal = new SerinDimensionReportBAL();
            _master = new MasterBAL();
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

            return View("~/Views/SerinReports/SerinDimensionReport.cshtml", model);
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

            if (command == "Search")
            {
                string date = model.FromDate;


                if (string.IsNullOrEmpty(date))
                {
                    model.dtReports = new DataTable();
                }
                else
                {
                    model.dtReports = _bal.GetDimensionReport(
                        date,
                        model.LineId,
                        model.ShiftId,
                        model.ProjectId,
                        model.MachineId);
                }


                HttpContext.Session.SetString("SearchDone", "true");
            }

            return View("~/Views/SerinReports/SerinDimensionReport.cshtml", model);
        }
    }
}