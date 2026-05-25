using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementAPI.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Department { get; set; }
    }
}