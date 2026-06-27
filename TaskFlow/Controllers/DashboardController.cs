using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Constants;
using TaskFlow.Data;
using TaskFlow.Interfaces;

namespace TaskFlow.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(AppDbContext context, ICurrentUserServices currentUser, IDashboardService dashboardService)
            : base(currentUser, context)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(Roles.SuperAdmin))
            {
                var vm = await _dashboardService.GetSuperAdminDashboardAsync();
                return View("SuperAdmin", vm);
            }

            if (User.IsInRole(Roles.CompanyAdmin))
            {
                var vm = await _dashboardService.GetCompanyDashboardAsync(CompanyId);
                return View("CompanyAdmin", vm);
            }

            var employeeVm = await _dashboardService.GetEmployeeDashboardAsync(CompanyId, EmployeeId);

            return View("Employee", employeeVm);
        }
    }
}