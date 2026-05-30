using HR_LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Contracts.Persistences
{
    public interface IAuditService
    {
        Task LogAsync(string userId, string email, string path);
        Task SuccessfulRegistrationAsync(string userId, string email);
        Task FailedRegistrationAsync(string userId, string email);
        Task SuccessfulLoginAsync(string userId, string email);
        Task FailedLoginAsync(string userId, string email);
        Task RefreshTokenAsync(string userId, string email);
        Task PasswordResetAsync(string userId, string email);
        Task RoleChangeAsync(string userId, string email);
        Task PermissionChangeAsync(string userId, string email);
    }
}
