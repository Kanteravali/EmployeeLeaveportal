using System.Text.Json;
using EmployeeLeavePortal.Models;

namespace EmployeeLeavePortal.Services
{
    public class HolidayApiService
    {
        private readonly HttpClient _httpClient;

        public HolidayApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Holiday>> GetHolidaysAsync(
            string countryCode,
            int year)
        {
            string url =
                $"national-holidays/api/{countryCode}/{year}.json";

            using HttpResponseMessage response =
                await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Holiday API returned status code {response.StatusCode}.");
            }

            string json =
                await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new HttpRequestException(
                    "Holiday API returned an empty response.");
            }

            var apiResponse =
                JsonSerializer.Deserialize<TallyfyResponse>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (apiResponse?.Holidays == null)
            {
                throw new HttpRequestException(
                    "Holiday API returned invalid JSON data.");
            }

            return apiResponse.Holidays.Select(h => new Holiday
            {
                Name = h.Name,
                Date = h.Date,
                Country = countryCode
            }).ToList();
        }

        private class TallyfyResponse
        {
            public List<TallyfyHoliday> Holidays { get; set; }
                = new List<TallyfyHoliday>();
        }

        private class TallyfyHoliday
        {
            public string Name { get; set; } = string.Empty;

            public string Date { get; set; } = string.Empty;
        }
    }
}