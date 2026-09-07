using EmployeeLeavePortal.Models;
using EmployeeLeavePortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeavePortal.Controllers
{
    public class LeaveController : Controller
    {
        private readonly LeaveService _leaveService;
        private readonly EmployeeService _employeeService;

        public LeaveController(
            LeaveService leaveService,
            EmployeeService employeeService)
        {
            _leaveService = leaveService;
            _employeeService = employeeService;
        }

        public IActionResult Index(
            string? search,
            string? status,
            string? leaveType)
        {
            var requests = _leaveService.GetAllLeaveRequests();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var employees = _employeeService.GetAllEmployees();

                requests = requests
                    .Where(r =>
                    {
                        var employee = employees.FirstOrDefault(e =>
                            e.EmployeeId.Equals(
                                r.EmployeeId,
                                StringComparison.OrdinalIgnoreCase));

                        return r.RequestId.ToString().Contains(search) ||
                               (employee != null &&
                                employee.FullName.Contains(
                                    search,
                                    StringComparison.OrdinalIgnoreCase));
                    })
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(status) &&
                status != "All")
            {
                requests = requests
                    .Where(r => r.Status.Equals(
                        status,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(leaveType) &&
                leaveType != "All")
            {
                requests = requests
                    .Where(r => r.LeaveType.Equals(
                        leaveType,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.LeaveType = leaveType;
            ViewBag.Employees = _employeeService.GetAllEmployees();

            return View(requests);
        }

        
        public IActionResult Details(int id)
        {
            var request = _leaveService.GetLeaveRequestById(id);

            if (request == null)
            {
                return NotFound();
            }

            ViewBag.Employees = _employeeService.GetAllEmployees();

            return View(request);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Employees = _employeeService.GetAllEmployees();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LeaveRequest request)
        {
            if (request.ToDate < request.FromDate)
            {
                ModelState.AddModelError(
                    "ToDate",
                    "To Date cannot be earlier than From Date.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Employees = _employeeService.GetAllEmployees();

                return View(request);
            }

            _leaveService.AddLeaveRequest(request);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var updated = _leaveService.UpdateStatus(
                id,
                "Approved");

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            var updated = _leaveService.UpdateStatus(
                id,
                "Rejected");

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}