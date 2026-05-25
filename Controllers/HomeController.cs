using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new
            {
                Message =
                "Employee Leave Management API Running Successfully",

                Status = "Active",

                Version = "1.0"
            });
        }
    }
}