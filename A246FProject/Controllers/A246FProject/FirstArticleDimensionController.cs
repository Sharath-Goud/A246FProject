using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class FirstArticleDimensionController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/FirstArticleDimension/Index.cshtml");
        }
    }
}
