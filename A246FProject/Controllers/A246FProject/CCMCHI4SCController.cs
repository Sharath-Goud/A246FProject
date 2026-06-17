using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL;
using A246FProject.Models;

namespace A246FProject.Controllers.A246FProject
{
    public class CCMCHI4SCController : Controller
    {
        CCMCHI4SCBAL _bal =
            new CCMCHI4SCBAL();

        [HttpGet]
        public IActionResult Index()
        {
            CCMCHI4SCViewModel model =
                new CCMCHI4SCViewModel();

            model.Lines = _bal.GetLine();

            model.Projects = _bal.GetProject();

            model.ModelNos = _bal.GetModelNo();

            model.Machines = _bal.GetMachines();

            model.PartNos =
                new List<PartNo>();

            return View(
                "~/Views/A246FProject/CCMCHI4SC/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult Index(
            CCMCHI4SCViewModel model)
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
                model.PartNos =
                    new List<PartNo>();
            }

            model.dtChecklist =
                _bal.GetCCMCHI4SCData(
                    model.LineId,
                    model.ProjectId,
                    model.MachineId);

            return View(
                "~/Views/A246FProject/CCMCHI4SC/Index.cshtml",
                model);
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(
            int modelId)
        {
            return Json(
                _bal.GetPartNoByModel(modelId));
        }

        [HttpPost]
        public JsonResult GetModelNoByProject(
            int projectId)
        {
            return Json(
                _bal.GetModelNoByProject(projectId));
        }
    }
}