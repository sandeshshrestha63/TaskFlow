using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Constants;

namespace TaskFlow.Controllers
{
    [Authorize(Policy = Policies.EmployeeAccess)]
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
