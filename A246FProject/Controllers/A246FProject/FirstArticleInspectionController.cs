using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class FirstArticleInspectionController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/FirstArticleInspection/Index.cshtml");
        }
    }
}
