using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class NegativeValidationReportController : Controller
    {
        private readonly NegativeValidationReportBAL _bal =
            new NegativeValidationReportBAL();

        private readonly MasterBAL _master =
            new MasterBAL();

        [HttpGet]
        public IActionResult Index()
        {
            NegativeValidationReportViewModel model = new();

            model.dtReports = new DataTable();
            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();

            return View("~/Views/Reports/NegativeValidationReport.cshtml", model);
        }

        [HttpPost]
        public IActionResult Index(
            NegativeValidationReportViewModel model,
            string command)
        {
            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();

            if (command == "Search")
            {
                model.dtReports = _bal.GetReport(
                    model.FromDate.Value.ToString("MM/dd/yyyy"),
                    model.LineId,
                    model.ShiftId,
                    model.ProjectId);
            }

            return View("~/Views/Reports/NegativeValidationReport.cshtml", model);
        }
    }
}
