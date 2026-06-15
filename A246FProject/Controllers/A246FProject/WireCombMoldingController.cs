using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class WireCombMoldingController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/WireCombMolding/Index.cshtml");
        }
    }
}
