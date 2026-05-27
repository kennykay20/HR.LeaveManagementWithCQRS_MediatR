using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.Features.Roles.Requests.Commands;
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

namespace HR_LeaveManagement.Application.Features.Roles.Handlers.Commands
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, BaseCommandResponse<UserRole>>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AssignRoleToUserCommandHandler> _logger;

        public AssignRoleToUserCommandHandler(
            IUserRoleRepository userRoleRepository, 
            IUserRepository userRepository, 
            ILogger<AssignRoleToUserCommandHandler> logger)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<UserRole>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Assign role to a user ");
            try
            {
                var response = new BaseCommandResponse<UserRole>();
                var userId = request.assignRoleUserDto.UserId;
                var roleIds = request.assignRoleUserDto.RoleIds;

                var user = await _userRepository.GetUserRolesByUserId(userId);
                
                if (user == null)
                {
                    _logger.LogInformation($" user with id - {userId} not found");
                    response.Success = false;
                    response.Message = "User not found";
                    response.Errors = null!;
                    return response;
                }

                // Remove old roles
                _logger.LogInformation("Remove old roles from a user");
                await _userRoleRepository.RemoveDataRange(user.UserRoles);

                // Add new roles
                _logger.LogInformation("Add new roles to a user");
                var userRoles = roleIds.Select(roleId =>
                    new UserRole
                    {
                        UserId = userId,
                        RoleId = roleId
                    });

                await _userRoleRepository.AddDataRange(userRoles);

                response.Success = true;
                response.Message = "Roles assigned successfully";
                response.Errors = null!;
                response.Id = userId;

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError("error occur {error}", ex.Message);
                throw new NotImplementedException();
            }
        }
    }
}
