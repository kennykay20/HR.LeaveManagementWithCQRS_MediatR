using HR_LeaveManagement.Application.DTOs.Auth;
using HR_LeaveManagement.Application.Features.Auths.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterDto request)
        {
            var command = new RegisterUserCommand {  registerDto = request };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
