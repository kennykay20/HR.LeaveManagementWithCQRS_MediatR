using Asp.Versioning;
using HR_LeaveManagement.Application.Contracts.Attributes.Permissions;
using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.Features.Users.Requests.Queries;
using HR_LeaveManagement.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_LeaveManagement.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/request
        //[HasPermission(Permissions.Leave.Create)]
        [HttpGet]
        public async Task<IActionResult> Get(int pageNum, int pageSize)
        {
            var command = new GetUserPageListRequest { PageNumber = pageNum, PageSize = pageSize };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
