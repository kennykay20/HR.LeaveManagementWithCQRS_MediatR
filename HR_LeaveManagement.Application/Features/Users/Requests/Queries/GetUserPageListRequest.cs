using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Users.Requests.Queries
{
    public class GetUserPageListRequest : IRequest<ApiListPageResponse<List<UserListDto>>>
    {
        public UserPageDto userPageDto { get; set; }
    }
}
