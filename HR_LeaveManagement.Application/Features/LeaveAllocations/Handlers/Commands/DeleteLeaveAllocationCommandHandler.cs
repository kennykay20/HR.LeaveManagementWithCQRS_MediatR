using HR_LeaveManagement.Application.Exceptions;
using HR_LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR_LeaveManagement.Application.Persistence.Contracts;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;

        public DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepo)
        {
            _leaveAllocationRepo = leaveAllocationRepo;
        }
        public async Task<Unit> Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var leaveAllocate = await _leaveAllocationRepo.Get(request.Id);
            if (leaveAllocate == null)
                throw new NotFoundException(nameof(LeaveAllocation), request.Id);

            await _leaveAllocationRepo.Delete(leaveAllocate);
            return Unit.Value;
        }
    }
}
