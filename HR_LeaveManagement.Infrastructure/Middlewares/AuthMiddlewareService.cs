using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Infrastructure.Middlewares
{
    public class AuthMiddlewareService
    {
        private readonly RequestDelegate _nextDelegate;
        private readonly ILogger<AuthMiddlewareService> _logger;

        public AuthMiddlewareService( 
            RequestDelegate requestDelegate, 
            ILogger<AuthMiddlewareService> logger
            )
        {
            _nextDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        {
            try
            {
                if (!context.Request.Headers.TryGetValue("userToken", out var tokenHeader))
                {
                    _logger.LogWarning("Missing 'userToken' header");
                    await WriteJsonError(context, 400, "Missing 'userToken' header.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(tokenHeader))
                {
                    _logger.LogWarning("Unauthorized client '{Client}'", tokenHeader.ToString());
                    await WriteJsonError(context, 401, "Unauthorized.");
                    return;
                }

                if (!string.IsNullOrEmpty(tokenHeader))
                {
                    var validateToken = await HandleTokenValidationAsync(context, tokenHeader!, jwtService);
                    if (!validateToken)
                    {
                        await WriteJsonError(context, 401, "Unauthorized");
                        return;
                    }
                    await _nextDelegate(context);
                    return;
                }
            }
            catch(Exception ex)
            {
                Log.Fatal("An error occured", ex);
                _logger.LogInformation($"An error occured {ex.Message}");
            }

            //await _requestDelegate(context);
        }

        public async Task<bool> HandleTokenValidationAsync(HttpContext context, string rawToken, IJwtService jwtService)
        {
            var result = jwtService.ValidateAndExtractToken(rawToken);
            if (result != null) 
            {
                //context.Items["Email"] = result.Email ?? "";
                //context.Items["UserId"] = result.Id ?? "";
                //context.Items["Roles"] = result.Roles ?? "";
                //context.Items["Permissions"] = result.Permissions ?? "";
                return true;
            }
            return false;
        }

        private async Task WriteJsonError(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var error = new BaseCommandResponse<string>
            {
                Success = false,
                Message = message,
                Data = null!,
                Errors = null!
            };

            var json = JsonSerializer.Serialize(error);
            await context.Response.WriteAsync(json);
        }
    }
}
