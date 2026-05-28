using HR_LeaveManagement.Application.DTOs.Permission;
using HR_LeaveManagement.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Permissions.Requests.Commands
{
    public class AssignPermissionToRoleCommand : IRequest<BaseCommandResponse<string>>
    {
        public AssignPermissionToRoleDto assignPermissionToRole { get; set; }
    }
}
