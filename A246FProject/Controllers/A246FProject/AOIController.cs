using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class AOIController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/AOI/Index.cshtml");
        }
    }
}
