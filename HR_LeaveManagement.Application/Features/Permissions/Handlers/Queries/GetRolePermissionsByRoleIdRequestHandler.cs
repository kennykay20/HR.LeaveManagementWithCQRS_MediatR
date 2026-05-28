using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Permission;
using HR_LeaveManagement.Application.DTOs.Role;
using HR_LeaveManagement.Application.Features.Permissions.Requests.Queries;
using HR_LeaveManagement.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Permissions.Handlers.Queries
{
    public class GetRolePermissionsByRoleIdRequestHandler : IRequestHandler<GetRolePermissionsByRoleIdRequest, BaseCommandResponse<List<PermissionDto>>>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRolePermissionsByRoleIdRequestHandler> _logger;

        public GetRolePermissionsByRoleIdRequestHandler(
            IRolePermissionRepository rolePermissionRepository,
            IMapper mapper,
            ILogger<GetRolePermissionsByRoleIdRequestHandler> logger
            )
        {
            _rolePermissionRepository = rolePermissionRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<List<PermissionDto>>> Handle(GetRolePermissionsByRoleIdRequest request, CancellationToken cancellationToken)
        {
            var roleId = request.RoleId;
            var response = new BaseCommandResponse<List<PermissionDto>>();
            _logger.LogInformation($"Get role permissions by role id {roleId}");

            try
            {
                var permissions = await _rolePermissionRepository.GetRolePermissionsByRoleId(roleId);

                foreach (var permission in permissions)
                {
                    _logger.LogInformation($"before mapping {permission.Id}, and {permission.Name}");
                    Console.WriteLine($"{permission.Id} - {permission.Name}");
                }
                _logger.LogInformation($"Get role permissions by role id {permissions[0].Id}");
                var result = _mapper.Map<List<PermissionDto>>(permissions);

                foreach (var permission in result)
                {
                    _logger.LogInformation($"after mapping {permission.Id}, and {permission.Name}");
                    Console.WriteLine($"{permission.Id} - {permission.Name}");
                }
                _logger.LogInformation($"after mapping permission response - count = {result.Count}");

                if (result.Count < 1)
                {
                    response.Success = false;
                    response.Message = "";
                    response.Data = null!;
                    return response;
                }

                response.Success = true;
                response.Message = $"Permissions details of role - {roleId}";
                response.Data = result;
                response.Errors = null!;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occur getting a permission assign to a role - {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
