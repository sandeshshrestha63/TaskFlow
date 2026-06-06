using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Constants;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.Services;

namespace TaskFlow.Controllers
{
    [Authorize(Policy = Policies.SuperAdminOnly)]
    public class CompanyController : BaseController
    {
        private readonly CompanyService _companyService;

        public CompanyController(ICurrentUserServices currentUser,CompanyService companyService) :base(currentUser)
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
        public IActionResult Create(Company company)
        {
            _companyService.AddCompany(company);
            TempData["success"] = "Company saved successfully!";
            return RedirectToAction("Index");
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
            if(existingCompany == null)
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
