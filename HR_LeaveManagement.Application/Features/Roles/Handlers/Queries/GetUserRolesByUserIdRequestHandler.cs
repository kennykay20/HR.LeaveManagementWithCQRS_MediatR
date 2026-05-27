using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Role;
using HR_LeaveManagement.Application.Features.Roles.Requests.Queries;
using HR_LeaveManagement.Application.Responses;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Roles.Handlers.Queries
{
    public class GetUserRolesByUserIdRequestHandler : IRequestHandler<GetUserRolesByUserIdRequest, BaseCommandResponse<List<RoleDto>>>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserRolesByUserIdRequestHandler> _logger;

        public GetUserRolesByUserIdRequestHandler(
            IUserRoleRepository userRoleRepository, 
            IMapper mapper,
            ILogger<GetUserRolesByUserIdRequestHandler> logger)
        {
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<List<RoleDto>>> Handle(GetUserRolesByUserIdRequest request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var response = new BaseCommandResponse<List<RoleDto>>();
            _logger.LogInformation($"Get user roles by user id {userId}");

            var roles = await _userRoleRepository.GetUserRolesByUserId(userId);

            foreach (var role in roles)
            {
                _logger.LogInformation($"before mapping {role.Id}, and {role.Name}");
                Console.WriteLine($"{role.Id} - {role.Name}");
            }
            _logger.LogInformation($"Get user roles by user id {roles[0].Id} and id {roles[1].Id}");
            var result = _mapper.Map<List<RoleDto>>(roles);

            foreach (var role in result)
            {
                _logger.LogInformation($"after mapping {role.Id}, and {role.Name}");
                Console.WriteLine($"{role.Id} - {role.Name}");
            }
            _logger.LogInformation($"after mapping roles response - count = {result.Count}");

            if (result.Count < 1)
            {
                response.Success = false;
                response.Message = "";
                response.Data = null!;
                return response;
            }

            response.Success = true;
            response.Message = $"Roles details of user {userId}";
            response.Data = result;
            response.Errors = null!;

            return response;
        }
    }
}
