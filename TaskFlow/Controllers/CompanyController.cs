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
            _companyService.UpdateCompany(company);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _companyService.DeleteCompany(id);
            return RedirectToAction("Index");
        }
    }
}
