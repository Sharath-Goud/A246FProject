using A246FProject.BAL;
using A246FProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class OQCInspectionController : Controller
    {
        OQCInspectionBAL _bal = new OQCInspectionBAL();

        [HttpGet]
        public IActionResult Index()
        {
            OQCInspectionViewModel model =
                new OQCInspectionViewModel();

            model.Projects = _bal.GetProject();

            model.dtChecklist = new DataTable();

            return View(
                "~/Views/A246FProject/OQCInspection/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult Index(OQCInspectionViewModel model)
        {
            model.Projects = _bal.GetProject();

            if (model.ProjectId.HasValue)
            {
                model.dtChecklist =
                    _bal.GetInspectionData(model.ProjectId.Value);
            }
            else
            {
                model.dtChecklist = new DataTable();
            }

            return View(
                "~/Views/A246FProject/OQCInspection/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult Save([FromBody] OQCInspectionViewModel model)
        {
            model.CreatedBy = HttpContext.Session.GetString("User") ?? "";

            model.InspectResult =
                model.SamplingInspection ||
                model.HundredPercentInspection ||
                model.OtherInspection;

            _bal.SaveInspection(model);

            return Json(new
            {
                success = true
            });
        }
    }
}