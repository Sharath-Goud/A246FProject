using A246FProject.BAL.Reports;
using A246FProject.BAL;
using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class OQCInspectionReportController : Controller
    {
        private readonly OQCInspectionReportBAL _bal;
        private readonly MasterBAL _master;

        public OQCInspectionReportController()
        {
            _bal = new OQCInspectionReportBAL();
            _master = new MasterBAL();
        }

        [HttpGet]
        public IActionResult Index()
        {
            OQCInspectionReportViewModel model = new();

            model.dtReports = new DataTable();

            return View("~/Views/Reports/OQCInspectionReport.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(OQCInspectionReportViewModel model, string command)
        {
            if (command == "Search")
            {
                string fromDate = model.FromDate?.ToString("yyyy-MM-dd");
                string toDate = model.ToDate?.ToString("yyyy-MM-dd");

                model.dtReports = _bal.GetOQCInspectionReport(
                    fromDate,
                    toDate,
                    model.TrackNumber);

                HttpContext.Session.SetString("SearchDone", "true");
            }

            return View("~/Views/Reports/OQCInspectionReport.cshtml", model);
        }
    }
}
