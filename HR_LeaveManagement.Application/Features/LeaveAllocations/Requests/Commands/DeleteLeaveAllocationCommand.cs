using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands
{
    public class DeleteLeaveAllocationCommand : IRequest
    {
        public int Id { get; set; }
    }
}
