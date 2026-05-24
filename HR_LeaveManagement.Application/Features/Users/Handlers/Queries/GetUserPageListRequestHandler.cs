using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.Features.Users.Requests.Queries;
using HR_LeaveManagement.Application.Responses;
using MediatR;
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

        public GetUserPageListRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ApiListPageResponse<List<UserListDto>>> Handle(GetUserPageListRequest request, CancellationToken cancellationToken)
        {
            var response = new ApiListPageResponse<List<UserListDto>>();
            var pageNumber = request.userPageDto.PageNumber;
            var pageSize = request.userPageDto.PageSize;

            if (pageNumber < 1 || pageSize < 1)
            {
                response.Success = false;
                response.Message = "Invalid pagination parameters.";
                response.Data = null;
                return response;
            }

            var results = await _userRepository.GetUserPageListAsync(pageNumber, pageSize);

            if (results.Count < 1)
            {
                response.Success = false;
                response.Message = "";
                response.Count = 0;
                response.PageNumber = pageNumber;
                response.PageSize = pageSize;
                response.TotalPages = results.Count;
                response.Data = null;
            }

            var total = results.Count;
            return new ApiListPageResponse<List<UserListDto>>()
            {
                Success = true,
                Errors = null,
                Message = $"Total number of results = {total}",
                PageNumber = pageNumber,
                PageSize = pageSize,
                Count = results.Count,
                TotalPages = (int)Math.Ceiling((double)total / pageSize),
                Data = _mapper.Map<List<UserListDto>>(results)
            };            
        }
    }
}
