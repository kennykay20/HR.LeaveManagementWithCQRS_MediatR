using HR_LeaveManagement.Application.DTOs.Auth;
using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.Responses;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Auths.Requests.Commands
{
    public class RegisterUserCommand : IRequest<BaseCommandResponse<UserDto>>
    {
        public RegisterDto registerDto { get; set; }
    }
}
