using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class DestructiveController : Controller
    {
        DestructiveBAL _bal = new();

        [HttpGet]
        public IActionResult Index()
        {
            DestructiveViewModel model =
                new DestructiveViewModel();

            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.ModelNos = _bal.GetModelNo();
            model.Machines = _bal.GetMachines();
            model.PartNos = new List<PartNo>();
            model.dtChecklist = new DataTable();
            return View(
                "~/Views/A246FProject/Destructive/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult Index(
            DestructiveViewModel model)
        {
            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.ModelNos = _bal.GetModelNo();
            model.Machines = _bal.GetMachines();

            if (model.ModelId > 0)
            {
                model.PartNos =
                    _bal.GetPartNoByModel(model.ModelId);
            }
            else
            {
                model.PartNos = new List<PartNo>();
            }

            model.dtChecklist =
                _bal.GetDestructiveData(
                    model.LineId,
                    model.ProjectId,
                    model.MachineId);

            return View(
                "~/Views/A246FProject/Destructive/Index.cshtml",
                model);
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(int modelId)
        {
            return Json(
                _bal.GetPartNoByModel(modelId));
        }

        [HttpPost]
        public JsonResult GetModelNoByProject(int projectId)
        {
            return Json(
                _bal.GetModelNoByProject(projectId));
        }

        [HttpPost]
        [HttpPost]
        public JsonResult SaveA246FMHB2CheckList([FromBody] DestructiveViewModel model)
        {
            try
            {
                if (model == null || model.A246FMHB2CheckListResults == null)
                    return Json(0);

                DataTable dtChecklist = new DataTable();
                dtChecklist.Columns.Add("Id", typeof(int));
                dtChecklist.Columns.Add("SectionId", typeof(int));
                dtChecklist.Columns.Add("Value1", typeof(decimal));
                dtChecklist.Columns.Add("InspectionResults", typeof(string));

                int id = 1;

                foreach (var row in model.A246FMHB2CheckListResults)
                {
                    dtChecklist.Rows.Add(
                        id,
                        row.SectionId,
                        row.Value1,
                        row.InspectionResults ?? "OK"
                    );

                    id++;
                }

                string user = HttpContext.Session.GetString("User");

                if (string.IsNullOrEmpty(user))
                {
                    return Json("Session Expired");
                }

                int result = _bal.InsertBulkA246FMHB2CheckList(
                    dtChecklist,
                    user,
                    model.LineId,
                    model.ProdLineLeader,
                    model.CheckedBy,
                    model.ApprovedBy,
                    model.ModelId,
                    model.PartId
                );

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }
    }
}