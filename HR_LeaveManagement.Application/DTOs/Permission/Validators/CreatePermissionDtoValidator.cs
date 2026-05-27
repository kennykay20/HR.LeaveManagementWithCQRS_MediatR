using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.DTOs.Permission.Validators
{
    public class CreatePermissionDtoValidator : AbstractValidator<CreatePermissionDto>
    {
        public CreatePermissionDtoValidator()
        {
            Include(new IPermissionDtoValidator());
        }
    }
}
