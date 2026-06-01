using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;
using TaskFlow.Services;
using TaskFlow.ViewModels;

namespace TaskFlow.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        private readonly CompanyService _companyService;

        public EmployeeController(EmployeeService employeeService, CompanyService companyService)
        {
            _employeeService = employeeService;
            _companyService = companyService;
        }
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployees();
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

            var emp = new Employee
            {
                CompanyId = employee.CompanyId,
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

            var employeeVM = new Employee
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                CompanyId = employee.CompanyId,
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
