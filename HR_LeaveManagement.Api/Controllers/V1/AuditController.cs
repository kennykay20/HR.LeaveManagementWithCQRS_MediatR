using Asp.Versioning;
using HR_LeaveManagement.Application.Features.AuditTrail.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_LeaveManagement.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageNum, int pageSize)
        {
            var command = new GetAuditPageListRequest { PageNumber = pageNum, PageSize = pageSize };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
