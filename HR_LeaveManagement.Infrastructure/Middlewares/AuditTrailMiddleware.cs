using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Infrastructure.Middlewares
{
    public class AuditTrailMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditTrailMiddleware(
            RequestDelegate next
            )
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context, 
            IAuditTrailRepository auditTrailRepo,
            ILogger<AuditTrailMiddleware> logger
            )
        {
            await _next(context);

            logger.LogInformation("inside the auth middleware ");
            var path = context.Request.Path.ToString();
            var loginPath = "/api/v1/Auth/login";
            var registerPath = "/api/v1/Auth/register";

            logger.LogInformation($"path {path}, loginPath - {loginPath}, and registerPath - {registerPath}");

            if (context.Request.Method == "GET")
                return;

            AuditTrail audit = new AuditTrail();

            if (path.Equals(loginPath, StringComparison.OrdinalIgnoreCase) ||
                    path.Equals(registerPath, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            logger.LogInformation("not login or register path ");
            audit.Id = Guid.NewGuid();
            audit.UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            audit.Email = context.User.FindFirst(ClaimTypes.Email)?.Value;
            audit.Action = $"{context.Request.Method} {path}";
            audit.Method = context.Request.Method;
            audit.Path = path;
            audit.StatusCode = context.Response.StatusCode;
            audit.IpAddress = context.Connection.RemoteIpAddress?.ToString();
            audit.CreatedAt = DateTime.UtcNow;

            await auditTrailRepo.Add(audit);

            logger.LogInformation("Audit trail saved");
        }
    }
}
