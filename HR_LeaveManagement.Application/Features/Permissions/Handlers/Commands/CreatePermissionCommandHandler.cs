using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Permission;
using HR_LeaveManagement.Application.DTOs.Permission.Validators;
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
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, BaseCommandResponse<PermissionDto>>
    {
        private readonly IPermissionRepository _permissionRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePermissionCommandHandler> _logger;

        public CreatePermissionCommandHandler(
            IPermissionRepository permissionRepo, 
            IMapper mapper,
            ILogger<CreatePermissionCommandHandler> logger)
        {
            _permissionRepo = permissionRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<PermissionDto>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var response = new BaseCommandResponse<PermissionDto>();
                var validator = new CreatePermissionDtoValidator();
                var validationResult = await validator.ValidateAsync(request.createPermissionDto);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Create permission failed";
                    response.Errors = validationResult.Errors.Select(er => er.ErrorMessage).ToList();
                    response.Data = null!;
                    return response;
                }

                _logger.LogInformation("Create permission is valid");

                var permission = _mapper.Map<Permission>(request.createPermissionDto);

                var result = await _permissionRepo.Add(permission);

                if (result is null)
                {
                    _logger.LogError("Error occur permission not added successfully");
                    response.Success = false;
                    response.Message = "Permission not added successfully.";
                    response.Data = null!;
                    response.Errors = null!;
                    return response;
                }
                var resultMap = _mapper.Map<PermissionDto>(result);

                _logger.LogInformation("Permission added for Id {id}", resultMap.Id);
                response.Success = true;
                response.Message = "Permission addedd successfully";
                response.Errors = null!;
                response.Data = resultMap;
                response.Id = resultMap.Id;

                _logger.LogInformation("Permission added for Id {id}", resultMap.Id);
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occur while assding a new permission - {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
