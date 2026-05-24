using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.DTOs.User.Validators
{
    public class IUserDtoValidator : AbstractValidator<IUserDto>
    {
        public IUserDtoValidator()
        {
            RuleFor(data => data.FirstName)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not be more than 50 characters.");

            RuleFor(data => data.LastName)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not be more than 50 characters.");

            RuleFor(data => data.Email)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(data => data.Password)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")

                .MinimumLength(8)
                .WithMessage("{PropertyName} must not be less than 8 characters")

                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$")
                .WithMessage("{PropertyName} must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
        }
    }
}
