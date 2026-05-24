using HR_LeaveManagement.Application.DTOs.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Users.Requests.Queries
{
    public class GetUserDetailByEmailRequest : IRequest<UserDto>
    {
        public string Email { get; set; }
    }
}
