using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Permission;
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
    public class GetPermissionDetailsByIdRequestHandler : IRequestHandler<GetPermissionDetailsByIdRequest, BaseCommandResponse<PermissionDto>>
    {
        private readonly IPermissionRepository _permissionRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPermissionDetailsByIdRequestHandler> _logger;

        public GetPermissionDetailsByIdRequestHandler(
            IPermissionRepository permissionRepo, 
            IMapper mapper, 
            ILogger<GetPermissionDetailsByIdRequestHandler> logger
            )
        {
            _permissionRepo = permissionRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<PermissionDto>> Handle(GetPermissionDetailsByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new BaseCommandResponse<PermissionDto>();
                var permission = await _permissionRepo.Get(request.Id);

                var result = _mapper.Map<PermissionDto>(permission);

                response.Success = result != null ? true : false;
                response.Message = "Permission data";
                response.Errors = null!;
                response.Data = result != null ? result : null!;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occur {ex.Message}");
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
