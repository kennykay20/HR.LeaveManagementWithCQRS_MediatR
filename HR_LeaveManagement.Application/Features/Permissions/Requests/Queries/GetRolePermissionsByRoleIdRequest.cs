using HR_LeaveManagement.Application.DTOs.Permission;
using HR_LeaveManagement.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Permissions.Requests.Queries
{
    public class GetRolePermissionsByRoleIdRequest : IRequest<BaseCommandResponse<List<PermissionDto>>>
    {
        public int RoleId { get; set; }
    }
}
