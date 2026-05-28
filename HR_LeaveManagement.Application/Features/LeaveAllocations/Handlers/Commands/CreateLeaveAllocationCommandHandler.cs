using AutoMapper;
using HR_LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using HR_LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Application.Responses;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HR_LeaveManagement.Application.DTOs.LeaveAllocation;
using Microsoft.Extensions.Logging;

namespace HR_LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse<LeaveAllocationDto>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateLeaveAllocationCommandHandler> _logger;

        public CreateLeaveAllocationCommandHandler(
            ILeaveAllocationRepository leaveAllocationRepo, 
            IMapper mapper,
            ILogger<CreateLeaveAllocationCommandHandler> logger
            )
        {
            _leaveAllocationRepo = leaveAllocationRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<LeaveAllocationDto>> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse<LeaveAllocationDto>();
            var validator = new CreateLeaveAllocationDtoValidator(_leaveAllocationRepo);
            try
            {
                var validationResult = await validator.ValidateAsync(request.LeaveAllocationDto);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Creation failed.";
                    response.Errors = validationResult.Errors.Select(er => er.ErrorMessage).ToList();
                    return response;
                }

                var leaveAllocation = _mapper.Map<LeaveAllocation>(request.LeaveAllocationDto);
                leaveAllocation = await _leaveAllocationRepo.Add(leaveAllocation);

                response.Success = true;
                response.Message = "Creation successful.";
                response.Id = leaveAllocation.Id;
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occur while adding a leave allocation {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
