using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace A246FProject.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("Name");
            ViewBag.User = HttpContext.Session.GetString("User");
            return View();
        }
    }
}