using System.Text.Json;
using EmployeeLeavePortal.Models;

namespace EmployeeLeavePortal.Services
{
    public class LeaveService
    {
        private readonly string _filePath;

        public LeaveService(IWebHostEnvironment environment)
        {
            _filePath = Path.Combine(
                environment.ContentRootPath,
                "Data",
                "leaveRequests.json");
        }

        public List<LeaveRequest> GetAllLeaveRequests()
        {
            if (!File.Exists(_filePath))
            {
                return new List<LeaveRequest>();
            }

            string json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<LeaveRequest>>(json)
                   ?? new List<LeaveRequest>();
        }

        public LeaveRequest? GetLeaveRequestById(int requestId)
        {
            var requests = GetAllLeaveRequests();

            return requests.FirstOrDefault(r =>
                r.RequestId == requestId);
        }

        public void AddLeaveRequest(LeaveRequest request)
        {
            var requests = GetAllLeaveRequests();

            if (requests.Any())
            {
                request.RequestId = requests.Max(r => r.RequestId) + 1;
            }
            else
            {
                request.RequestId = 1;
            }

            request.Status = "Pending";
            request.RequestDate = DateTime.Now;

            request.TotalLeaveDays =
                (request.ToDate - request.FromDate).Days + 1;

            requests.Add(request);

            SaveLeaveRequests(requests);
        }

        public bool UpdateStatus(int requestId, string status)
        {
            var requests = GetAllLeaveRequests();

            var request = requests.FirstOrDefault(r =>
                r.RequestId == requestId);

            if (request == null)
            {
                return false;
            }

            request.Status = status;

            SaveLeaveRequests(requests);

            return true;
        }

        private void SaveLeaveRequests(List<LeaveRequest> requests)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(requests, options);

            File.WriteAllText(_filePath, json);
        }
    }
}