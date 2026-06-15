using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class HankingController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/Hanking/Index.cshtml");
        }
    }
}
