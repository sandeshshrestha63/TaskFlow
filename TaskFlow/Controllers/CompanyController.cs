using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Constants;
using TaskFlow.Data;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.Services;

namespace TaskFlow.Controllers
{
    [Authorize(Policy = Policies.SuperAdminOnly)]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(AppDbContext context, ICurrentUserServices currentUser, ICompanyService companyService) : base(currentUser, context)
        {
            _companyService = companyService;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return View(companies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Company company)
        {
            Company data = await _companyService.AddCompanyAsyc(company);
            await CreateDefaultTaskStatuses(data.Id);
            TempData["success"] = "Company saved successfully!";
            return RedirectToAction("Index");
        }
        private async Task CreateDefaultTaskStatuses(int companyId)
        {
            var statuses = new List<EmployeeTaskStatus>
            {
                new() { CompanyId = companyId, Name = "Pending", DisplayOrder = 1, IsActive = true, IsSystem = true, IsDefault = true, ColorCode ="#6C757D" },
                new() { CompanyId = companyId, Name = "In Progress", DisplayOrder = 2, IsActive = true, IsSystem = true, ColorCode ="#F59E0B" },
                new() { CompanyId = companyId, Name = "On Hold", DisplayOrder = 3, IsActive = true, IsSystem = true, ColorCode ="#885E3C" },
                new() { CompanyId = companyId, Name = "Completed", DisplayOrder = 4, IsActive = true, IsSystem = true, ColorCode ="#198754" },
                new() { CompanyId = companyId, Name = "Cancelled", DisplayOrder = 5, IsActive = true, IsSystem = true , ColorCode = "#495057"}
            };

            _db.EmployeeTaskStatus.AddRange(statuses);

            var priorities = new List<EmployeeTaskPriority>
            {
                new() { CompanyId = companyId, Name = "Low", DisplayOrder = 1, IsDefault = false, IsSystem = true, IsActive = true, ColorCode ="#198754" },
                new() { CompanyId = companyId,Name = "Medium", DisplayOrder = 2, IsDefault = true, IsSystem = true, IsActive = true, ColorCode ="#FFC107" },
                new() { CompanyId = companyId, Name = "High", DisplayOrder = 3, IsDefault = false, IsSystem = true, IsActive = true,ColorCode ="#FD7E14" },
                new() { CompanyId = companyId, Name = "Critical", DisplayOrder = 4, IsDefault = false, IsSystem = true, IsActive = true,ColorCode ="#DC3545" }
            };

            _db.TaskPriorities.AddRange(priorities);

            await _db.SaveChangesAsync();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Company company)
        {
            var existingCompany = await _companyService.GetCompanyByIdAsync(company.Id);
            if (existingCompany == null)
            {
                return NotFound();
            }
            await _companyService.UpdateCompanyAsync(company);
            TempData["success"] = "Company updated successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _companyService.DeleteCompanyAsync(id);
            TempData["success"] = "Company deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
