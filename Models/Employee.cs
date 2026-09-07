using System.ComponentModel.DataAnnotations;

namespace EmployeeLeavePortal.Models
{
    public class Employee
    {
        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Designation { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }
    }
}