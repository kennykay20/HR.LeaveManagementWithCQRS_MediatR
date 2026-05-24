using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.Features.Users.Requests.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Users.Handlers.Queries
{
    public class GetUserDetailByEmailRequestHandler : IRequestHandler<GetUserDetailByEmailRequest, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserDetailByEmailRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDto> Handle(GetUserDetailByEmailRequest request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetUserByEmail(request.Email);

            return _mapper.Map<UserDto>(result);
        }
    }
}
