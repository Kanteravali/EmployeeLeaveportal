using System.ComponentModel.DataAnnotations;

namespace EmployeeLeavePortal.Models
{
    public class LeaveRequest
    {
        public int RequestId { get; set; }

        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        public string LeaveType { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public int TotalLeaveDays { get; set; }
    }
}