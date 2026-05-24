using FluentValidation;
using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.DTOs.User.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.DTOs.Auth.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            Include(new IUserDtoValidator());
        }
    }
}
