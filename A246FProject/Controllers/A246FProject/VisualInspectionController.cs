using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class VisualInspectionController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/VisualInspection/Index.cshtml");
        }
    }
}
