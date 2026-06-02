using Asp.Versioning;
using HR_LeaveManagement.Application.DTOs.LeaveAllocation;
using HR_LeaveManagement.Application.DTOs.LeaveRequest;
using HR_LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR_LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using HR_LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR_LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_LeaveManagement.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/LeaveRequests
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var leaveRequests = await _mediator.Send(new GetLeaveRequestListRequest());
            return Ok(leaveRequests);
        }

        // GET: api/LeaveRequests/2
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var leaveRequest = await _mediator.Send(new GetLeaveRequestDetailsRequest { Id = id });
            return Ok(leaveRequest);
        }

        // POST: api/LeaveRequests
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLeaveRequestDto request)
        {
            Console.WriteLine("request dto, here inside the CreateLeave controller with LeaveTypeId and email ");
            Console.WriteLine(request.LeaveTypeId);
            Console.WriteLine(request.Email);
            var command = new CreateLeaveRequestCommand { LeaveRequestDto = request };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT api/LeaveRequests
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateLeaveRequestDto request)
        {
            var command = new UpdateLeaveRequestCommand { LeaveRequestDto = request };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("ChangeApproval")]
        public async Task<IActionResult> ChangeLeaveApproval([FromBody] ChangeLeaveRequestApprovalDto request)
        {
            var command = new UpdateLeaveRequestCommand { ChangeLeaveRequestApprovalDto = request };
            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/LeaveRequests/4
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteLeaveRequestCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

    }
}
