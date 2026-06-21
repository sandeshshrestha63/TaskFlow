using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Data;
using TaskFlow.Interfaces;
using TaskFlow.Models;

namespace TaskFlow.Controllers
{
    public class CompanyAdminController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CompanyAdminController(AppDbContext context,ICurrentUserServices currentUser,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager) : base(currentUser, context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
    }
}
