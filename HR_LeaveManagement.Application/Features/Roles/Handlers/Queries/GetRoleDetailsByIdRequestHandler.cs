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
    public class GetRoleDetailsByIdRequestHandler : IRequestHandler<GetRoleDetailsByIdRequest, BaseCommandResponse<RoleDetailsDto>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRoleDetailsByIdRequestHandler> _logger;

        public GetRoleDetailsByIdRequestHandler(
            IRoleRepository roleRepository, 
            IMapper mapper, 
            ILogger<GetRoleDetailsByIdRequestHandler> logger)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<RoleDetailsDto>> Handle(GetRoleDetailsByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new BaseCommandResponse<RoleDetailsDto>();
                var roles = await _roleRepository.Get(request.Id);
                var result = _mapper.Map<RoleDetailsDto>(roles);

                response.Success = result != null ? true : false;
                response.Message = "Roles data";
                response.Errors = null!;
                response.Data = result ?? null!;
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
