using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class SOPPORController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/SOPPOR/Index.cshtml");
        }
    }
}
