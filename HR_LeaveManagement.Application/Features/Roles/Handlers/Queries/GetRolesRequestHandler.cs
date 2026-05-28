using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Role;
using HR_LeaveManagement.Application.Features.Roles.Requests.Queries;
using HR_LeaveManagement.Application.Responses;
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
    public class GetRolesRequestHandler : IRequestHandler<GetRolesRequest, BaseCommandResponse<List<RoleDto>>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRolesRequestHandler> _logger;

        public GetRolesRequestHandler(
            IRoleRepository roleRepository, 
            IMapper mapper, 
            ILogger<GetRolesRequestHandler> logger
            )
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<List<RoleDto>>> Handle(GetRolesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new BaseCommandResponse<List<RoleDto>>();

                _logger.LogInformation("Get Role details");

                var roles = await _roleRepository.GetAll();

                var result = _mapper.Map<List<RoleDto>>(roles);

                response.Success = result.Count > 0 ? true : false;
                response.Message = $"Roles = {result.Count}";
                response.Errors = null!;
                response.Data = result.Count > 0 ? result : null!;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occur while getting roles {error}", ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
