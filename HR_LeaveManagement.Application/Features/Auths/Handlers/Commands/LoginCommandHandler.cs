using AutoMapper;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Application.DTOs.Auth.Validators;
using HR_LeaveManagement.Application.Features.Auths.Requests.Commands;
using HR_LeaveManagement.Application.Responses;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Features.Auths.Handlers.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IClaimsService _claimsService;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IUserRepository userRepository, 
            IJwtService jwtService,
            IClaimsService claimsService,
            IPasswordHelper passwordHelper,
            ILogger<LoginCommandHandler> logger
            )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _claimsService = claimsService;
            _passwordHelper = passwordHelper;
            _logger = logger;
        }

        //public IClaimsService ClaimsService { get; }

        public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new TokenResponse();
            var validator = new LoginDtoValidator();
            var validationResult = await validator.ValidateAsync(request.loginDto);
            var isMatch = false;

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Login failed";
                response.Errors = validationResult.Errors.Select(er => er.ErrorMessage).ToList();
                response.AccessToken = "";
                response.RefreshToken = "";
                return response;
            }
            _logger.LogInformation("Validator is valid ");
            try
            {
                var user = await _userRepository.GetUserByEmail(request.loginDto.Email);
                if (user is null)
                {
                    response.Success = false;
                    response.Message = "User not foind, Please sign up or register.";
                    response.Errors = null;
                    response.AccessToken = "";
                    response.RefreshToken = "";
                    return response;
                }

                _logger.LogInformation($"Is user active {user.IsActive}, email = {user.Email}");

                if (!user.IsActive)
                {
                    if (user.IsNewUser)
                    {
                        response.Success = false;
                        response.Message = "Account not verified, Please check your email for verification link.";
                        response.Errors = null;
                        response.AccessToken = "";
                        response.RefreshToken = "";
                        return response;
                    }
                    else
                    {
                        _logger.LogInformation($"Email {user.Email} has been de-activated. ");
                        response.Success = false;
                        response.Message = "User has been de-activated.";
                        response.Errors = null;
                        response.AccessToken = "";
                        response.RefreshToken = "";
                        return response;
                    }
                }

                var hashPassword = user.Password;
                if (!string.IsNullOrEmpty(hashPassword))
                {
                    isMatch = _passwordHelper.VerifyHashPassword(request.loginDto.Password, hashPassword);
                }
                _logger.LogInformation($"Is login password match {isMatch}");
                if (!isMatch)
                {
                    _logger.LogInformation("Invalid login password ");
                    response.Success = false;
                    response.Message = "Invalid login password.";
                    response.Errors = null;
                    response.AccessToken = "";
                    response.RefreshToken = "";
                    return response;
                }

                var token = await GenerateAccessToken(user.Id);
                user.RegistrationToken = token ?? "";
                // update the user table
                await _userRepository.Update(user);

                var newData = await GenerateNewRefreshToken(user.Id);
                
                response.Success = true;
                response.Message = "Login successfully";
                response.Errors = null!;
                response.AccessToken = token ?? "";
                response.RefreshToken = newData.RefreshToken ?? "";

                return response;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> GenerateAccessToken(int userId)
        {
            var user = await _userRepository.Get(userId);
            if (user is null)
            {
                return null!;
            }

            var claims = await _claimsService.GetUserClaimsAsync(user);
            _logger.LogInformation($"token claims - {claims} ");
            var token = _jwtService.GenerateAccessToken(user!, claims);
            return token;
        }

        private async Task<TokenResponse> GenerateNewRefreshToken(int userId)
        {
            var response = new TokenResponse();
            var user = await _userRepository.Get(userId);
            if (user is null)
            {
                response.Success = false;
                response.Errors = null;
                response.Message = "User not found";
                response.AccessToken = "";
                response.RefreshToken = "";
                return response;
            }

            var refreshToken = _jwtService.GenerateRefreshToken();

            _logger.LogInformation($"refreshToken generated {refreshToken} ");
            Console.WriteLine("refreshToken generated ", refreshToken);

            var hashedRefreshToken = SHA256.HashData(
                Encoding.UTF8.GetBytes(refreshToken)
            );

            _logger.LogInformation($"save hashed refreshtoken {hashedRefreshToken} ");
            _logger.LogInformation($"save hashed refreshtoken tostring {hashedRefreshToken.ToString()} ");
            Console.WriteLine("save hashed refreshtoken ", hashedRefreshToken);

            user!.RefreshToken = hashedRefreshToken.ToString();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.Update(user);

            response.Success = true;
            response.Message = "Refresh token generated";
            response.AccessToken = "";
            response.RefreshToken = hashedRefreshToken.ToString()!;
            response.Errors = null!;
            return response;
        }
    }
}
