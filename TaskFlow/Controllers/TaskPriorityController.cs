using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.ViewModels;

namespace TaskFlow.Controllers
{
    public class TaskPriorityController : BaseController
    {
        public TaskPriorityController(ICurrentUserServices currentUser, AppDbContext context) : base(currentUser, context)
        {
        }
        public async Task<IActionResult> Index()
        {
            var statuses = await _db.TaskPriorities.Where(x => x.CompanyId == CompanyId)
                .OrderBy(x => x.DisplayOrder).ToListAsync();
            return View(statuses);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskPriorityDataVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            var companyId = CompanyId.Value;
            if (vm.IsDefault)
            {
                var existingDefaults = await _db.TaskPriorities.Where(x => x.CompanyId == companyId).ToListAsync();
                existingDefaults.ForEach(x => x.IsDefault = false);
            }
            var status = new EmployeeTaskPriority
            {
                CompanyId = companyId,
                Name = vm.Name,
                DisplayOrder = vm.DisplayOrder,
                IsDefault = vm.IsDefault,
                IsSystem = false,
                IsActive = vm.IsActive,
            };
            _db.TaskPriorities.Add(status);
            await _db.SaveChangesAsync();
            TempData["Success"] = "priority created successfully.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var companyId = CompanyId.Value;
            var priority = await _db.TaskPriorities.FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId);
            if (priority == null)
                return NotFound();
            if (priority.IsSystem)
            {
                TempData["Error"] = "System statuses cannot be modified.";
                return RedirectToAction(nameof(Index));
            }
            var vm = new TaskPriorityDataVM
            {
                Id = priority.Id,
                Name = priority.Name,
                DisplayOrder = priority.DisplayOrder,
                IsActive = priority.IsActive
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskPriorityDataVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            var companyId = CompanyId.Value;
            var priority = await _db.TaskPriorities.FirstOrDefaultAsync(x => x.Id == vm.Id && x.CompanyId == companyId);
            if (priority == null)
                return NotFound();
            if (priority.IsSystem)
            {
                TempData["Error"] = "System priorities cannot be modified.";
                return RedirectToAction(nameof(Index));
            }
            if (priority.IsDefault && !vm.IsDefault)
            {
                bool hasAnotherDefault = await _db.EmployeeTaskStatus.AnyAsync(x => x.CompanyId == companyId && x.Id != vm.Id && x.IsDefault);
                if (!hasAnotherDefault)
                {
                    ModelState.AddModelError("IsDefault", "At least one default priority is required.");
                    return View(vm);
                }
            }
            // If this status is being made default,
            // remove default flag from all others
            if(vm.IsDefault) 
            { 
                var existingDefaults = await _db.TaskPriorities.Where(x => x.CompanyId == companyId && x.Id != vm.Id).ToListAsync();
                existingDefaults.ForEach(x => x.IsDefault = false);
            }
            priority.Name = vm.Name;
            priority.DisplayOrder = vm.DisplayOrder; 
            priority.IsActive = vm.IsActive; 
            priority.IsDefault = vm.IsDefault; 
            await _db.SaveChangesAsync(); 
            TempData["Success"] = "Status updated successfully."; 
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id) 
        { 
            var priority = await _db.TaskPriorities.FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == CompanyId);
            if(priority == null)
                return NotFound(); 
            if(priority.IsSystem)
            { 
                TempData["Error"] = "System Priority cannot be modified.";
                return RedirectToAction(nameof(Index)); 
            } 
            priority.IsActive = false; 
            await _db.SaveChangesAsync(); 
            TempData["Success"] = "Priority deactivated successfully."; 
            return RedirectToAction(nameof(Index)); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyId = CompanyId.Value; 
            var priority = await _db.TaskPriorities.FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId); 
            if(priority == null) 
                return NotFound(); 
            if (priority.IsSystem) 
            {
                TempData["Error"] = "System Priority cannot be deleted.";
                return RedirectToAction(nameof(Index));
            }
            bool isUsed = await _db.EmployeeTasks.AnyAsync(x => x.EmployeeTaskPriorityId == id); 
            if (isUsed) 
            {
                TempData["Error"] = "This Priority is being used by tasks and cannot be deleted.";
                return RedirectToAction(nameof(Index)); 
            }
            priority.IsActive = false; 
            await _db.SaveChangesAsync();
            TempData["Success"] = "Priority deactivated successfully."; 
            return RedirectToAction(nameof(Index)); 
        }
    }
}
