using FluentValidation;
using HR_LeaveManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class CreateLeaveRequestDtoValidator : AbstractValidator<CreateLeaveRequestDto>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepo;

        public CreateLeaveRequestDtoValidator(ILeaveTypeRepository leaveTypeRepo)
        {
            _leaveTypeRepo = leaveTypeRepo;

            Include(new ILeaveRequestDtoValidator(_leaveTypeRepo));
            
        }
    }
}
