using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Constants;

namespace TaskFlow.Controllers
{
    //this allow access to only superadmin users
    [Authorize(Policy = Policies.SuperAdminOnly)]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
