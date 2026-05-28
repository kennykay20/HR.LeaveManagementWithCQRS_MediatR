using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Application.DTOs.LeaveRequest;
using HR_LeaveManagement.Application.DTOs.LeaveType;
using HR_LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR_LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR_LeaveManagement.Application.Responses;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, BaseCommandResponse<LeaveTypeDto>>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly IMapper _mapper;

        public CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepo, IMapper mapper)
        {
            _leaveTypeRepo = leaveTypeRepo;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse<LeaveTypeDto>> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse<LeaveTypeDto>();
            var validator = new CreateLeaveTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(request.LeaveTypeDto);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation failed.";
                response.Errors = validationResult.Errors.Select(er => er.ErrorMessage).ToList();
                response.Data = null;
                return response;
            }
            //check name already exist
            var existLeaveType = await _leaveTypeRepo.GetLeaveTypeByName(request.LeaveTypeDto.Name);
            if(existLeaveType != null)
            {
                response.Success = false;
                response.Message = "Name already exist";
                response.Data = null;
                return response;
            }
            var leaveType = _mapper.Map<LeaveType>(request.LeaveTypeDto);
            var leaveResponse = await _leaveTypeRepo.Add(leaveType);

            var result = _mapper.Map<LeaveTypeDto>(leaveResponse);

            response.Success = true;
            response.Message = "Creation successful.";
            response.Id = result.Id;
            response.Data = result ?? null!;

            return response;
        }
    }
}
