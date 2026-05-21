using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_LeaveManagement.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HomeTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult get() => Ok("Hello Welcome to my API.");
    }
}
