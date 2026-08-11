using A246FProject.BAL;
using A246FProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class FirstArticleDimensionController : Controller
    {
        private readonly FirstArticleDimensionBAL _bal;

        public FirstArticleDimensionController()
        {
            _bal = new FirstArticleDimensionBAL();
        }

        [HttpGet]
        public IActionResult Index()
        {
            FirstArticleDimensionViewModel model = new();

            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            var user = HttpContext.Session.GetString("User");

            ViewBag.CreatedBy = user;

            return View(
                "~/Views/A246FProject/FirstArticleDimension/Index.cshtml",
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
        public JsonResult GetDimensionData(int lineId, int projectId)
        {
            return Json(_bal.GetDimensionData(lineId, projectId));
        }

        [HttpPost]
        public JsonResult SaveDimension(SaveDimensionDto model)
        {
            try
            {
                int i = _bal.SaveDimension(model);

                return Json(new
                {
                    success = i > 0,
                    message = i > 0 ? "Saved Successfully" : "Save Failed"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public JsonResult SubmitDimension([FromBody] FirstArticleDimensionViewModel model)
        {
            try
            {
                int i = _bal.SubmitDimension(model);

                return Json(new
                {
                    success = i > 0,
                    message = i > 0 ? "Submitted Successfully" : "Submit Failed"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}