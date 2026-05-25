using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementAPI.Models
{
    public class LeaveRequest
    {
        public int LeaveId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [RegularExpression("Sick|Casual|Annual|Maternity|Paternity", ErrorMessage = "LeaveType must be: Sick, Casual, Annual, Maternity, or Paternity.")]
        public string LeaveType { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public string Reason { get; set; }

        public string Status { get; set; }
    }
}