using FluentValidation;
using HR_LeaveManagement.Application.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class UpdateLeaveRequestDtoValidator : AbstractValidator<UpdateLeaveRequestDto>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;

        public UpdateLeaveRequestDtoValidator(ILeaveRequestRepository leaveRequestRepo)
        {
            _leaveRequestRepo = leaveRequestRepo;
            Include(new ILeaveRequestDtoValidator(_leaveRequestRepo));

            RuleFor(data => data.Cancelled)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");

            RuleFor(data => data.Id).NotNull().WithMessage("{PropertyName} must be present.");
        }
    }
}
