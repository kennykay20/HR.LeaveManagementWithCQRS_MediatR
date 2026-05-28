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
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly HRLeaveManagementDbContext _dbContext;

        public RoleRepository(HRLeaveManagementDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role?> GetRolePermissionsByRoleId(int roleId)
        {
            var role = await _dbContext.Roles.Include(role => role.RolePermissions)
                            .FirstOrDefaultAsync(role => role.Id == roleId && role.IsDeleted == false);

            return role ?? null;
        }
    }
}
