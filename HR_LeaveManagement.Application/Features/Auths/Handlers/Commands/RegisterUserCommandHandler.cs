using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Auth.Validators;
using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.Features.Auths.Requests.Commands;
using HR_LeaveManagement.Application.Responses;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Auths.Handlers.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, BaseCommandResponse<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IOtpService _otpService;

        public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper, IPasswordHelper passwordHelper, IOtpService otpService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHelper = passwordHelper;
            _otpService = otpService;
        }
        public async Task<BaseCommandResponse<UserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse<UserDto>();
            var validator = new RegisterDtoValidator();
            var validationResult = await validator.ValidateAsync(request.registerDto);
            var email = request.registerDto.Email;
            var password = request.registerDto.Password;

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "User Registration Failed.";
                response.Errors = validationResult.Errors.Select(er => er.ErrorMessage).ToList();
                response.Data = null;
                return response;
            }

            var emailExist = await _userRepository.GetUserByEmail(email);
            if (emailExist != null)
            {
                response.Success = false;
                response.Message = "Email already exist";
                response.Data = null;
                response.Errors = null;

                return response;
            }

            var salt = _passwordHelper.GenerateSalt();
            var passwordHash = _passwordHelper.GenerateHashPassword(password, salt);

            var user = _mapper.Map<User>(request.registerDto);
            user.IsActive = false;
            user.Password = passwordHash;
            user.IsNewUser = true;
            user.IsDeleted = false;
            user.Roles = "Admin";

            var result = await _userRepository.Add(user);

            try
            {
                // generate an otp
                var otp = _otpService.GenerateOtp();
                // Send otp to user's eamil;
                result.Otp = otp;
                result.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

                // update the user table

                response.Success = true;
                response.Message = "New user registered successfully, Please check your email to verify your account";
                response.Id = result.Id;

                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error registering a user ", ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
