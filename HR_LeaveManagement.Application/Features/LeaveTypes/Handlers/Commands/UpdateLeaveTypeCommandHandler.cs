using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Application.DTOs.LeaveType.Validators;
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
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateLeaveTypeCommandHandler> _logger;
        private readonly ICacheService _cacheService;
        private const string LeaveTypesCacheKey = "leave-types";
        public UpdateLeaveTypeCommandHandler(
            ILeaveTypeRepository leaveTypeRepo,
            IMapper mapper,
            ILogger<UpdateLeaveTypeCommandHandler> logger,
            ICacheService cacheService
            )
        {
            _leaveTypeRepo = leaveTypeRepo;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
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
                await _cacheService.RemoveAsync(LeaveTypesCacheKey);
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
