using A246FProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class FirstArticleInspectionController : Controller
    {
        private readonly FirstArticleInspectionBAL _bal;

        public FirstArticleInspectionController()
        {
            _bal = new FirstArticleInspectionBAL();
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

        [HttpGet]
        public IActionResult Index()
        {
            FirstArticleInspectionViewModel model = new();

            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            ViewBag.CreatedBy = HttpContext.Session.GetString("User");

            return View(
                "~/Views/A246FProject/FirstArticleInspection/Index.cshtml",
                model);
        }

        [HttpPost]
        public JsonResult GetInspectionData(int lineId, int projectId)
        {

            if (lineId <= 0 || projectId <= 0)
            {
                return Json(new List<object>());
            }

            var data =
                _bal.GetInspectionData(
                    lineId,
                    projectId);

            return Json(data);
        }

        [HttpPost]
        public JsonResult SaveInspection(SaveInspectionDto model)
        {
            int i = _bal.SaveInspection(model);

            return Json(new
            {
                success = i > 0
            });
        }

        [HttpPost]
        public IActionResult SubmitInspection([FromBody] FirstArticleInspectionViewModel model)
        {
            int i = _bal.SubmitInspection(model);

            if (i > 0)
                return Json(new { success = true });

            return Json(new { success = false });
        }
    }
}