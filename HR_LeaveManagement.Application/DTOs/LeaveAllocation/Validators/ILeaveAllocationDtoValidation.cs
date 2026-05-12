using FluentValidation;
using HR_LeaveManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.DTOs.LeaveAllocation.Validators
{
    public class ILeaveAllocationDtoValidation : AbstractValidator<ILeaveAllocationDto>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;

        public ILeaveAllocationDtoValidation(ILeaveAllocationRepository leaveAllocationRepo)
        {
            _leaveAllocationRepo = leaveAllocationRepo;

            RuleFor(data => data.NumberOfDays)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonValue}.");

            RuleFor(data => data.Period)
                .GreaterThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("{PropertyName} must be after {ComparisonValue}.");

            RuleFor(data => data.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(async(id, token) =>
                {
                    var leaveAllocationExist = await _leaveAllocationRepo.Exist(id);
                    return !leaveAllocationExist;
                })
                .WithMessage("{PropertyName} does not exist.");

        }
    }
}
