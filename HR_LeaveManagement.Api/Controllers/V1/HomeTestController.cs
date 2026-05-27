using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_LeaveManagement.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HomeTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult get() => Ok("Hello Welcome to my API.");
    }
}
