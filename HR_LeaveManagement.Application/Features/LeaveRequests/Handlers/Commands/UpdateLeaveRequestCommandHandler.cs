using AutoMapper;
using HR_LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR_LeaveManagement.Application.Exceptions;
using HR_LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR_LeaveManagement.Application.Persistence.Contracts;
using HR_LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly IMapper _mapper;

        public UpdateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepo, IMapper mapper)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveRequestDtoValidator(_leaveRequestRepo);
            var validationResult = validator.Validate(request.LeaveRequestDto);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var leaveRequest = await _leaveRequestRepo.Get(request.Id);
            if (leaveRequest == null)
            {
                throw new Exception("LeaveRequest doesn't exist");
            }
            if (request.LeaveRequestDto != null)
            {
                _mapper.Map(request.LeaveRequestDto, leaveRequest);
                await _leaveRequestRepo.Update(leaveRequest);
            }
            else if(request.ChangeLeaveRequestApprovalDto != null)
            {
                await _leaveRequestRepo.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);
            }
            return Unit.Value;
        }
    }
}
