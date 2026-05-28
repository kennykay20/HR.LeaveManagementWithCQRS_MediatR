using AutoMapper;
using HR_LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR_LeaveManagement.Application.Exceptions;
using HR_LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HR_LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateLeaveTypeCommandHandler> _logger;

        public UpdateLeaveTypeCommandHandler(
            ILeaveTypeRepository leaveTypeRepo, 
            IMapper mapper,
            ILogger<UpdateLeaveTypeCommandHandler> logger
            )
        {
            _leaveTypeRepo = leaveTypeRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdateLeaveTypeDtoValidator();
                var validationResult = await validator.ValidateAsync(request.LeaveTypeDto);

                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult);

                var leaveType = await _leaveTypeRepo.Get(request.LeaveTypeDto.Id);
                if (leaveType == null)
                {
                    throw new NotFoundException(nameof(LeaveType), request.LeaveTypeDto.Id);
                }
                _mapper.Map(request.LeaveTypeDto, leaveType);
                await _leaveTypeRepo.Update(leaveType);
                return Unit.Value;
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occur while updating a leave type {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
