using Asp.Versioning;
using HR_LeaveManagement.Application.DTOs.Permission;
using HR_LeaveManagement.Application.Features.Permissions.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_LeaveManagement.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePermissionDto dto)
        {
            var command = new CreatePermissionCommand { createPermissionDto = dto };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermission(int id)
        {
            return Ok();
        }
    }
}
