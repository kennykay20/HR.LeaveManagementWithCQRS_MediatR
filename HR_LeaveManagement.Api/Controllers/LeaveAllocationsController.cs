using HR_LeaveManagement.Application.DTOs.LeaveAllocation;
using HR_LeaveManagement.Application.DTOs.LeaveType;
using HR_LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR_LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_LeaveManagement.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LeaveAllocationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveAllocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/LeaveAllocations
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var leaveAllocations = await _mediator.Send(new GetLeaveAllocationListRequest());
            return Ok(leaveAllocations);
        }

        // GET: api/LeaveAllocations/2
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var leaveAllocation = await _mediator.Send(new GetLeaveAllocationDetailRequest { Id = id });
            return Ok(leaveAllocation);
        }

        // POST: api/LeaveAllocations
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateLeaveAllocationDto request)
        {
            var command = new CreateLeaveAllocationCommand { LeaveAllocationDto = request };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT api/LeaveAllocations
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateLeaveAllocationDto request)
        {
            var command = new UpdateLeaveAllocationCommand { LeaveAllocationDto = request };
            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/LeaveAllocations/4
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteLeaveAllocationCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
