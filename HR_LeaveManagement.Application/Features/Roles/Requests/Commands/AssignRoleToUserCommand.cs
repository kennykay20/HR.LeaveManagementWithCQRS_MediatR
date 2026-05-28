using HR_LeaveManagement.Application.DTOs.Role;
using HR_LeaveManagement.Application.Responses;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Roles.Requests.Commands
{
    public class AssignRoleToUserCommand : IRequest<BaseCommandResponse<string>>
    {
        public AssignRoleToUserDto assignRoleUserDto { get; set; }
    }
}
