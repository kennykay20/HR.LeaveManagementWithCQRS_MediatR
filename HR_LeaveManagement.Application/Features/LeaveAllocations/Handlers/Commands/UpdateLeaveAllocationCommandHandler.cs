using AutoMapper;
using HR_LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using HR_LeaveManagement.Application.Exceptions;
using HR_LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HR_LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateLeaveAllocationCommandHandler> _logger;

        public UpdateLeaveAllocationCommandHandler(
            ILeaveAllocationRepository leaveAllocationRepo, 
            IMapper mapper,
            ILogger<UpdateLeaveAllocationCommandHandler> logger
            )
        {
            _leaveAllocationRepo = leaveAllocationRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdateLeaveAllocationDtoValidation(_leaveAllocationRepo);
                var validationResult = validator.Validate(request.LeaveAllocationDto);

                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult);

                var leaveAllocate = await _leaveAllocationRepo.Get(request.LeaveAllocationDto.Id);
                if (leaveAllocate == null)
                {
                    throw new NotFoundException(nameof(LeaveAllocation), request.LeaveAllocationDto.Id);
                }
                _mapper.Map(request.LeaveAllocationDto, leaveAllocate);
                await _leaveAllocationRepo.Update(leaveAllocate);
                return Unit.Value;
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occur while updating the leave allocation {ex.Message}");
                throw;
            }
        }
    }
}
