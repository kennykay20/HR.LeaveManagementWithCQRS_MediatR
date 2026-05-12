using FluentValidation;
using HR_LeaveManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.DTOs.LeaveAllocation.Validators
{
    public class CreateLeaveAllocationDtoValidator : AbstractValidator<CreateLeaveAllocationDto>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;

        public CreateLeaveAllocationDtoValidator(ILeaveAllocationRepository leaveAllocationRepo)
        {
            _leaveAllocationRepo = leaveAllocationRepo;
            Include(new ILeaveAllocationDtoValidation(_leaveAllocationRepo));
        }
    }
}
