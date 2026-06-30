using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models;
using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class CCMCHI4SCReportController : Controller
    {
        private readonly CCMCHI4SCReportBAL _bal;
        private readonly MasterBAL _master;

        public CCMCHI4SCReportController()
        {
            _bal = new CCMCHI4SCReportBAL();
            _master = new MasterBAL();
        }

        [HttpGet]
        public IActionResult Index()
        {
            CCMCHI4SCReportViewModel model = new();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Machines = _master.GetA246FMachines();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            return View("~/Views/Reports/CCMCHI4SCReport.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CCMCHI4SCReportViewModel model, string command)
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

                model.dtReports = _bal.GetCCMCHI4SCReport(
                    date,
                    model.LineId,
                    model.ShiftId,
                    model.ProjectId,
                    model.MachineId);

                HttpContext.Session.SetString("SearchDone", "true");
            }

            return View("~/Views/Reports/CCMCHI4SCReport.cshtml", model);
        }
    }
}