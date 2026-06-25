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
        private readonly CompanyService _companyService;

        public CompanyController(AppDbContext context, ICurrentUserServices currentUser, CompanyService companyService) : base(currentUser, context)
        {
            _companyService = companyService;
        }

        public IActionResult Index()
        {
            var companies = _companyService.GetAllCompanies();
            return View(companies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Company company)
        {
            Company data = _companyService.AddCompany(company);
            await CreateDefaultTaskStatuses(data.Id);
            TempData["success"] = "Company saved successfully!";
            return RedirectToAction("Index");
        }
        private async Task CreateDefaultTaskStatuses(int companyId)
        {
            var statuses = new List<EmployeeTaskStatus>
            {
                new() { CompanyId = companyId, Name = "Pending", DisplayOrder = 1, IsActive = true, IsSystem = true, IsDefault = true },
                new() { CompanyId = companyId, Name = "In Progress", DisplayOrder = 2, IsActive = true, IsSystem = true },
                new() { CompanyId = companyId, Name = "On Hold", DisplayOrder = 3, IsActive = true, IsSystem = true },
                new() { CompanyId = companyId, Name = "Completed", DisplayOrder = 4, IsActive = true, IsSystem = true },
                new() { CompanyId = companyId, Name = "Cancelled", DisplayOrder = 5, IsActive = true, IsSystem = true }
            };

            _db.EmployeeTaskStatus.AddRange(statuses);

            var priorities = new List<EmployeeTaskPriority>
            {
                new() { CompanyId = companyId, Name = "Low", DisplayOrder = 1, IsDefault = false, IsSystem = true, IsActive = true },
                new() { CompanyId = companyId,Name = "Medium", DisplayOrder = 2, IsDefault = true, IsSystem = true, IsActive = true },
                new() { CompanyId = companyId, Name = "High", DisplayOrder = 3, IsDefault = false, IsSystem = true, IsActive = true },
                new() { CompanyId = companyId, Name = "Critical", DisplayOrder = 4, IsDefault = false, IsSystem = true, IsActive = true }
            };

            _db.TaskPriorities.AddRange(priorities);

            await _db.SaveChangesAsync();
        }

        public IActionResult Edit(int id)
        {
            var company = _companyService.GetCompanyById(id);
            return View(company);
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            var existingCompany = _companyService.GetCompanyById(company.Id);
            if (existingCompany == null)
            {
                return NotFound();
            }
            _companyService.UpdateCompany(company);
            TempData["success"] = "Company updated successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _companyService.DeleteCompany(id);
            TempData["success"] = "Company deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
