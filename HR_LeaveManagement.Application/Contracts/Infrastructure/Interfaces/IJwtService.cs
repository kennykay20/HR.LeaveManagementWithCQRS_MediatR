using HR_LeaveManagement.Application.DTOs;
using HR_LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces
{
    public interface IJwtService
    {
        string[] GetSecretKeys();
        string GenerateAccessToken(User user, IList<Claim> claims);
        string GenerateRefreshToken();
        JwtValidationResultDto ValidateAndExtractToken(string jwtToken);
    }
}
