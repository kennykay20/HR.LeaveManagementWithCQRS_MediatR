using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Templates;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Auth.Validators;
using HR_LeaveManagement.Application.DTOs.User;
using HR_LeaveManagement.Application.Features.Auths.Requests.Commands;
using HR_LeaveManagement.Application.Models;
using HR_LeaveManagement.Application.Responses;
using HR_LeaveManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
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
        private readonly IEmailJobService _emailJobService;
        private readonly IConfiguration _configuration;

        public RegisterUserCommandHandler(
            IUserRepository userRepository, 
            IMapper mapper, 
            IPasswordHelper passwordHelper, 
            IOtpService otpService,
            IEmailJobService emailJobService,
            IConfiguration configuration
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHelper = passwordHelper;
            _otpService = otpService;
            _emailJobService = emailJobService;
            _configuration = configuration;
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
            //user.Roles = "Admin";

            var result = await _userRepository.Add(user);

            try
            {
                // generate an otp
                var otp = _otpService.GenerateOtp();
                // Send otp to user's email;
                var fullName = request.registerDto.FirstName + " " + request.registerDto.LastName;
                var appURL = Environment.GetEnvironmentVariable("BASE_URL") ?? _configuration["BASE:URL"];
                Console.WriteLine("Base URL = ", appURL);
                var fullVerifyUrl = $"{appURL}/api/v1/auth/verify-email?otp={otp}";
                var emailData = new Email
                {
                    To = request.registerDto.Email ?? "kennyoluwadamilare20@gmail.com",
                    Subject = "Register User Verification",
                    Body = EmailTemplateGetter.RegisterNotification(fullName, fullVerifyUrl)
                };

                try
                {
                    Console.WriteLine($"Hangfire started sending registration user notification to {emailData.To}");
                    _emailJobService.QueueLeaveRequestEmail(emailData);
                }
                catch (Exception ex)
                {
                    // Log or handler error
                    Console.WriteLine("Error sending email notification " + ex.Message);
                }

                result.Otp = otp;
                result.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

                // update the user table
                await _userRepository.Update(result);

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
