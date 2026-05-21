using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class ChangeApproveLeaveRequestDtoValidator : AbstractValidator<ChangeLeaveRequestApprovalDto>
    {
        public ChangeApproveLeaveRequestDtoValidator()
        {
            RuleFor(data => data.Id).NotNull().WithMessage("{PropertyName} must be present.");

            RuleFor(data => data.Approved)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");
        }
    }
}
