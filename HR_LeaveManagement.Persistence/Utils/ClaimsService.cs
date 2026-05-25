using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence.Utils
{
    public class ClaimsService : IClaimsService
    {
        private readonly HRLeaveManagementDbContext _dbContext;

        public ClaimsService(HRLeaveManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Claim>> GetUserClaimsAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _dbContext.UserRoles
            .Where(x => x.UserId == user.Id)
            .Select(x => x.Role)
            .ToListAsync();

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var permissions = await _dbContext.RolePermissions
            .Where(x => roles.Select(r => r.Id).Contains(x.RoleId))
            .Select(x => x.Permission.Name)
            .Distinct()
            .ToListAsync();

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            return claims;
        }
    }
}
