using HR_LeaveManagement.Application.DTOs.LeaveAllocation;
using HR_LeaveManagement.Application.Responses;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands
{
    public class CreateLeaveAllocationCommand : IRequest<BaseCommandResponse<LeaveAllocationDto>>
    {
        public CreateLeaveAllocationDto LeaveAllocationDto { get; set; }
    }
}
