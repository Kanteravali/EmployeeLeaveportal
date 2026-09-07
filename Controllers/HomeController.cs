using EmployeeLeavePortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeavePortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeService _employeeService;
        private readonly LeaveService _leaveService;

        public HomeController(
            EmployeeService employeeService,
            LeaveService leaveService)
        {
            _employeeService = employeeService;
            _leaveService = leaveService;
        }

        public IActionResult Index()
        {
            var employees = _employeeService.GetAllEmployees();
            var leaveRequests = _leaveService.GetAllLeaveRequests();

            ViewBag.TotalEmployees = employees.Count;

            ViewBag.TotalLeaveRequests = leaveRequests.Count;

            ViewBag.PendingRequests = leaveRequests.Count(
                r => r.Status == "Pending");

            ViewBag.ApprovedRequests = leaveRequests.Count(
                r => r.Status == "Approved");

            ViewBag.RejectedRequests = leaveRequests.Count(
                r => r.Status == "Rejected");

            var recentRequests = leaveRequests
                .OrderByDescending(r => r.RequestDate)
                .Take(5)
                .ToList();

            ViewBag.RecentRequests = recentRequests;

            ViewBag.Employees = employees;

            return View();
        }
    }
}