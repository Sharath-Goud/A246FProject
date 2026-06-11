using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class ShellCrimpController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/ShellCrimp/Index.cshtml");
        }
    }
}
