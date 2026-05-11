using FluentValidation;
using HR_LeaveManagement.Application.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.DTOs.LeaveAllocation.Validators
{
    public class UpdateLeaveAllocationDtoValidation : AbstractValidator<UpdateLeaveAllocationDto>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;

        public UpdateLeaveAllocationDtoValidation(ILeaveAllocationRepository leaveAllocationRepo)
        {
            _leaveAllocationRepo = leaveAllocationRepo;
            Include(new ILeaveAllocationDtoValidation(_leaveAllocationRepo));

            RuleFor(data => data.Id)
                .NotNull().WithMessage("{PropertyName} must be present.");
        }
    }
}
