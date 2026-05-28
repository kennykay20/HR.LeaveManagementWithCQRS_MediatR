using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Permission;
using HR_LeaveManagement.Application.Features.Permissions.Requests.Queries;
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

namespace HR_LeaveManagement.Application.Features.Permissions.Handlers.Queries
{
    public class GetPermissionsRequestHandler : IRequestHandler<GetPermissionsRequest, BaseCommandResponse<List<PermissionDto>>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPermissionsRequestHandler> _logger;

        public GetPermissionsRequestHandler(
            IPermissionRepository permissionRepository, 
            IMapper mapper,
            ILogger<GetPermissionsRequestHandler> logger)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<List<PermissionDto>>> Handle(GetPermissionsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new BaseCommandResponse<List<PermissionDto>>();
                var permissions = await _permissionRepository.GetAll();

                var result = _mapper.Map<List<PermissionDto>>(permissions);

                response.Success = result.Count > 0 ? true : false;
                response.Message = "All List of permissions";
                response.Errors = null!;
                response.Data = result.Count > 0 ? result : null!;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occur while getting permissions {ex.Message}");
                throw new Exception(ex.Message);
            }
            
        }
    }
}
