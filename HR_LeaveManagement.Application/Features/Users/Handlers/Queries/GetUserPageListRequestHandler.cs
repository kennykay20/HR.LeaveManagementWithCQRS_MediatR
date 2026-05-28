using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.Features.Users.Requests.Queries;
using HR_LeaveManagement.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Users.Handlers.Queries
{
    public class GetUserPageListRequestHandler : IRequestHandler<GetUserPageListRequest, ApiListPageResponse<List<UserListDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserPageListRequestHandler> _logger;

        public GetUserPageListRequestHandler(
            IUserRepository userRepository, 
            IMapper mapper,
            ILogger<GetUserPageListRequestHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiListPageResponse<List<UserListDto>>> Handle(GetUserPageListRequest request, CancellationToken cancellationToken)
        {
            var response = new ApiListPageResponse<List<UserListDto>>();
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            _logger.LogInformation($"PageNum - {pageNumber}, and pageSize - {pageSize}");

            if (pageNumber < 1 || pageSize < 1)
            {
                _logger.LogInformation("Invalid pagination parameters");
                response.Success = false;
                response.Message = "Invalid pagination parameters.";
                response.Data = null!;
                response.Errors = null!;
                return response;
            }

            var results = await _userRepository.GetUserPageListAsync(pageNumber, pageSize);

            var total = results.Count;
            _logger.LogInformation($"total number of users - {total}");

            if (results.Count < 1)
            {
                response.Success = false;
                response.Message = "No data";
                response.Count = 0;
                response.PageNumber = pageNumber;
                response.PageSize = pageSize;
                response.TotalPages = total;
                response.Data = null!;
                return response;
            }

            
            _logger.LogInformation($"total counts = {total}");

            return new ApiListPageResponse<List<UserListDto>>()
            {
                Success = true,
                Errors = null!,
                Message = $"Total number of results = {total}",
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = total,
                TotalPages = (int)Math.Ceiling((double)total / pageSize),
                Data = _mapper.Map<List<UserListDto>>(results)
            };            
        }
    }
}
