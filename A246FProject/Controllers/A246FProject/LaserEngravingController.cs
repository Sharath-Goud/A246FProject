using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class LaserEngravingController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/LaserEngraving/Index.cshtml");
        }
    }
}
