using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TaskFlow.Data;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.ViewModels;

namespace TaskFlow.Controllers
{
    public class EmployeeTaskController : BaseController
    {
        public EmployeeTaskController(ICurrentUserServices currentUser, AppDbContext db) : base(currentUser,db)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();

            return View(new EmployeeTaskDataVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeTaskDataVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(vm);
            }

            var defaultStatus = await _db.EmployeeTaskStatus.Where(x =>  x.CompanyId == CompanyId.Value && x.IsDefault && x.IsActive).FirstOrDefaultAsync();

            if (defaultStatus == null)
            {
                TempData["Error"] = "No default task status has been configured.";
                await LoadDropdowns();
                return View(vm);
            }
            var employeeTask = new EmployeeTask
            {
                Title = vm.Title,
                Description = vm.Description,

                CompanyId = CompanyId.Value,

                CreatedByEmployeeId = EmployeeId.Value,

                AssignedToEmployeeId = vm.AssignedToEmployeeId,

                EmployeeTaskPriorityId = vm.EmployeeTaskPriorityId,

                EmployeeTaskStatusId = defaultStatus.Id,

                DueDate = vm.DueDate,

                EstimatedHours = vm.EstimatedHours,

                CreatedDate = DateTime.UtcNow,

                IsDeleted = false
            };

            _db.EmployeeTasks.Add(employeeTask);

            await _db.SaveChangesAsync();

            TempData["Success"] = "Task created successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            var companyId = CompanyId.Value;

            ViewBag.Employees = await _db.Employees.Where(x => x.CompanyId == companyId).OrderBy(x => x.FirstName)
                                            .ToListAsync();
            ViewBag.Priorities = await _db.TaskPriorities.Where(x => x.CompanyId == companyId && x.IsActive).OrderBy(x => x.DisplayOrder)
                                             .ToListAsync();
        }
    }
}
