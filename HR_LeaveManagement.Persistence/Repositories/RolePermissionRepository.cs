using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence.Repositories
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly HRLeaveManagementDbContext _dbContext;

        public RolePermissionRepository(HRLeaveManagementDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddDataRange(IEnumerable<RolePermission> rolePermissions)
        {
            await _dbContext.RolePermissions.AddRangeAsync(rolePermissions);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Permission>> GetRolePermissionsByRoleId(int roleId)
        {
            var permissions = await _dbContext.RolePermissions
                        .Where(role => role.RoleId == roleId)
                        .Select(permission => new Permission
                        {
                            Id = permission.Permission.Id,
                            Name = permission.Permission.Name,
                            Description = permission.Permission.Description
                        }).ToListAsync();

            return permissions;
        }

        public async Task RemoveDataRange(ICollection<RolePermission> rolePermissions)
        {
            _dbContext.RolePermissions.RemoveRange(rolePermissions);
            await _dbContext.SaveChangesAsync();
        }
    }
}
