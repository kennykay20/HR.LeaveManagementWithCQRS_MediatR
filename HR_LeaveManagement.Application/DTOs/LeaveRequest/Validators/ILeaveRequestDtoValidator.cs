using FluentValidation;
using HR_LeaveManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class ILeaveRequestDtoValidator : AbstractValidator<ILeaveRequestDto>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;

        public ILeaveRequestDtoValidator(ILeaveRequestRepository leaveRequestRepo)
        {
            _leaveRequestRepo = leaveRequestRepo;

            RuleFor(data => data.StartDate)
                .LessThan(data => data.EndDate)
                .WithMessage("{PropertyName} must be before {ComparisonValue}.");

            RuleFor(data => data.EndDate)
                .GreaterThan(data => data.StartDate)
                .WithMessage("{PropertyName} must be after {ComparisonValue}.");

            RuleFor(data => data.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var leaveRequestExist = await _leaveRequestRepo.Exist(id);
                    return !leaveRequestExist;
                })
                .WithMessage("{PropertyName} does not exist.");

            RuleFor(data => data.RequestComments)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");
        }
    }
}
