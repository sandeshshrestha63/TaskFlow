using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Constants;
using TaskFlow.Interfaces;
using TaskFlow.Models;
using TaskFlow.Services;
using TaskFlow.ViewModels;

namespace TaskFlow.Controllers
{
    [Authorize(Policy = Policies.CompanyAccess)]
    public class EmployeeController : BaseController
    {
        private readonly EmployeeService _employeeService;
        private readonly CompanyService _companyService;

        public EmployeeController(ICurrentUserServices currentUser,EmployeeService employeeService,CompanyService companyService): base(currentUser)
        {
            _employeeService = employeeService;
            _companyService = companyService;
        }
        public async Task<IActionResult> Index()
        {
            var CompanyId = CurrentUser.CompanyId;
            var employees = await _employeeService.GetAllEmployees(CompanyId);
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        public IActionResult Create()
        {
            var employee = new EmployeeViewModel
            {
                Companies = _companyService.GetAllCompanies()
            };
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
                return View(employee);
            var companyId = CurrentUser.CompanyId;
            var emp = new Employee
            {
                CompanyId = companyId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                CreatedAt = DateTime.Now
            };
            await _employeeService.AddEmployee(emp);

            TempData["success"] = "Employee created successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
                return NotFound();

            var viewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                CompanyId = employee.CompanyId,

                Companies = _companyService.GetAllCompanies()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
                return View(employee);
            var companyId = CurrentUser.CompanyId;
            var employeeVM = new Employee
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                CompanyId = companyId,
                Email = employee.Email
            };
            await _employeeService.UpdateEmployee(employeeVM);


            TempData["success"] = "Employee updated successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteEmployee(id);

            TempData["success"] = "Employee deleted successfully";
            return RedirectToAction(nameof(Index));
        }

    }
}
