using AutoMapper;
using HR_LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR_LeaveManagement.Application.Exceptions;
using HR_LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace HR_LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateLeaveRequestCommandHandler> _logger;

        public UpdateLeaveRequestCommandHandler(
            ILeaveRequestRepository leaveRequestRepo,
            IMapper mapper,
            ILogger<UpdateLeaveRequestCommandHandler> logger,
            ILeaveTypeRepository leaveTypeRepo
            )
        {
            _leaveRequestRepo = leaveRequestRepo;
            _mapper = mapper;
            _logger = logger;
            _leaveTypeRepo = leaveTypeRepo;
        }
        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var id = 0;
            ValidationResult validationResult;

            try
            {
                if (request.LeaveRequestDto is not null)
                {
                    id = request.LeaveRequestDto.Id;
                    var validator = new UpdateLeaveRequestDtoValidator(_leaveRequestRepo, _leaveTypeRepo);
                    validationResult = await validator.ValidateAsync(request.LeaveRequestDto, cancellationToken);
                }
                else if (request.ChangeLeaveRequestApprovalDto is not null)
                {
                    id = request.ChangeLeaveRequestApprovalDto.Id;
                    var validator = new ChangeApproveLeaveRequestDtoValidator();
                    validationResult = await validator.ValidateAsync(request.ChangeLeaveRequestApprovalDto, cancellationToken);
                }
                else
                {
                    throw new BadRequestException("Invalid request.");
                }


                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult);


                var leaveRequest = await _leaveRequestRepo.Get(id);
                if (leaveRequest == null)
                {
                    throw new NotFoundException(nameof(LeaveRequest), id);
                }
                if (request.LeaveRequestDto != null)
                {
                    _mapper.Map(request.LeaveRequestDto, leaveRequest);
                    await _leaveRequestRepo.Update(leaveRequest);
                }
                else if (request.ChangeLeaveRequestApprovalDto != null)
                {
                    await _leaveRequestRepo.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);
                }
                return Unit.Value;
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occur while updating the leave request {ex.Message}");
                throw;
            }
        }
    }
}
