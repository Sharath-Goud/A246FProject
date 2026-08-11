using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class ShellCrimpController : Controller
    {
        ShellCrimpBAL _bal = new();

        [HttpGet]
        public IActionResult Index()
        {
            var model = new ShellCrimpViewModel
            {
                Lines = _bal.GetLine(),
                Projects = _bal.GetProject(),
                ModelNos = _bal.GetModelNo(),
                Machines = _bal.GetMachines(),
                PartNos = new List<PartNo>(),
                dtChecklist = new DataTable()
            };

            return View("~/Views/A246FProject/ShellCrimp/Index.cshtml", model);
        }

        [HttpPost]
        public IActionResult Index(ShellCrimpViewModel model)
        {

            model.Lines = _bal.GetLine();

            model.Projects = _bal.GetProject();

            model.ModelNos = _bal.GetModelNo();

            model.Machines = _bal.GetMachines();

            model.PartNos = model.ModelId > 0
                ? _bal.GetPartNoByModel(model.ModelId)
                : new List<PartNo>();

            if (model.LineId <= 0 ||
                model.ProjectId <= 0 ||
                model.ModelId <= 0 ||
                model.PartId <= 0 ||
                model.MachineId <= 0 ||
                string.IsNullOrWhiteSpace(model.ProdLineLeader) ||
                string.IsNullOrWhiteSpace(model.CheckedBy) ||
                string.IsNullOrWhiteSpace(model.ApprovedBy))
            {

                ViewBag.Message =
                "Please enter Production Line Leader, Checked By and Approved By before Search.";

                model.dtChecklist = new DataTable();

                return View(
                    "~/Views/A246FProject/ShellCrimp/Index.cshtml",
                    model);
            }

            model.dtChecklist =
                _bal.GetShellCrimpData(
                    model.ProjectId,
                    model.LineId,
                    model.MachineId);

            return View(
                "~/Views/A246FProject/ShellCrimp/Index.cshtml",
                model);
        }

        [HttpPost]
        public JsonResult GetModelNoByProject(int projectId)
        {
            return Json(_bal.GetModelNoByProject(projectId));
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(int modelId)
        {
            return Json(_bal.GetPartNoByModel(modelId));
        }

        [HttpPost]
        public JsonResult Save([FromBody] ShellCrimpViewModel model)
        {
            if (model?.ShellCrimpResults == null)
                return Json(0);

            DataTable dt = new();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("SectionId", typeof(int));
            dt.Columns.Add("Value1", typeof(decimal));
            dt.Columns.Add("Value2", typeof(decimal));
            dt.Columns.Add("Value3", typeof(decimal));
            dt.Columns.Add("Value4", typeof(decimal));
            dt.Columns.Add("Value5", typeof(decimal));
            dt.Columns.Add("InspectionResults", typeof(string));
            dt.Columns.Add("CablesSerialNumber", typeof(string));

            int i = 1;

            foreach (var r in model.ShellCrimpResults)
            {
                dt.Rows.Add(i++, r.SectionId,
                    r.Value1 ?? 0,
                    r.Value2 ?? 0,
                    r.Value3 ?? 0,
                    r.Value4 ?? 0,
                    r.Value5 ?? 0,
                    r.InspectionResults,
                    r.CablesSerialNumber);
            }

            string user = HttpContext.Session.GetString("User");

            int result = _bal.InsertBulkShellCrimp(
                dt,
                model.LineId,
                model.ProjectId,
                model.ModelId,
                model.PartId,
                model.ProdLineLeader,
                model.CheckedBy,
                model.ApprovedBy,
                user
            );

            return Json(result);
        }
    }
}