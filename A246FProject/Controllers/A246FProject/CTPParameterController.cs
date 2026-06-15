using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL;
using A246FProject.Models;

namespace A246FProject.Controllers.A246FProject
{
    public class CTPParameterController : Controller
    {
        A246FCTPParameterBAL _bal =
            new A246FCTPParameterBAL();

        [HttpGet]
        public IActionResult Index()
        {
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
            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.Machines = _bal.GetMachines();
            model.ModelNos = _bal.GetModelNo();

            model.PartNos = new List<PartNo>();

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
    }
}