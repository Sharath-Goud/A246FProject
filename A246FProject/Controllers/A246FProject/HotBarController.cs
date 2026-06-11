using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class HotBarController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/HotBar/Index.cshtml");
        }
    }
}
