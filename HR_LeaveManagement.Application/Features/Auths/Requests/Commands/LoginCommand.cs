using HR_LeaveManagement.Application.DTOs.Auth;
using HR_LeaveManagement.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Auths.Requests.Commands
{
    public class LoginCommand : IRequest<TokenResponse>
    {
        public LoginDto loginDto { get; set; }
    }
}
