using EmployeeLeavePortal.Models;
using EmployeeLeavePortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeavePortal.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IActionResult Index(string? search, string? department)
        {
            var employees = _employeeService.GetAllEmployees();

            if (!string.IsNullOrWhiteSpace(search))
            {
                employees = employees
                    .Where(e => e.FullName.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(department))
            {
                employees = employees
                    .Where(e => e.Department.Equals(
                        department,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.Search = search;
            ViewBag.Department = department;

            ViewBag.Departments = _employeeService
                .GetAllEmployees()
                .Select(e => e.Department)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            return View(employees);
        }

        public IActionResult Details(string id)
        {
            var employee = _employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            if (_employeeService.GetEmployeeById(employee.EmployeeId) != null)
            {
                ModelState.AddModelError(
                    "EmployeeId",
                    "Employee ID already exists.");

                return View(employee);
            }

            _employeeService.AddEmployee(employee);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var employee = _employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            var updated = _employeeService.UpdateEmployee(employee);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            var deleted = _employeeService.DeleteEmployee(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}