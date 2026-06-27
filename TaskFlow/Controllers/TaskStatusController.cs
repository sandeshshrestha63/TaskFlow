using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TaskFlow.Data;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.ViewModels;

namespace TaskFlow.Controllers
{
    public class TaskStatusController : BaseController
    {
        public TaskStatusController(ICurrentUserServices currentUser, AppDbContext context) : base(currentUser,context)
        {
        }
        public async Task<IActionResult> Index()
        {
            var statuses = await _db.EmployeeTaskStatus.Where(x => x.CompanyId == CompanyId)
                                    .OrderBy(x => x.DisplayOrder).ToListAsync();
            return View(statuses);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskStatusDataVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var companyId = CompanyId;

            if (vm.IsDefault)
            {
                var existingDefaults = await _db.EmployeeTaskStatus
                    .Where(x => x.CompanyId == companyId)
                    .ToListAsync();

                existingDefaults.ForEach(x => x.IsDefault = false);
            }

            var status = new EmployeeTaskStatus
            {
                CompanyId = companyId,
                Name = vm.Name,
                DisplayOrder = vm.DisplayOrder,
                IsDefault = vm.IsDefault,
                IsSystem = false,
                IsActive = vm.IsActive,
                ColorCode = vm.ColorCode
            };

            _db.EmployeeTaskStatus.Add(status);

            await _db.SaveChangesAsync();

            TempData["Success"] = "Status created successfully.";

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var companyId = CompanyId;

            var status = await _db.EmployeeTaskStatus
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId);

            if (status == null)
                return NotFound();

            if (status.IsSystem)
            {
                TempData["Error"] = "System statuses cannot be modified.";
                return RedirectToAction(nameof(Index));
            }

            var vm = new TaskStatusDataVM
            {
                Id = status.Id,
                Name = status.Name,
                DisplayOrder = status.DisplayOrder,
                IsActive = status.IsActive,
                ColorCode = status.ColorCode
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskStatusDataVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var companyId = CompanyId;

            var status = await _db.EmployeeTaskStatus
                .FirstOrDefaultAsync(x =>
                    x.Id == vm.Id &&
                    x.CompanyId == companyId);

            if (status == null)
                return NotFound();

            if (status.IsSystem)
            {
                TempData["Error"] = "System statuses cannot be modified.";
                return RedirectToAction(nameof(Index));
            }
            if (status.IsDefault && !vm.IsDefault)
            {
                bool hasAnotherDefault = await _db.EmployeeTaskStatus
                    .AnyAsync(x =>
                        x.CompanyId == companyId &&
                        x.Id != vm.Id &&
                        x.IsDefault);

                if (!hasAnotherDefault)
                {
                    ModelState.AddModelError(
                        "IsDefault",
                        "At least one default status is required.");

                    return View(vm);
                }
            }

            // If this status is being made default,
            // remove default flag from all others
            if (vm.IsDefault)
            {
                var existingDefaults = await _db.EmployeeTaskStatus
                    .Where(x =>
                        x.CompanyId == companyId &&
                        x.Id != vm.Id)
                    .ToListAsync();

                existingDefaults.ForEach(x => x.IsDefault = false);
            }

            status.Name = vm.Name;
            status.DisplayOrder = vm.DisplayOrder;
            status.IsActive = vm.IsActive;
            status.IsDefault = vm.IsDefault;
            status.ColorCode = vm.ColorCode;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Status updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {

            var status = await _db.EmployeeTaskStatus
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == CompanyId);

            if (status == null)
                return NotFound();

            if (status.IsSystem)
            {
                TempData["Error"] = "System statuses cannot be modified.";
                return RedirectToAction(nameof(Index));
            }

            status.IsActive = false;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Status deactivated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyId = CompanyId;
            var status = await _db.EmployeeTaskStatus.FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId);
            if (status == null)
                return NotFound();
            if (status.IsSystem)
            {
                TempData["Error"] = "System statuses cannot be deleted.";
                return RedirectToAction(nameof(Index));
            }
            bool isUsed = await _db.EmployeeTasks.AnyAsync(x => x.EmployeeTaskStatusId == id);
            if (isUsed)
            {
                TempData["Error"] ="This status is being used by tasks and cannot be deleted.";
                return RedirectToAction(nameof(Index));
            }
            status.IsActive = false;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Status deactivated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
