using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Application.Exceptions;
using HR_LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        ILogger<DeleteLeaveTypeCommandHandler> _logger;
        private readonly ICacheService _cacheService;
        private const string LeaveTypesCacheKey = "leave-types";
        public DeleteLeaveTypeCommandHandler(
            ILeaveTypeRepository leaveTypeRepo, 
            ILogger<DeleteLeaveTypeCommandHandler> logger, 
            ICacheService cacheService
            )
        {
            _leaveTypeRepo = leaveTypeRepo;
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveType = await _leaveTypeRepo.Get(request.Id);
            if (leaveType == null)
                throw new NotFoundException(nameof(LeaveType), request.Id);

            await _leaveTypeRepo.Delete(leaveType);
            await _cacheService.RemoveAsync(LeaveTypesCacheKey);
            return Unit.Value;
        }
    }
}
