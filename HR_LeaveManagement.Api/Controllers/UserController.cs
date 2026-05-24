using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.Features.Users.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_LeaveManagement.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/request
        [HttpGet]
        public async Task<IActionResult> Get([FromBody] UserPageDto request)
        {
            var command = new GetUserPageListRequest { userPageDto = request };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
