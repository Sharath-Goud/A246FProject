using A246FProject.BAL;
using A246FProject.BAL.Reports;
using A246FProject.Models;
using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class GlueWeighingReportController : Controller
    {
        private readonly GlueWeighingReportBAL _bal;
        private readonly MasterBAL _master;

        public GlueWeighingReportController()
        {
            _bal = new GlueWeighingReportBAL();
            _master = new MasterBAL();
        }

        public List<Adhesive> GetAdhesiveByProject(int projectId)
        {
            return _master.GetAdhesive(projectId);
        }

        [HttpGet]
        public IActionResult Index()
        {
            GlueWeighingReportViewModel model = new();

            model.dtReports = new DataTable();

            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Adhesives = new List<Adhesive>();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            return View("~/Views/SerinReports/GlueWeighingReport.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(GlueWeighingReportViewModel model, string command)
        {
            model.Lines = _master.GetLine();
            model.Shifts = _master.GetShift();
            model.Projects = _master.GetProject();
            model.Adhesives = model.ProjectId > 0
                ? GetAdhesiveByProject(model.ProjectId)
                : new List<Adhesive>();

            model.ModelNos = model.ProjectId > 0
                ? _master.GetModelNoByProject(model.ProjectId)
                : new List<ModelNo>();

            model.PartNos = model.ModelId > 0
                ? _master.GetPartNoByModel(model.ModelId)
                : new List<PartNo>();

            if (command == "Search")
            {
                string date = null;

                if (model.FromDate.HasValue)
                {
                    date = model.FromDate.Value.ToString("MM/dd/yyyy");
                }


                model.dtReports = _bal.GetGlueWeighingReport(
                    date,
                    model.LineId,
                    model.ShiftId,
                    model.ProjectId,
                    model.AdhesiveId
                );


                HttpContext.Session.SetString("SearchDone", "true");
            }

            return View("~/Views/SerinReports/GlueWeighingReport.cshtml", model);
        }
    }
}