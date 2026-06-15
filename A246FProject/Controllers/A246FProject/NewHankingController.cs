using Microsoft.AspNetCore.Mvc;

namespace A246FProject.Controllers.A246FProject
{
    public class NewHankingController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/A246FProject/NewHanking/Index.cshtml");
        }
    }
}
