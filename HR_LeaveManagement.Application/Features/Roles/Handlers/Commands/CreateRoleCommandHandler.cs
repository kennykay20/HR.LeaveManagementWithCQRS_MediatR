using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Role;
using HR_LeaveManagement.Application.DTOs.Role.Validators;
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
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, BaseCommandResponse<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRoleCommandHandler> _logger;

        public CreateRoleCommandHandler(
            IRoleRepository roleRepository, 
            IMapper mapper,
            ILogger<CreateRoleCommandHandler> logger)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new BaseCommandResponse<RoleDto>();
                var validator = new CreateRoleDtoValidator();
                var validationResult = await validator.ValidateAsync(request.createRoleDto);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Create Role failed";
                    response.Errors = validationResult.Errors.Select(er => er.ErrorMessage).ToList();
                    response.Data = null!;
                    return response;
                }

                _logger.LogInformation("request data ");
                _logger.LogInformation("Name: {name} and description: {desc} ", request.createRoleDto.Name, request.createRoleDto.Description);

                var role = _mapper.Map<Role>(request.createRoleDto);
                role.IsDeleted = false;

                _logger.LogInformation("after mapping {role} = ", role);
                var result = await _roleRepository.Add(role);

                _logger.LogInformation("after adding, role id = {id}", result.Id);

                response.Id = result.Id;
                response.Success = true;
                response.Message = "Role added successfully!";
                response.Errors = null!;
                response.Data = null!;

                return response;
            }
            catch(Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
