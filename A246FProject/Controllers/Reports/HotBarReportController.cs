using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.DAL.Reports;
using A246FProject.Models;
using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class HotBarReportController : Controller
    {
        private readonly HotBarReportBAL _bal;
        private readonly MasterBAL _master;

        public HotBarReportController()
        {
            _bal = new HotBarReportBAL();
            _master = new MasterBAL();
        }

        [HttpGet]
        public IActionResult Index()
        {
            HotBarReportViewModel model = new();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Machines = _master.GetA246FMachines();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            return View("~/Views/Reports/HotBarReport.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(HotBarReportViewModel model, string command)
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

                int shiftId = model.ShiftId;

                model.dtReports = _bal.GetHotBarReport(
                    date,
                    model.LineId,
                    shiftId,
                    model.ProjectId);

                HttpContext.Session.SetString("SearchDone", "true");
            }

            return View("~/Views/Reports/HotBarReport.cshtml", model);
        }
    }
}
