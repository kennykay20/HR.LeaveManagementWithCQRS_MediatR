using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.DTOs.LeaveType.Validators
{
    public class UpdateLeaveTypeDtoValidator : AbstractValidator<LeaveTypeDto>
    {
        public UpdateLeaveTypeDtoValidator()
        {
            Include(new ILeaveTypeDtoValidator());

            RuleFor(data => data.Id).NotNull().WithMessage("{PropertyName} must be present.");
        }
    }
}
