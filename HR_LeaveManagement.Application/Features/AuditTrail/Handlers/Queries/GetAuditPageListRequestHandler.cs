using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Audit;
using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.Features.AuditTrail.Requests.Queries;
using HR_LeaveManagement.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.AuditTrail.Handlers.Queries
{
    public class GetAuditPageListRequestHandler : IRequestHandler<GetAuditPageListRequest, ApiListPageResponse<List<AuditDto>>>
    {
        private readonly IAuditTrailRepository _auditTrailRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAuditPageListRequestHandler> _logger;

        public GetAuditPageListRequestHandler(
            IAuditTrailRepository auditTrailRepo, 
            IMapper mapper,
            ILogger<GetAuditPageListRequestHandler> logger)
        {
            _auditTrailRepo = auditTrailRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiListPageResponse<List<AuditDto>>> Handle(GetAuditPageListRequest request, CancellationToken cancellationToken)
        {
            var response = new ApiListPageResponse<List<AuditDto>>();
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            _logger.LogInformation($"PageNum - {pageNumber}, and pageSize - {pageSize}");

            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    _logger.LogInformation("Invalid pagination parameters");
                    response.Success = false;
                    response.Message = "Invalid pagination parameters.";
                    response.Data = null!;
                    response.Errors = null!;
                    return response;
                }

                var results = await _auditTrailRepo.GetAuditPageListAsync(pageNumber, pageSize);

                var totalCount = await _auditTrailRepo.GetTotalAuditCountAsync();
                _logger.LogInformation($"total count - {totalCount}");

                if (results == null || results.Count == 0)
                {
                    response.Success = false;
                    response.Message = "No data";
                    response.Count = 0;
                    response.PageNumber = pageNumber;
                    response.PageSize = pageSize;
                    response.TotalPages = totalCount / pageSize;
                    response.TotalCount = totalCount;
                    response.Data = null!;
                    return response;
                }

                _logger.LogInformation($"total counts = {totalCount}");

                return new ApiListPageResponse<List<AuditDto>>()
                {
                    Success = true,
                    Errors = null!,
                    Message = $"Total number of results = {totalCount}",
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Count = results.Count,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    TotalCount = totalCount,
                    Data = _mapper.Map<List<AuditDto>>(results)
                };


            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occur fetching an audit trail {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
