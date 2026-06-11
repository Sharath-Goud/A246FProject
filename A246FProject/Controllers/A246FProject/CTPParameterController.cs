using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class CTPParameterController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/A246FCTPParameter/Index.cshtml");
        }
    }
}