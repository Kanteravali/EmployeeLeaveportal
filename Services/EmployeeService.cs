using System.Text.Json;
using EmployeeLeavePortal.Models;

namespace EmployeeLeavePortal.Services
{
    public class EmployeeService
    {
        private readonly string _filePath;

        public EmployeeService(IWebHostEnvironment environment)
        {
            _filePath = Path.Combine(
                environment.ContentRootPath,
                "Data",
                "employees.json");
        }

        public List<Employee> GetAllEmployees()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Employee>();
            }

            string json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<Employee>>(json)
                   ?? new List<Employee>();
        }

        public Employee? GetEmployeeById(string employeeId)
        {
            var employees = GetAllEmployees();

            return employees.FirstOrDefault(e =>
                e.EmployeeId.Equals(employeeId, StringComparison.OrdinalIgnoreCase));
        }

        public void AddEmployee(Employee employee)
        {
            var employees = GetAllEmployees();

            employees.Add(employee);

            SaveEmployees(employees);
        }

        public bool UpdateEmployee(Employee employee)
        {
            var employees = GetAllEmployees();

            var existingEmployee = employees.FirstOrDefault(e =>
                e.EmployeeId.Equals(employee.EmployeeId, StringComparison.OrdinalIgnoreCase));

            if (existingEmployee == null)
            {
                return false;
            }

            existingEmployee.FullName = employee.FullName;
            existingEmployee.Department = employee.Department;
            existingEmployee.Email = employee.Email;
            existingEmployee.Designation = employee.Designation;
            existingEmployee.JoinDate = employee.JoinDate;

            SaveEmployees(employees);

            return true;
        }

        public bool DeleteEmployee(string employeeId)
        {
            var employees = GetAllEmployees();

            var employee = employees.FirstOrDefault(e =>
                e.EmployeeId.Equals(employeeId, StringComparison.OrdinalIgnoreCase));

            if (employee == null)
            {
                return false;
            }

            employees.Remove(employee);

            SaveEmployees(employees);

            return true;
        }

        private void SaveEmployees(List<Employee> employees)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(employees, options);

            File.WriteAllText(_filePath, json);
        }
    }
}