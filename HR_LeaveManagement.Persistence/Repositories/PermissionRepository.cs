using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence.Repositories
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        private readonly HRLeaveManagementDbContext _dbContext;

        public PermissionRepository(HRLeaveManagementDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
