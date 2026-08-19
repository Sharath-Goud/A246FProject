using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models;
using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;

namespace A246FProject.Controllers.Reports
{
    public class NewHankingReportController : Controller
    {
        private readonly NewHankingReportBAL _bal;
        private readonly MasterBAL _master;

        public NewHankingReportController()
        {
            _bal = new NewHankingReportBAL();
            _master = new MasterBAL();
        }

        [HttpPost]
        public JsonResult GetModelNoByProject(int projectId)
        {
            return Json(_master.GetModelNoByProject(projectId));
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(int modelId)
        {
            return Json(_master.GetPartNoByModel(modelId));
        }

        [HttpGet]
        public IActionResult Index()
        {
            NewHankingReportViewModel model = new NewHankingReportViewModel();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();

            model.Shifts = _master.GetShift();

            model.Projects = _master.GetProject();

            model.Machines = _master.GetA246FMachines();

            model.ModelNos = new List<ModelNo>();

            model.PartNos = new List<PartNo>();

            return View(
                "~/Views/Reports/NewHankingReport.cshtml",
                model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(
            NewHankingReportViewModel model,
            string command)
        {
            // Reload dropdown data
            model.Lines = _master.GetLine();

            model.Shifts = _master.GetShift();

            model.Projects = _master.GetProject();

            model.Machines = _master.GetA246FMachines();

            // Reload Model dropdown
            if (model.ProjectId > 0)
            {
                model.ModelNos =
                    _master.GetModelNoByProject(model.ProjectId);
            }
            else
            {
                model.ModelNos = new List<ModelNo>();
            }

            // Reload Part dropdown
            if (model.ModelId > 0)
            {
                model.PartNos =
                    _master.GetPartNoByModel(model.ModelId);
            }
            else
            {
                model.PartNos = new List<PartNo>();
            }

            // Search button
            if (command == "Search")
            {
   
                string rawDate = Request.Form["FromDate"].ToString();

                DateTime parsedDate;

                if (string.IsNullOrWhiteSpace(rawDate))
                {
                    ModelState.AddModelError(
                        "FromDate",
                        "Please select Date.");

                    model.dtReports = new DataTable();

                    return View(
                        "~/Views/Reports/NewHankingReport.cshtml",
                        model);
                }

                if (!DateTime.TryParseExact(
                        rawDate,
                        new[]
                        {
                        "MM/dd/yyyy",
                        "MM/dd/yy"
                        },
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out parsedDate))
                {
                    ModelState.AddModelError(
                        "FromDate",
                        "Invalid Date. Please select a valid date.");

                    model.dtReports = new DataTable();

                    return View(
                        "~/Views/Reports/NewHankingReport.cshtml",
                        model);
                }

                // IMPORTANT
                model.FromDate = parsedDate.Date;

                // Validate Line
                if (model.LineId <= 0)
                {
                    ModelState.AddModelError(
                        "LineId",
                        "Please select Line.");

                    model.dtReports = new DataTable();

                    return View(
                        "~/Views/Reports/NewHankingReport.cshtml",
                        model);
                }

                // Validate Shift
                // ShiftId = 0 is a valid Shift in your SP,
                // so DO NOT check ShiftId <= 0 here.

                // Validate Project
                if (model.ProjectId <= 0)
                {
                    ModelState.AddModelError(
                        "ProjectId",
                        "Please select Project.");

                    model.dtReports = new DataTable();

                    return View(
                        "~/Views/Reports/NewHankingReport.cshtml",
                        model);
                }

                // Validate Machine
                if (model.MachineId <= 0)
                {
                    ModelState.AddModelError(
                        "MachineId",
                        "Please select Machine.");

                    model.dtReports = new DataTable();

                    return View(
                        "~/Views/Reports/NewHankingReport.cshtml",
                        model);
                }

                // Validate Part
                if (model.PartId <= 0)
                {
                    ModelState.AddModelError(
                        "PartId",
                        "Please select Part Number.");

                    model.dtReports = new DataTable();

                    return View(
                        "~/Views/Reports/NewHankingReport.cshtml",
                        model);
                }

                // Call BAL
                model.dtReports =
                    _bal.GetNewHankingReport(
                        model.FromDate,
                        model.LineId,
                        model.ShiftId,
                        model.ProjectId,
                        model.MachineId,
                        model.PartId);

                HttpContext.Session.SetString(
                    "SearchDone",
                    "true");
            }

            return View(
                "~/Views/Reports/NewHankingReport.cshtml",
                model);
        }
    }
}