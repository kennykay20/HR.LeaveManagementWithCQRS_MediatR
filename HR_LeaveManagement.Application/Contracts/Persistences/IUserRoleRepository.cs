using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Contracts.Persistences
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task RemoveDataRange(ICollection<UserRole> userRoles);
        Task AddDataRange(IEnumerable<UserRole> userRoles);
        Task<List<Role>> GetUserRolesByUserId(int userId);
    }
}
