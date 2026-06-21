using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskFlow.Constants;
using TaskFlow.Data;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.ViewModels;

namespace TaskFlow.Controllers
{
    [Authorize(Policy = Policies.SuperAdminOnly)]
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public UserController(UserManager<ApplicationUser> userManager,AppDbContext context,ICurrentUserServices currentUser) : base(currentUser,context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(x => x.Company).ToListAsync();

            var model = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    CompanyName = user.Company?.Name,
                    Role = roles.FirstOrDefault(),
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                });
            }
            return View(model);
        }

        [Authorize(Policy = Policies.SuperAdminOnly)]
        public IActionResult Create(int Id)
        {
            var model = new UserViewModel
            {
                CompanyId = Id,
                Role = Roles.CompanyAdmin // default for this flow
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.SuperAdminOnly)]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                CompanyId = model.CompanyId,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }
            await _userManager.AddToRoleAsync(user, model.Role);
            SuccessMessage("Company Admin Created successfully");
            return RedirectToAction("Index", "Company");
        }
    }
}
