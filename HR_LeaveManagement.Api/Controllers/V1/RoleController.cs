using Asp.Versioning;
using HR_LeaveManagement.Application.DTOs.Role;
using HR_LeaveManagement.Application.Features.Roles.Requests.Commands;
using HR_LeaveManagement.Application.Features.Roles.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_LeaveManagement.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            var command = new CreateRoleCommand { createRoleDto = dto };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _mediator.Send(new GetRolesRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var command = new GetRoleDetailsByIdRequest { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserDto dto)
        {
            var command = new AssignRoleToUserCommand { assignRoleUserDto = dto };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("user-roles/{userId}")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var command = new GetUserRolesByUserIdRequest { UserId = userId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
