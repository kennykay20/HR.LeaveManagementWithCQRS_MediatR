using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence.Utils
{
    public class AuditService : IAuditService
    {
        private readonly IAuditTrailRepository _auditTrailRepo;
        private readonly ILogger<AuditService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public AuditService(
            IAuditTrailRepository auditTrailRepo,
            ILogger<AuditService> logger,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _auditTrailRepo = auditTrailRepo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string userId, string email, string path)
        {
            _logger.LogInformation("inside the audit serivce - logAsync ");
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "";

            var anotherPath = context?.Request?.Path;

            var statusCode = context?.Response?.StatusCode ?? 0;

            _logger.LogInformation($"user id {userId}, email - {email}, path - {path}, anotherPath - {anotherPath}, and ipAddress - {ipAddress} ");

            var audit = new AuditTrail
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Action = $"POST {path}",
                Method = "POST",
                Path = path,
                StatusCode = statusCode,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepo.Add(audit);
            _logger.LogInformation($"Login audit for user {email}");
        }


        public async Task FailedLoginAsync(string userId, string email)
        {
            _logger.LogInformation("inside the audit serivce - failedLogin ");
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "";

            var statusCode = context?.Response?.StatusCode ?? 0;

            _logger.LogInformation($"user id {userId}, email - {email}, and ipAddress - {ipAddress} ");

            var audit = new AuditTrail
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Action = $"FAILED_LOGIN",
                Method = "POST",
                Path = "",
                StatusCode = statusCode,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepo.Add(audit);
            _logger.LogInformation($"Failed Login for email {email}");
        }

        public async Task FailedRegistrationAsync(string userId, string email)
        {
            _logger.LogInformation("inside the audit serivce - failedRegistration ");
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            var statusCode = context?.Response?.StatusCode ?? 0;

            _logger.LogInformation($"user id {userId}, email - {email}, and ipAddress - {ipAddress} ");

            var audit = new AuditTrail
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Action = $"FAILED_REGISTRATION",
                Method = "POST",
                Path = "",
                StatusCode = statusCode,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepo.Add(audit);
            _logger.LogInformation($"Failed Registration for email {email}");
        }

        public async Task PasswordResetAsync(string userId, string email)
        {
            _logger.LogInformation("inside the audit serivce - passwordResetAsync ");
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "";

            _logger.LogInformation($"user id {userId}, email - {email}, and ipAddress - {ipAddress} ");

            var audit = new AuditTrail
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Action = $"PASSWORD_RESET",
                Method = "POST",
                Path = "",
                StatusCode = 201,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepo.Add(audit);
            _logger.LogInformation($"Password reset for email {email}");
        }

        public async Task PermissionChangeAsync(string userId, string email)
        {
            _logger.LogInformation("inside the audit serivce - permissionChangeAsync ");
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "";

            _logger.LogInformation($"user id {userId}, email - {email}, and ipAddress - {ipAddress} ");

            var audit = new AuditTrail
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Action = $"PERMISSION_CHANGE",
                Method = "POST",
                Path = "",
                StatusCode = 201,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepo.Add(audit);
            _logger.LogInformation($"Permission change for email {email}");
        }

        public async Task RefreshTokenAsync(string userId, string email)
        {
            _logger.LogInformation("inside the audit serivce - refreshTokenAsync ");
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "";

            _logger.LogInformation($"user id {userId}, email - {email}, and ipAddress - {ipAddress} ");

            var audit = new AuditTrail
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Action = $"REFRESH_TOKEN",
                Method = "POST",
                Path = "",
                StatusCode = 201,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepo.Add(audit);
            _logger.LogInformation($"Refresh token for email {email}");
        }

        public async Task RoleChangeAsync(string userId, string email)
        {
            _logger.LogInformation("inside the audit serivce - roleChangeAsync ");
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "";

            _logger.LogInformation($"user id {userId}, email - {email}, and ipAddress - {ipAddress} ");

            var audit = new AuditTrail
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Action = $"ROLE_CHANGE",
                Method = "POST",
                Path = "",
                StatusCode = 201,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepo.Add(audit);
            _logger.LogInformation($"Role change for email {email}");
        }

        public async Task SuccessfulLoginAsync(string userId, string email)
        {
            _logger.LogInformation("inside the audit serivce - successfulLoginAsync ");
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "";

            _logger.LogInformation($"user id {userId}, email - {email}, and ipAddress - {ipAddress} ");

            var audit = new AuditTrail
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Action = $"SUCCESSFUL_LOGIN",
                Method = "POST",
                Path = "",
                StatusCode = 201,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepo.Add(audit);
            _logger.LogInformation($"Successful Login for email {email}");
        }

        public async Task SuccessfulRegistrationAsync(string userId, string email)
        {
            _logger.LogInformation("inside the audit serivce - successfulRegistrationAsync ");
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "";

            _logger.LogInformation($"user id {userId}, email - {email}, and ipAddress - {ipAddress} ");

            var audit = new AuditTrail
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Action = $"SUCCESSFUL_REGISTRATION",
                Method = "POST",
                Path = "",
                StatusCode = 201,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            await _auditTrailRepo.Add(audit);
            _logger.LogInformation($"Successful register for email {email}");
        }
    }
}
