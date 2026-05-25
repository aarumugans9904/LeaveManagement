using EmployeeLeaveManagementAPI.Models;
using LeaveManagementAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementAPI.Controllers
{
    [Route("api/leaves")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly LeaveRepository _repository;

        public LeavesController(
            LeaveRepository repository)
        {
            _repository = repository;
        }

        // Apply Leave
        [HttpPost]
        public IActionResult ApplyLeave([FromBody] LeaveRequest leave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository
                .EmployeeExists(
                leave.EmployeeId))
            {
                return BadRequest(
                    "Employee does not exist");
            }

            if (leave.FromDate.Date < DateTime.Today)
            {
                return BadRequest("FromDate cannot be in the past.");
            }

            if (leave.FromDate >
                leave.ToDate)
            {
                return BadRequest(
                    "FromDate cannot be greater than ToDate");
            }

            _repository.ApplyLeave(leave);

            return Ok(
                "Leave Applied Successfully");
        }

        // Get All Leaves
        [HttpGet]
        public IActionResult GetLeaves()
        {
            var leaves =
                _repository.GetLeaves();

            return Ok(leaves);
        }

        // Get Leave By Id
        [HttpGet("{id}")]
        public IActionResult GetLeaveById(
            int id)
        {
            var leave =
                _repository.GetLeaveById(id);

            if (leave == null)
            {
                return NotFound(
                    "Leave Not Found");
            }

            return Ok(leave);
        }

        // Approve Leave
        [HttpPut("{id}/approve")]
        public IActionResult ApproveLeave(
            int id)
        {
            var leave =
                _repository.GetLeaveById(id);

            if (leave == null)
            {
                return NotFound(
                    "Leave Not Found");
            }

            if (leave.Status != "Pending")
                return BadRequest($"Leave is already {leave.Status}.");

            _repository.ApproveLeave(id);
            return Ok("Leave Approved Successfully");
        }

        // Reject Leave
        [HttpPut("{id}/reject")]
        public IActionResult RejectLeave(
            int id)
        {
            var leave =
                _repository.GetLeaveById(id);

            if (leave == null)
            {
                return NotFound(
                    "Leave Not Found");
            }

            _repository.RejectLeave(id);

            return Ok(
                "Leave Rejected Successfully");
        }
    }
}