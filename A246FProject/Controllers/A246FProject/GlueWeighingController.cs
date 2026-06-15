using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class GlueWeighingController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/GlueWeighing/Index.cshtml");
        }
    }
}
