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

            var path = context.Request.Path.ToString();

            if (context.Request.Method == "GET")
                return;

            var audit = new AuditTrail
            {
                Id = Guid.NewGuid(),
                UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = context.User.FindFirst(ClaimTypes.Email)?.Value,
                Action = $"{context.Request.Method} {path}",
                Method = context.Request.Method,
                Path = path,
                StatusCode = context.Response.StatusCode,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            await auditTrailRepo.Add(audit);

            logger.LogInformation("Audit trail saved");
        }
    }
}
