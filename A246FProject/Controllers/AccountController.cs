using Microsoft.AspNetCore.Mvc;
using A246FProject.Data;
using A246FProject.Models;

namespace A246FProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (string.IsNullOrEmpty(user.EmployeeId) || string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            if (user.Password != user.ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            if (string.IsNullOrEmpty(user.Role))
            {
                ViewBag.Error = "Please select Role.";
                return View(user);
            }

            var exists = _context.Users
                .FirstOrDefault(x => x.EmployeeId == user.EmployeeId);

            if (exists != null)
            {
                ViewBag.Error = "Employee ID already registered.";
                return View();
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string employeeId, string password)
        {
            if (string.IsNullOrEmpty(employeeId) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            var user = _context.Users
                .FirstOrDefault(x => x.EmployeeId == employeeId &&
                                     x.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("User", user.EmployeeId);
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("IsLoggedIn", "true");
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid Employee ID or Password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}