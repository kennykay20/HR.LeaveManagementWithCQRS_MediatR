using HR_LeaveManagement.Application.DTOs.LeaveType;
using HR_LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR_LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace HR_LeaveManagement.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/LeaveTypes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var leaveTypes = await _mediator.Send(new GetLeaveTypeListRequest());
            return Ok(leaveTypes);
        }

        // GET: api/LeaveTypes/2
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var leaveType = await _mediator.Send(new GetLeaveTypeDetailRequest { Id = id });
            return Ok(leaveType);
        }

        // POST: api/LeaveTypes
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateLeaveTypeDto request)
        {
            var command = new CreateLeaveTypeCommand { LeaveTypeDto = request };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT api/LeaveTypes
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LeaveTypeDto request)
        {
            var command = new UpdateLeaveTypeCommand { LeaveTypeDto = request };
            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/LeaveTypes/4
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteLeaveTypeCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
