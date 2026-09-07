using EmployeeLeavePortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeavePortal.Controllers
{
    public class HolidayController : Controller
    {
        private readonly HolidayApiService _holidayApiService;

        public HolidayController(HolidayApiService holidayApiService)
        {
            _holidayApiService = holidayApiService;
        }

        public async Task<IActionResult> Index(
            string countryCode = "IN",
            int year = 2026)
        {
            try
            {
                var holidays =
                    await _holidayApiService.GetHolidaysAsync(
                        countryCode,
                        year);

                ViewBag.CountryCode = countryCode;
                ViewBag.Year = year;

                return View(holidays);
            }
            catch (HttpRequestException)
            {
                ViewBag.ErrorMessage =
                    "Unable to load holiday data. Please try again.";

                ViewBag.CountryCode = countryCode;
                ViewBag.Year = year;

                return View(new List<EmployeeLeavePortal.Models.Holiday>());
            }
            catch (TaskCanceledException)
            {
                ViewBag.ErrorMessage =
                    "The holiday API took too long to respond.";

                ViewBag.CountryCode = countryCode;
                ViewBag.Year = year;

                return View(new List<EmployeeLeavePortal.Models.Holiday>());
            }
        }
    }
}