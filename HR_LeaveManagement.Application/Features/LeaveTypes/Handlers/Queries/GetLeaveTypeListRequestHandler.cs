using AutoMapper;
using HR_LeaveManagement.Application.DTOs.LeaveType;
using HR_LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using HR_LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace HR_LeaveManagement.Application.Features.LeaveTypes.Handlers.Queries
{
    public class GetLeaveTypeListRequestHandler : IRequestHandler<GetLeaveTypeListRequest, List<LeaveTypeDto>>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<GetLeaveTypeListRequestHandler> _logger;
        private const string LeaveTypesCacheKey = "leave-types";
        public GetLeaveTypeListRequestHandler(
            ILeaveTypeRepository leaveTypeRepository,
            IMapper mapper,
            ICacheService cacheService,
            ILogger<GetLeaveTypeListRequestHandler> logger
            )
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }
        public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypeListRequest request, CancellationToken cancellationToken)
        {
            // check cache
            _logger.LogInformation("check cache memory");
            var cached = await _cacheService.GetAsync<List<LeaveTypeDto>>(LeaveTypesCacheKey);
            if (cached is not null)
            {
                _logger.LogInformation("Leave types returned from redis");
                return cached;
            }

            var leaveTypes = await _leaveTypeRepository.GetAll();
            var result = _mapper.Map<List<LeaveTypeDto>>(leaveTypes);

            await _cacheService.SetAsync(LeaveTypesCacheKey, result, TimeSpan.FromMinutes(30));

            return result;
        }
    }
}
