using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.DTOs
{
    public sealed record JwtValidationResultDto
    (string Email, string Id, string Roles, string Permissions);
}
