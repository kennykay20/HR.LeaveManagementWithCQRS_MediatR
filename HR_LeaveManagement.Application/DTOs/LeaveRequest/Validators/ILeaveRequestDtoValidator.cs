using FluentValidation;
using HR_LeaveManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class ILeaveRequestDtoValidator : AbstractValidator<ILeaveRequestDto>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepo;

        public ILeaveRequestDtoValidator(
            ILeaveTypeRepository leaveTypeRepo
            )
        {
            _leaveTypeRepo = leaveTypeRepo;

            RuleFor(data => data.StartDate)
                .LessThan(data => data.EndDate)
                .WithMessage("{PropertyName} must be before {ComparisonValue}.");

            RuleFor(data => data.EndDate)
                .GreaterThan(data => data.StartDate)
                .WithMessage("{PropertyName} must be after {ComparisonValue}.");

            RuleFor(data => data.Email)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(data => data.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var leaveTypeExist = await _leaveTypeRepo.Exist(id);
                    return leaveTypeExist;
                })
                .WithMessage("{PropertyName} does not exist.");

            RuleFor(data => data.RequestComments)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");
            
        }
    }
}
