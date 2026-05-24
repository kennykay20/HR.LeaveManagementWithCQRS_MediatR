using HR_LeaveManagement.Application.DTOs.LeaveType;
using HR_LeaveManagement.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.Features.LeaveTypes.Requests.Commands
{
    public class CreateLeaveTypeCommand : IRequest<BaseCommandResponse<LeaveTypeDto>>
    {
        public CreateLeaveTypeDto LeaveTypeDto { get; set; }
    }
}
