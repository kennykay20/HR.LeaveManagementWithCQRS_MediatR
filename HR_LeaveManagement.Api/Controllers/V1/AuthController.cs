using Asp.Versioning;
using HR_LeaveManagement.Application.Contracts.Attributes.Permissions;
using HR_LeaveManagement.Application.DTOs.Auth;
using HR_LeaveManagement.Application.Features.Auths.Requests.Commands;
using HR_LeaveManagement.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HR_LeaveManagement.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: // api/v1/Auth/register
        //[Authorize]
        //[HasPermission(Permissions.Role.Create)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto dto)
        {
            var command = new RegisterUserCommand {  registerDto = dto };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var command = new LoginCommand { loginDto = dto };
            var result = await _mediator.Send(command);
            return Created("", result);
        }

        [HttpPut("otp/verify")]
        public async Task<IActionResult> VerifyOtp()
        {
            return Ok();
        }

        [HttpPost("otp")]
        public async Task<IActionResult> GenerateOtp([FromBody]string email)
        {
            return Ok();
        }

        [HttpPost("access-token")]
        public async Task<IActionResult> GenerateAccessToken([FromBody]string email)
        {
            return Ok();
        }
    }
}
