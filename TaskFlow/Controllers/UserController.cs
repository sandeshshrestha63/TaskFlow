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
    [Authorize(Policy = Policies.CompanyAccess)]
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
            if(IsCompanyAdmin)
            {
                users = users.Where(a => a.CompanyId == CompanyId).ToList();
            }
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FullName?.Split(" ").FirstOrDefault(),
                    LastName = user.FullName?.Split(" ").LastOrDefault(),
                    Email = user.Email,
                    CompanyName = user.Company?.Name,
                    Role = roles.FirstOrDefault(),
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                });
            }
            return View(model);
        }

        public IActionResult Create(int Id)
        {
            var model = new UserViewModel
            {
                CompanyId = Id,
                Role = Roles.CompanyAdmin // default for this flow
            };
            if(IsCompanyAdmin)
            {
                model.Role = Roles.Employee;
                model.CompanyId = CompanyId;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.CompanyAccess)]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FirstName + " " + model.LastName ,
                CompanyId = model.CompanyId,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                CompanyId = model.CompanyId.Value,
                CreatedAt = DateTime.UtcNow,
                ApplicationUserId = user.Id,
            };
            _context.Employees.Add(employee);
            _context.SaveChanges();
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }
            await _userManager.AddToRoleAsync(user, model.Role);
            SuccessMessage("Company Admin Created successfully");
            return RedirectToAction("Index", "User");
        }
    }
}
