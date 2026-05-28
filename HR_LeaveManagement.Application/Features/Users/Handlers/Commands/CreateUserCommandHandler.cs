using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.DTOs.User.Validators;
using HR_LeaveManagement.Application.Features.Users.Requests.Commands;
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

namespace HR_LeaveManagement.Application.Features.Users.Handlers.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseCommandResponse<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            IUserRepository userRepository, 
            IMapper mapper,  
            IPasswordHelper passwordHelper,
            ILogger<CreateUserCommandHandler> logger
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHelper = passwordHelper;
            _logger = logger;
        }
        public async Task<BaseCommandResponse<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new BaseCommandResponse<UserDto>();
                var validator = new CreateUserDtoValidator();
                var validationResult = await validator.ValidateAsync(request.createUserRequestDto);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "User creation failed.";
                    response.Errors = validationResult.Errors.Select(er => er.ErrorMessage).ToList();
                    response.Data = null!;
                    return response;
                }
                var salt = _passwordHelper.GenerateSalt();
                var passwordHash = _passwordHelper.GenerateHashPassword(request.createUserRequestDto.Password, salt);

                var user = _mapper.Map<User>(request.createUserRequestDto);
                user.IsActive = false;
                user.Password = passwordHash;
                user.IsNewUser = true;
                //user.IsDeleted = false;
                //user.Roles = "Admin";

                var userAdded = await _userRepository.Add(user);
                var result = _mapper.Map<UserDto>(userAdded);

                response.Success = true;
                response.Message = "User created successfully";
                response.Errors = null!;
                response.Id = result.Id;
                response.Data = result;

                return response;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occur {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
