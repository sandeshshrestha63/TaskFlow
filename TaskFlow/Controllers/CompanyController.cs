using Microsoft.AspNetCore.Mvc;
using TaskFlow.Models;
using TaskFlow.Services;

namespace TaskFlow.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
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
            TempData["SuccessMessage"] = "Company saved successfully!";
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
            TempData["SuccessMessage"] = "Company deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
