using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class CTPParameterController : Controller
    {
        A246FCTPParameterBAL _bal =
            new A246FCTPParameterBAL();

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Role =
       HttpContext.Session.GetString("Role");
            A246FCTPParameterViewModel model =
                new A246FCTPParameterViewModel();

            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.Machines = _bal.GetMachines();
            model.ModelNos = _bal.GetModelNo();

            model.PartNos = new List<PartNo>();

            return View(
                "~/Views/A246FProject/A246FCTPParameter/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult Index(
            A246FCTPParameterViewModel model)
        {
            ViewBag.Role =
       HttpContext.Session.GetString("Role");
            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.Machines = _bal.GetMachines();
            model.ModelNos = _bal.GetModelNo();
            if (model.ModelId > 0)
            {
                model.PartNos =
                    _bal.GetPartNoByModel(model.ModelId);
            }
            else
            {
                model.PartNos =
                    new List<PartNo>();
            }

            model.dtChecklist =
                _bal.GetCTPParameterData(
                    model.LineId,
                    model.ProjectId,
                    model.MachineId);

            return View(
                "~/Views/A246FProject/A246FCTPParameter/Index.cshtml",
                model);
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(int modelId)
        {
            var partNos = _bal.GetPartNoByModel(modelId);

            return Json(partNos);
        }


        [HttpPost]
        public JsonResult GetModelNoByProject(int projectId)
        {
            var models = _bal.GetModelNoByProject(projectId);

            return Json(models);
        }

        [HttpPost]
        public JsonResult SaveA246FCTPParameter(
    [FromBody] A246FCTPParameterViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json("Model is null");
                }

                if (model.A246FCTPParameterResults == null ||
                    model.A246FCTPParameterResults.Length == 0)
                {
                    return Json("No records found");
                }

                DataTable dtChecklist = new DataTable();

                dtChecklist.Columns.Add("Id", typeof(int));
                dtChecklist.Columns.Add("LimitId", typeof(int));
                dtChecklist.Columns.Add("Value", typeof(decimal));

                int uid = 1;

                foreach (var row in model.A246FCTPParameterResults)
                {
                    dtChecklist.Rows.Add(
                        uid,
                        row.LimitId,
                        row.Value);

                    uid++;
                }

                int result =
                    _bal.InsertBulkA246FCTPParameter(
                        dtChecklist,
                        HttpContext.Session.GetString("User"),
                        model.LineId,
                        model.ProdLineLeader,
                        model.CheckedBy,
                        model.ApprovedBy,
                        model.ModelId,
                        model.PartId);

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}