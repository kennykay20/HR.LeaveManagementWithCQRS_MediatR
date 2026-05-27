using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.DTOs.Role.Validators
{
    public class IRoleDtoValidator : AbstractValidator<IRoleDto>
    {
        public IRoleDtoValidator()
        {
            RuleFor(data => data.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not be more than 50 characters.");

            RuleFor(data => data.Description)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not be more than 50 characters.");
        }
    }
}
