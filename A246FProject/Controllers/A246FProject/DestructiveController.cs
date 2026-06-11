using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class DestructiveController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/Destructive/Index.cshtml");
        }
    }
}
