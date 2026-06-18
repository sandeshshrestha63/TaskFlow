using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Constants;
using TaskFlow.Models;

namespace TaskFlow.Controllers
{
    //this allow access to only superadmin users
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            if (User.IsInRole(Roles.SuperAdmin))
                return RedirectToAction(nameof(SuperAdmin));

            if (User.IsInRole(Roles.CompanyAdmin))
                return RedirectToAction(nameof(CompanyAdmin));

            return RedirectToAction(nameof(Employee));
        }

        [Authorize(Policy = Policies.SuperAdminOnly)]
        public async Task<IActionResult> SuperAdmin()
        {
            // TODO: replace with real DB counts later
            ViewBag.TotalCompanies = 12;
            ViewBag.TotalUsers = 48;

            return View();
        }

        [Authorize(Policy = Policies.CompanyAccess)]
        public async Task<IActionResult> CompanyAdmin()
        {
            var user = await _userManager.GetUserAsync(User);

            ViewBag.CompanyName = "My Company";
            ViewBag.TotalEmployees = 8;
            ViewBag.TotalTasks = 25;

            return View();
        }

        [Authorize(Policy = Policies.EmployeeAccess)]
        public async Task<IActionResult> Employee()
        {
            var user = await _userManager.GetUserAsync(User);

            ViewBag.TotalTasks = 5;
            ViewBag.PendingTasks = 2;

            return View();
        }
    }
}
