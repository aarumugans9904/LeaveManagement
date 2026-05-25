using EmployeeLeaveManagementAPI.DataAccess;
using EmployeeLeaveManagementAPI.Models;
using LeaveManagementAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementAPI.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository _repository;
        private readonly LeaveRepository _leaveRepository;

        public EmployeesController(
            EmployeeRepository repository,
            LeaveRepository leaveRepository)
        {
            _repository = repository;
            _leaveRepository = leaveRepository;
        }

        // Add Employee
        [HttpPost]
        public IActionResult AddEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _repository.AddEmployee(employee);
                return Ok("Employee Added Successfully");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // Get All Employees
        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees =
                _repository.GetEmployees();

            return Ok(employees);
        }

        // Get Employee By Id
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee =
                _repository.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound(
                    "Employee Not Found");
            }

            return Ok(employee);
        }

        // Update Employee
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee employee)
        {
            var existingEmployee =
                _repository.GetEmployeeById(id);

            if (existingEmployee == null)
            {
                return NotFound(
                    "Employee Not Found");
            }

            _repository.UpdateEmployee(
                id,
                employee);

            return Ok(
                "Employee Updated Successfully");
        }

        // Delete Employee
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee =
                _repository.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound(
                    "Employee Not Found");
            }

            if (_leaveRepository.GetLeaveHistoryByEmployee(id).Count > 0)
                return BadRequest("Cannot delete employee with existing leave records.");

            _repository.DeleteEmployee(id);

            return Ok(
                "Employee Deleted Successfully");
        }

        // Get Leave History By Employee
        [HttpGet("{id}/leaves")]
        public IActionResult GetLeaveHistory(int id)
        {
            if (!_leaveRepository.EmployeeExists(id))
            {
                return NotFound(
                    "Employee Not Found");
            }

            var leaveHistory =
                _leaveRepository
                .GetLeaveHistoryByEmployee(id);

            return Ok(leaveHistory);
        }
    }
}