using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Contracts.Persistences
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermission>
    {
        Task RemoveDataRange(ICollection<RolePermission> rolePermissions);
        Task AddDataRange(IEnumerable<RolePermission> rolePermissions);
        Task<List<Permission>> GetRolePermissionsByRoleId(int roleId);
    }
}
