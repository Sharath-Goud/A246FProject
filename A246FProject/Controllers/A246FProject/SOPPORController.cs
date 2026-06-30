using A246FProject.BAL;
using A246FProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class SOPPORController : Controller
    {
        A246FCTPParameterBAL _commonBAL =
            new A246FCTPParameterBAL();

        SOPPORBAL _bal =
            new SOPPORBAL();

        [HttpGet]
        public IActionResult Index()
        {
            SOPPORViewModel model =
                new SOPPORViewModel();

            model.dtChecklist =
                new DataTable();

            model.Lines =
                _commonBAL.GetLine();

            model.Projects =
                _commonBAL.GetProject();

            model.ModelNos =
                new List<ModelNo>();

            model.PartNos =
                new List<PartNo>();

            model.Statuses =
                _bal.GetResult();

            return View(
                "~/Views/A246FProject/SOPPOR/Index.cshtml",
                model);
        }

        [HttpGet]
        public JsonResult GetModels(
            int projectId)
        {
            return Json(
                _commonBAL
                .GetModelNoByProject(
                    projectId));
        }

        [HttpGet]
        public JsonResult GetParts(
            int modelId)
        {
            return Json(
                _commonBAL
                .GetPartNoByModel(
                    modelId));
        }

        [HttpPost]
        public IActionResult Index(SOPPORViewModel model)
        {
            model.Lines = _commonBAL.GetLine();
            model.Projects = _commonBAL.GetProject();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            model.Statuses = _bal.GetResult();

            model.dtChecklist =
                _bal.GetSOPPORData(model.LineId, model.ProjectId);

            return View("~/Views/A246FProject/SOPPOR/Index.cshtml", model);
        }

        
    }
}