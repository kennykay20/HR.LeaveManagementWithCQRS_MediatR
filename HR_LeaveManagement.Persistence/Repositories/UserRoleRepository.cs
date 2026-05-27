using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence.Repositories
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        private readonly HRLeaveManagementDbContext _dbContext;
        private readonly ILogger<UserRoleRepository> _logger;

        public UserRoleRepository(
            HRLeaveManagementDbContext dbContext, 
            ILogger<UserRoleRepository> logger
            )
            : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddDataRange(IEnumerable<UserRole> userRoles)
        {
            await _dbContext.UserRoles.AddRangeAsync(userRoles);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveDataRange(ICollection<UserRole> userRoles)
        {
            _dbContext.UserRoles.RemoveRange(userRoles);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Role>> GetUserRolesByUserId(int userId)
        {
            var roles = await _dbContext.UserRoles
                        .Where(user => user.UserId == userId)
                        .Select(role => new Role
                        {
                            Id = role.Role.Id,
                            Name = role.Role.Name,
                            Description = role.Role.Description
                        }).ToListAsync();

            return roles;
        }
    }
}
