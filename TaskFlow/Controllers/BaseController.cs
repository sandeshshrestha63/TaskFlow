using Microsoft.AspNetCore.Mvc;
using TaskFlow.Constants;
using TaskFlow.Interfaces;
using TaskFlow.Services;

namespace TaskFlow.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ICurrentUserServices CurrentUser;

        protected BaseController(ICurrentUserServices currentUser)
        {
            CurrentUser = currentUser;
        }

        protected int? CompanyId => CurrentUser.CompanyId;

        protected string? UserId => CurrentUser.UserId;

        protected string? UserEmail => CurrentUser.Email;

        protected bool IsSuperAdmin => CurrentUser.IsInRole(Roles.SuperAdmin);

        protected bool IsCompanyAdmin => CurrentUser.IsInRole(Roles.CompanyAdmin);

        protected bool IsEmployee => CurrentUser.IsInRole(Roles.Employee);

        protected void SuccessMessage(string message)
        {
            TempData["success"] = message;
        }

        protected void ErrorMessage(string message)
        {
            TempData["error"] = message;
        }
    }
}
