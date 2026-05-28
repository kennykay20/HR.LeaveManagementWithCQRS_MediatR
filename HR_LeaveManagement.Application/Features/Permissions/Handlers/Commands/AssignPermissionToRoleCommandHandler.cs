using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.Features.Permissions.Requests.Commands;
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

namespace HR_LeaveManagement.Application.Features.Permissions.Handlers.Commands
{
    public class AssignPermissionToRoleCommandHandler : IRequestHandler<AssignPermissionToRoleCommand, BaseCommandResponse<string>>
    {
        private readonly IRolePermissionRepository _rolePermissionRepo;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<AssignPermissionToRoleCommandHandler> _logger;

        public AssignPermissionToRoleCommandHandler(
            IRolePermissionRepository rolePermissionRepo,
            IRoleRepository roleRepository,
            ILogger<AssignPermissionToRoleCommandHandler> logger)
        {
            _rolePermissionRepo = rolePermissionRepo;
            _roleRepository = roleRepository;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<string>> Handle(AssignPermissionToRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Assign permission to a role ");

            try
            {
                var response = new BaseCommandResponse<string>();
                var roleId = request.assignPermissionToRole.RoleId;
                var permissionIds = request.assignPermissionToRole.PermissionIds;

                var role = await _roleRepository.GetRolePermissionsByRoleId(roleId);

                if (role == null)
                {
                    _logger.LogInformation($" role with id - {roleId} not found"); // 403 not found
                    response.Success = false;
                    response.Message = "Role not found";
                    response.Errors = null!;
                    return response;
                }

                // Remove old permissions
                _logger.LogInformation("Remove old permission from a role");
                await _rolePermissionRepo.RemoveDataRange(role.RolePermissions);

                // Add new permissions
                _logger.LogInformation("Add new permission to a role");
                var rolePermissions = permissionIds.Select(permissionId =>
                    new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permissionId
                    });

                await _rolePermissionRepo.AddDataRange(rolePermissions);

                response.Success = true;
                response.Message = "Permission assigned successfully";
                response.Errors = null!;
                response.Id = roleId;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occur - {ex.Message} ");
                throw new Exception(ex.Message);
            }
        }
    }
}
